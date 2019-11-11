namespace DeriSock
{
    using Converter;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Model;
    using System.Threading;
    using ClearLog;

    public class DeribitApiV2 : JsonRpcWebSocketClient
    {
        private readonly List<Tuple<string, object, object>> _listeners;

        private string _accessToken;
        private string _refreshToken;
        private Logger log;
        public DeribitApiV2(string hostname, Logger log) : base($"wss://{hostname}/ws/api/v2", log)
        {
            this.log = log;
            _listeners = new List<Tuple<string, object, object>>();
        }



        static private ManualResetEvent mre = new ManualResetEvent(true);////dave added
        public string AccessToken
        {
            get
            {
                mre.WaitOne();
                return _accessToken;
            }
            set
            {
                _accessToken = value;
            }
        }




        #region Helpers




        private async Task<bool> PublicSubscribeAsync<T>(string channelName, Action<T> originalCallback, Action<EventResponse> myCallback)
        {
            if (!await ManagedSubscribeAsync(channelName, false, null, myCallback)) return false;
            lock (_listeners)
            {
                _listeners.Add(Tuple.Create(channelName, (object)originalCallback, (object)myCallback));
            }
            return true;
        }

        private async Task<bool> UnsubscribePublicAsync<T>(string channelName, Action<T> originalCallback)
        {
            Tuple<string, object, object> entry;
            lock (_listeners)
            {
                entry = _listeners.FirstOrDefault(x => x.Item1 == channelName && x.Item2 == (object)originalCallback);
            }
            if (entry == null) return false;
            if (!await ManagedUnsubscribeAsync(channelName, false, null, (Action<EventResponse>)entry.Item3)) return false;
            lock (_listeners)
            {
                _listeners.Remove(entry);
            }
            return true;
        }

        private async Task<bool> PrivateSubscribeAsync<T>(string channelName, Action<T> originalCallback, Action<EventResponse> myCallback)
        {
            if (!await ManagedSubscribeAsync(channelName, true, AccessToken, myCallback)) return false;
            lock (_listeners)
            {
                _listeners.Add(Tuple.Create(channelName, (object)originalCallback, (object)myCallback));
            }
            return true;
        }

        private async Task<bool> UnsubscribePrivateAsync<T>(string channelName, Action<T> originalCallback)
        {
            Tuple<string, object, object> entry;
            lock (_listeners)
            {
                entry = _listeners.FirstOrDefault(x => x.Item1 == channelName && x.Item2 == (object)originalCallback);
            }
            if (entry == null) return false;
            if (!await ManagedUnsubscribeAsync(channelName, true, AccessToken, (Action<EventResponse>)entry.Item3)) return false;
            lock (_listeners)
            {
                _listeners.Remove(entry);
            }
            return true;
        }

        #endregion

        public async Task<AuthResponse> PublicAuthAsync(string accessKey, string accessSecret, string sessionName)
        {
            log.Info("Authenticate");
            var scope = "connection";
            if (!string.IsNullOrEmpty(sessionName))
            {
                scope = $"session:{sessionName} expires:60";
            }
            var loginRes = await SendAsync("public/auth", new
            {
                grant_type = "client_credentials",
                client_id = accessKey,
                client_secret = accessSecret,
                scope = scope
            }, new ObjectJsonConverter<AuthResponse>());
            AccessToken = loginRes.access_token;
            _refreshToken = loginRes.refresh_token;
            _ = Task.Delay(TimeSpan.FromSeconds(loginRes.expires_in - 5)).ContinueWith(t =>
            {
                if (IsConnected) _ = PublicAuthRefreshAsync(_refreshToken);
            });
            return loginRes;
        }

        public async Task<AuthResponse> PublicAuthRefreshAsync(string refreshToken)
        {
            log.Info("Refreshing Auth: Start");

            mre.Reset();
            var loginRes = await SendAsync("public/auth", new
            {
                grant_type = "refresh_token",
                refresh_token = refreshToken
            }, new ObjectJsonConverter<AuthResponse>());

            AccessToken = loginRes.access_token;
            _refreshToken = loginRes.refresh_token;
            mre.Set();
            log.Info("Refreshing Auth: End");


            _ = Task.Delay(TimeSpan.FromSeconds(loginRes.expires_in - 5)).ContinueWith(t =>
            {
                if (IsConnected) _ = PublicAuthRefreshAsync(_refreshToken);
            });
            return loginRes;
        }

        public Task<object> PublicSetHeartbeatAsync(int intervalSeconds)
        {
            return SendAsync("public/set_heartbeat", new { interval = intervalSeconds }, new ObjectJsonConverter<object>());
        }

        public Task<object> PublicDisableHeartbeatAsync()
        {
            return SendAsync("public/disable_heartbeat", new { }, new ObjectJsonConverter<object>());
        }

        public Task<object> PrivateDisableCancelOnDisconnectAsync()
        {
            return SendAsync("private/disable_cancel_on_disconnect", new { access_token = AccessToken }, new ObjectJsonConverter<object>());
        }

        public Task<AccountSummaryResponse> PrivateGetAccountSummaryAsync()
        {
            return SendAsync("private/get_account_summary", new { currency = "BTC", extended = true, access_token = AccessToken }, new ObjectJsonConverter<AccountSummaryResponse>());
        }


        public Task<bool> PrivateSubscribeUserChanges(string instrument, Action<UserChanges> callback, out string channel)
        {
            channel = "user.changes." + instrument + ".100ms";

            return PrivateSubscribeAsync(channel, callback, response =>
            {
                callback(response.@params.data.ToObject<UserChanges>());
            });
        }


        public Task<bool> PublicSubscribeBookAsync(string instrument, int group, int depth, Action<MarketBook> callback, out string channel)
        {
            channel = "book." + instrument + "." + (group == 0 ? "none" : group.ToString()) + "." + depth + ".100ms";

            return PublicSubscribeAsync(channel, callback, response =>
            {
                callback(response.@params.data.ToObject<MarketBook>());
            });
        }



        public Task<bool> PublicSubscribeTradeAsync(string instrument, int group, int depth, Action<MarketTrade[]> callback, out string channel)
        {
            channel = "trades." + instrument + ".raw";
            return PublicSubscribeAsync(channel, callback, response =>
            {
                callback(response.@params.data.ToObject<MarketTrade[]>());
            });
        }

        public Task<bool> PrivateSubscribeOrdersAsync(string instrument, Action<OrderResponse> callback)
        {
            return PrivateSubscribeAsync("user.orders." + instrument + ".raw", callback, response =>
            {
                var orderResponse = response.@params.data.ToObject<OrderResponse>();
                orderResponse.timestamp = response.@params.timestamp;
                callback(orderResponse);
            });
        }

        public Task<bool> PrivateSubscribePortfolioAsync(string currency, Action<PortfolioResponse> callback)
        {
            return PrivateSubscribeAsync($"user.portfolio.{currency.ToLower()}", callback, response =>
            {
                callback(response.@params.data.ToObject<PortfolioResponse>());
            });
        }

        public Task<bool> PublicSubscribeTickerAsync(string instrument, string interval, Action<TickerResponse> callback)
        {
            return PublicSubscribeAsync($"ticker.{instrument}.{interval}", callback, response =>
            {
                callback(response.@params.data.ToObject<TickerResponse>());
            });
        }

        public Task<BookResponse> PublicGetOrderBookAsync(string instrument, int depth)
        {
            return SendAsync("public/get_order_book", new
            {
                instrument_name = instrument,
                depth
            }, new ObjectJsonConverter<BookResponse>());
        }

        public Task<OrderItem[]> PrivateGetOpenOrdersAsync(string instrument)
        {
            return SendAsync("private/get_open_orders_by_instrument", new
            {
                instrument_name = instrument,
                access_token = AccessToken
            }, new ObjectJsonConverter<OrderItem[]>());
        }

        public Task<BuySellResponse> PrivateBuyLimitAsync(string instrument, double amount, double price, string label)
        {
            return SendAsync("private/buy", new
            {
                instrument_name = instrument,
                amount,
                type = "limit",
                label = label,
                price,
                time_in_force = "good_til_cancelled",
                post_only = true,
                access_token = AccessToken
            }, new ObjectJsonConverter<BuySellResponse>());
        }


        public Task<GetPosition> PrivateGetPosition(string instrument)
        {
            return SendAsync("private/" + "get_position", new
            {
                instrument_name = instrument,
                access_token = AccessToken
            }, new ObjectJsonConverter<GetPosition>());
        }

        public Task<BuySellResponse> PrivateOrderAsync(string instrument, string buy_sell, string market_limit, string label, double amount, double price = 0)
        {
            if (market_limit == "limit")
            {
                return SendAsync("private/" + buy_sell, new
                {
                    instrument_name = instrument,
                    amount,
                    type = market_limit,
                    label = label,
                    price,
                    time_in_force = "good_til_cancelled",
                    post_only = true,
                    access_token = AccessToken
                }, new ObjectJsonConverter<BuySellResponse>());
            }
            else
            {
                return SendAsync("private/" + buy_sell, new
                {
                    instrument_name = instrument,
                    amount,
                    type = market_limit,
                    label = label,
                    price,
                    time_in_force = "good_til_cancelled",
                    //post_only = true,
                    access_token = AccessToken
                }, new ObjectJsonConverter<BuySellResponse>());
            }
        }


        public Task<BuySellResponse> PrivateSellLimitAsync(string instrument, double amount, double price, string label)
        {
            return SendAsync("private/sell", new
            {
                instrument_name = instrument,
                amount,
                type = "limit",
                label = label,
                price,
                time_in_force = "good_til_cancelled",
                post_only = true,
                access_token = AccessToken
            }, new ObjectJsonConverter<BuySellResponse>());
        }

        public async Task<OrderResponse> PrivateGetOrderStateAsnyc(string orderId)
        {
            try
            {
                var result = await SendAsync("private/get_order_state", new
                {
                    order_id = orderId,
                    access_token = AccessToken
                }, new ObjectJsonConverter<OrderResponse>());
                return result;
            }
            catch
            {
                return null;
            }
        }


        public Task<BuySellResponse> PrivateEditAsync(string orderId, double amount, double price)
        {
            return SendAsync("private/edit", new
            {
                order_id = orderId,
                amount,
                price,
                access_token = AccessToken
            }, new ObjectJsonConverter<BuySellResponse>());
        }

        public Task<object> PrivateCancelOrderAsync(string orderId)
        {
            return SendAsync("private/cancel", new
            {
                order_id = orderId,
                access_token = AccessToken
            }, new ObjectJsonConverter<object>());
        }

        public Task<object> PrivateCancelAllOrdersByInstrumentAsync(string instrument)
        {
            return SendAsync("private/cancel_all_by_instrument", new
            {
                instrument_name = instrument,
                access_token = AccessToken
            }, new ObjectJsonConverter<object>());
        }

        public Task<SettlementResponse> PrivateGetSettlementHistoryByInstrumentAsync(string instrument, int count)
        {
            return SendAsync("private/get_settlement_history_by_instrument", new
            {
                instrument_name = instrument,
                type = "settlement",
                count = count,
                access_token = AccessToken
            }, new ObjectJsonConverter<SettlementResponse>());
        }
          
        public Task<SettlementResponse> PrivateGetSettlementHistoryByCurrencyAsync(string currency, int count)
        {
            return SendAsync("private/get_settlement_history_by_currency", new
            {
                currency = currency,
                type = "settlement",
                count = count,
                access_token = AccessToken
            }, new ObjectJsonConverter<SettlementResponse>());
        }
    }
}
