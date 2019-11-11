namespace DeriSock.Model
{
    using System.Collections.Generic;
    using System.Linq;


    public class GetPosition
    {
        public string instrument_name;
        public int size;
    }
    public class MarketBook
    {
        public List<double[]> asks;
        public List<double[]> bids;
        public long change_id;
        public string instrument_name;//dave
        public long timestamp;//dave

        public double Bid { 
            get => bids[0][0]; 
        }
        public double Ask { 
            get => asks[0][0]; 
        }

        public string ToString(string instrument)
        {
            return $"{instrument}, {change_id}, BID: {Bid:0.00}, ASK: {Ask:0.00}";
        }
    }

    public class UserChanges
    {
        public List<UserPosition> positions;//as call back function, can't set as private
        public List<UserOrder> orders;//as call back function, can't set as private
        public List<UserTrade> trades;//as call back function, can't set as private

        public UserPosition public_position //只留最新一筆
        {
            get
            {
                if (positions != null && positions.Count > 0)
                    return this.positions[this.positions.Count - 1];
                else
                    return null;
            }
        }

        public List<UserOrder> public_orders_filter//去掉重複的 order id，都只留最新一筆
        {
            get
            {
                if (orders != null && orders.Count > 0)
                {
                    return (
                        from order in this.orders
                        group order by order.order_id into g
                        select g.Last()
                                        ).ToList();
                }
                else
                {
                    return null;
                }
            }
        }


        public List<UserTrade>[] public_trades_group//照order id分類
        {
            get
            {
                if (trades != null && trades.Count > 0)
                {
                    return (
                                from trade in this.trades
                                group trade by trade.order_id into g
                                select g.ToList()
                              ).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }
    }


    public enum tick_direction
    {
        //Direction of the "tick" (0 = Plus Tick, 1 = Zero-Plus Tick, 2 = Minus Tick, 3 = Zero-Minus Tick).
        PlusTick = 0,
        ZeroPlusTick = 1,
        MinusTick = 2,
        ZeroMinusTick = 3,
    }

    public enum state
    {
        open,
        filled,
        rejected,
        cancelled,
        untriggered,
        //archive,    removed so that trade and order use same enum
    }
    //public enum order_state
    //{
    //    open,
    //    filled,
    //    rejected,
    //    cancelled,
    //    untriggered,
    //}

    public enum liquidity
    {
        //Describes what was role of users order: "M" when it was maker order, "T" when it was taker order
        M,
        T,
    }

    public enum direction
    {
        //direction, buy or sell
        buy,
        sell,
    }
    public enum fee_currency
    {
        BTC,
        ETH,
    }

    public class UserOrder
    {
        //advanced	string	advanced type: "usd" or "implv" (Only for options; field is omitted if not applicable).
        public double amount;
        // 0915 public bool api;
        public double average_price;
        // 0915 public double commission;
        // 0915 public long creation_timestamp;
        // 0915 public direction direction;
        public double filled_amount;
        //implv	number	Implied volatility in percent. (Only if advanced="implv")
        public string instrument_name;
        // 0915 public bool is_liquidation;
        public string label;
        public long last_update_timestamp;
        // 0915 public int max_show;
        public string order_id;
        public state order_state;
        // 0915 public order_type order_type;
        // original_order_type	string	Original order type. Optional field
        // 0915 public bool post_only;
        public double price;
        // 0915 public double profit_loss;
        // 0915 public bool reduce_only;
        //stop_price	number	stop price (Only for future stop orders)
        // 0915 time_in_force time_in_force;
        //trigger	string	Trigger type (Only for stop orders). Allowed values: "index_price", "mark_price", "last_price".
        //triggered	boolean	Whether the stop order has been triggered (Only for stop orders)
        //usd	number	Option price in USD (Only if advanced="usd")
    }
    public class UserPosition
    {
        // 0915 public double average_price;
        //average_price_usd	number	Only for options, average price in USD
        // 0915 public double delta;
        // 0915 public direction direction;
        // 0915 public double estimated_liquidation_price;
        // 0915 public double floating_profit_loss;
        //floating_profit_loss_usd	number	Only for options, floating profit or loss in USD
        // 0915 public double index_price;
        // 0915 public double initial_margin;
        public string instrument_name;
        // 0915 public kind kind;
        // 0915 public double maintenance_margin;
        // 0915 public double mark_price;
        // 0915 public double open_orders_margin;
        // 0915 public double realized_profit_loss;
        // 0915 public double settlement_price;
        public double size;
        // 0915 public double size_currency;
        // 0915 public double total_profit_loss;

    }

    public class UserTrade
    {
        public double amount;
        // 0915 public direction direction;
        // 0915 public double fee;
        // 0915 public fee_currency fee_currency;
        // 0915 public double index_price;
        public string instrument_name;
        //iv	number	Option implied volatility for the price (Option only)
        // 0915 public string label;
        //liquidation	string	Optional field (only for trades caused by liquidation): "M" when maker side of trade was under liquidation, "T" when taker side was under liquidation, "MT" when both sides of trade were under liquidation
        // 0915  public liquidity liquidity;
        // 0915 public string matching_id;
        public string order_id;
        // 0915 public order_type order_type;
        public double price;
        // 0915 public bool self_trade;
        // 0915 public state state;
        // 0915 public tick_direction tick_direction;
        public long timestamp;
        // 0915 public string trade_id;
        // 0915 public long trade_seq;


        //bool reduce_only;  no mentions
        //bool post_only;  no mentions
    }

    public enum kind
    {
        future,
        option,
    }



    //public enum order_type
    //{
    //    //order type, "limit", "market", "stop_limit", "stop_market"
    //    limit,
    //    market,

    //    stop_limit,
    //    stop_market,
    //}

    //public enum order_type
    //{
    //    //Order type: "limit, "market", or "liquidation"
    //    limit,
    //    market,
    //    liquidation,
    //}


    public enum order_type
    {
        limit,
        market,
        //stop_limit,  for UserOrder class only
        //stop_market,  for UserOrder class only
        //liquidation,  fore UserTrade class only
    }



    public enum time_in_force
    {
        //Order time in force: "good_til_cancelled", "fill_or_kill", "immediate_or_cancel"

        good_til_cancelled,
        fill_or_kill,
        immediate_or_cancel,
    }




    public class MarketTrade
    {
        //public long tradeId;
        //public string instrument;
        //public long timeStamp;
        //public long tradeSeq;
        //public double quantity;
        //public double amount;
        //public double price;
        //public string direction;
        //public int tickDirection;
        //public double indexPrice;



        //dave
        //public string method;
        //public string channel;

        public int trade_seq;
        public string trade_id;
        public tick_direction tick_direction;
        public double amount;
        public double price;
        public double index_price;
        public string instrument_name;
        public direction direction;
        public long timestamp;
        //dave
    }
}
