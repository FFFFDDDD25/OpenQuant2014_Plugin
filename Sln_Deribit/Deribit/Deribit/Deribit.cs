
using System;
using System.Linq;
using DeriSock;
using DeriSock.Model;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ClearLog;
using MKD;
using IF;
using OpenQunatFunction;


namespace SmartQuant.DB
{
    public class MarketInfo
    {
        public Instrument inst;
        public MarketBook br;
        public MarketTrade tr;
        public UserPosition up;
        public BarMaker bm;
        public string[] chs;
        public MarketInfo(Instrument inst, BarMaker bm, string[] chs)
        {
            this.inst = inst;
            this.br = null;
            this.tr = null;
            this.chs = chs;
            this.up = null;
            this.bm = bm;
        }
    }
    public class OrderInfo
    {
        public UserOrder order;
        public List<UserTrade> trade = new List<UserTrade>();
        public ExecutionCommand command;
        public Order order_dll;

        public OrderInfo(ExecutionCommand command, Order order_dll)
        {
            this.command = command;
            this.order_dll = order_dll;
        }
    }

    public class Deribit : Provider, IInstrumentProvider, IHistoricalDataProvider, IDataProvider, IExecutionProvider//, ICommissionProvider
    {
        void IInstrumentProvider.Cancel(string requestId)
        {
        }
        void IHistoricalDataProvider.Cancel(string requestId)
        {
        }

        DeribitApiV2 api;
        MultiKeyDictionary<string, int, OrderInfo> m_orders = new MultiKeyDictionary<string, int, OrderInfo>();

        ConcurrentDictionary<string, MarketInfo> m_subscribedInst = new ConcurrentDictionary<string, MarketInfo>();
        Logger log;
        IniFile ini;

        public Deribit(Framework framework) : base(framework)
        {
            base.id = 89;  
            base.name = "Deribit";

            string enableDeribit = _Global.Func.InputBox("Enable Deribit?", "Y");
            if (enableDeribit != "Y")
                return;

            log = new Logger(this.GetType().Name);
            ini = new IniFile(this.GetType().Name);

            api = new DeribitApiV2(ini.ReadOrWrite("Server", "test.deribit.com"), log);
            api.ConnectAsync().Wait();
            api.PublicAuthAsync(
                ini.ReadOrWrite("account", "這邊填入申請好的deribit測試用帳號"),
                ini.ReadOrWrite("password", "這邊填入申請好的deribit測試用密碼"),
                "apiconsole-mwc6qnvtf5"
                ).Wait();
        }

        public override void Connect()
        {
            base.Connect();
        }

        //public override void Disconnect()
        //{
        //    api.DisconnectAsync().Wait();
        //    base.Disconnect();
        //}


        public override void Send(ExecutionCommand command)
        {
            log.Info("Send(ExecutionCommand command):" + command.Type + " OQ order id:" + command.Order.Id);
            try
            {
                switch (command.Type)
                {
                    case ExecutionCommandType.Send:
                        {
                            log.Info("Send Order " + " OQ order id:" + command.Order.Id);

                            string temp_id = Guid.NewGuid().ToString().Substring(0, 5);  //deribit max length is 32
                            string market_limit = (command.OrdType == SmartQuant.OrderType.Limit ? order_type.limit.ToString() : order_type.market.ToString());
                            string buy_sell = (command.Side == OrderSide.Buy ? direction.buy.ToString() : direction.sell.ToString());

                            m_orders.Add(temp_id, command.Order.Id, new OrderInfo(command, command.Order));
                            Task<BuySellResponse> rtn = api.PrivateOrderAsync(command.Instrument.Symbol, buy_sell, market_limit, temp_id, command.Qty, command.Price);
                            break;
                        }

                    case ExecutionCommandType.Cancel:
                        {
                            OrderInfo oi = m_orders[command.Order.Id];
                            if (oi.order != null)
                            {
                                Task<object> rtn = api.PrivateCancelOrderAsync(oi.order.order_id);
                            }
                            else
                            {
                                log.Error("cancel unknow order, OQ id:" + oi.order_dll.Id);
                            }
                            break;
                        }

                    case ExecutionCommandType.Replace:
                        {
                            OrderInfo oi = m_orders[command.Order.Id];
                            if (oi.order != null)
                            {
                                Task<BuySellResponse> rtn = api.PrivateEditAsync(oi.order.order_id, command.Qty, command.Price);
                            }
                            else
                            {
                                log.Error("replace unknow order, OQ id:" + oi.order_dll.Id);
                            }
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                log.Error(command.Type + " fail, msg:" + e.ToString());
            }
        }

        public void SubscribeInstru_1(MarketTrade[] ti_calbak)
        {
            MarketTrade tr_last = ti_calbak.Last();
            log.Info("receive new trade data, symbol:" + tr_last.instrument_name + " price:" + tr_last.price + " trade_seq:" + tr_last.trade_seq);

            m_subscribedInst.TryGetValue(tr_last.instrument_name, out MarketInfo mi);

            mi.tr = tr_last;

            Trade trd = new Trade(DateTime.Now, this.id, mi.inst.Id, tr_last.price, (int)tr_last.amount);
            
            base.EmitData(trd, true);

            mi.bm.Input(tr_last.price, (int)tr_last.amount);/*will do base.EmitData(bar, true);*/
        }


        public void SubscribeInstru_2(MarketBook br_calbak)
        {
            log.Info("receive new oder data, symbol:" + br_calbak.instrument_name + " change_id:" + br_calbak.change_id);
            m_subscribedInst.TryGetValue(br_calbak.instrument_name, out MarketInfo bi);
            bi.br = br_calbak;

            int bid_count = br_calbak.bids.Count();
            int ask_count = br_calbak.asks.Count();

            Bid[] bids = new Bid[bid_count];
            Ask[] asks = new Ask[ask_count];

            for (int i = 0; i < bid_count; i++)
            {
                bids[i] = new Bid(DateTime.Now, this.id, bi.inst.Id, br_calbak.bids[i][0], (int)br_calbak.bids[i][1]);
            }

            for (int i = 0; i < ask_count; i++)
            {
                asks[i] = new Ask(DateTime.Now, this.id, bi.inst.Id, br_calbak.asks[i][0], (int)br_calbak.asks[i][1]);
            }



            Level2Snapshot snap = new Level2Snapshot(
                DateTime.Now,
                this.id,
                bi.inst.Id,
                bids,
                asks
            );

            if (bid_count > 0)
                base.EmitData(bids[0], true);

            if (ask_count > 0)
                base.EmitData(asks[0], true);

            if (ask_count > 0 || bid_count > 0)
                base.EmitData(snap, true);


            //emit snap & emit aks/bid will trigger different callback function in strategy
        }


        public void SubscribeInstru_3(UserChanges uc_calbak)
        {
            UserPosition pos = uc_calbak.public_position;
            List<UserOrder> ords = uc_calbak.public_orders_filter;
            List<UserTrade>[] trds_arr = uc_calbak.public_trades_group;

            if (pos != null)
            {  
                //log.Info("update subscribed floating_profit_loss in " + pos.instrument_name + ": " + pos.floating_profit_loss);
                //log.Info("update subscribed total_profit_loss in " + pos.instrument_name + ": " + pos.total_profit_loss);

                if (m_subscribedInst.TryGetValue(pos.instrument_name, out MarketInfo mbi))
                {
                    mbi.up = pos;
                    log.Info("update subscribed position count in " + pos.instrument_name + ": " + pos.size);
                    EmitData(OQFunc.GetPositionData(pos.instrument_name, Convert.ToInt32(pos.size), framework, this.id));
                }
                else
                {
                    log.Info("update non-subscribed position count in " + pos.instrument_name + ": " + pos.size);
                }

            }



            if (ords != null && ords.Count > 0)
            {
                OrderInfo moi;
                foreach (UserOrder ord in ords)
                {
                    if (m_orders.ContainsKey(ord.order_id))//(m_orders.TryGetValue(ord.order_id, out moi))
                    {
                    }
                    else if (m_orders.TryGetValue(ord.label, out moi))
                    {
                        int subKey = m_orders.primaryToSubkeyMapping[ord.label];
                        m_orders.Remove(ord.label);
                        m_orders.Add(ord.order_id, subKey, moi);

                        string commant = "New Order received, OQ id:" + subKey + " Provider temp id:" + ord.label + " Provider id:" + ord.order_id;
                        log.Info(commant);

                        moi.order = ord;
                        MakeExecutionReport(moi, ord, OrderStatus.New, ord.last_update_timestamp, commant);
                    }
                    else
                    {
                        //this order is not send by OQ since program start
                        log.Info("unknow order in " + ord.instrument_name + "with Provider id " + ord.order_id);
                    }
                }
            }

            if (trds_arr != null && trds_arr.Length > 0)
            {
                foreach (List<UserTrade> trds in trds_arr)
                {
                    if (m_orders.TryGetValue(trds[0].order_id, out OrderInfo oi))
                    {
                        oi.trade.AddRange(trds);

                        var matched_qty = oi.trade.Select(o => o.amount).Sum();


                        UserTrade trd_last = trds.Last();
                        foreach (UserTrade trd in trds)
                        {
                            if (trd.amount <= 0)
                            {
                                log.Error("unknow trade msg");
                            }
                            else if (trd.Equals(trd_last) && matched_qty == oi.order.amount)
                            {
                                string commant = "All Filled Order received, OQ id:" + oi.order_dll.Id + " Provider id:" + trd.order_id;
                                log.Info(commant);
                                MakeExecutionReport(oi, trd, OrderStatus.Filled, trd.timestamp, commant);
                            }
                            else
                            {
                                string commant = "Partially Filled Order received, OQ id:" + oi.order_dll.Id + " Provider id:" + trd.order_id;
                                log.Info(commant);
                                MakeExecutionReport(oi, trd, OrderStatus.PartiallyFilled, trd.timestamp, commant);
                            }
                        }
                    }
                    else
                    {
                        //this order is not send by OQ since program start
                        log.Info("unknow trade in " + trds[0].instrument_name + " with order id " + trds[0].order_id);
                    }
                }
            }


            if (ords != null && ords.Count > 0)
            {
                OrderInfo moi;
                foreach (UserOrder ord in ords)
                {
                    if (m_orders.TryGetValue(ord.order_id, out moi))
                    {
                        int subKey = m_orders.primaryToSubkeyMapping[ord.order_id];

                        string commant = "Order received, OQ id:" + subKey + " Provider temp id:" + ord.label + " Provider id:" + ord.order_id;

                        if ((moi.order != null) && (moi.order.price != ord.price || moi.order.amount != ord.amount))
                        {
                            log.Info("Replace " + commant);
                            MakeExecutionReport(moi, ord, OrderStatus.Replaced, ord.last_update_timestamp, "Replace " + commant);
                        }

                        if (ord.order_state == state.cancelled)
                        {
                            log.Info("Cancel " + commant);
                            MakeExecutionReport(moi, ord, OrderStatus.Cancelled, ord.last_update_timestamp, "Cancel " + commant);
                            m_orders.Remove(ord.order_id);
                        }
                        else if (ord.order_state == state.filled)
                        {
                            log.Info("Fill " + commant);
                            //MakeExecutionReport(moi, ord, OrderStatus.Filled, ord.last_update_timestamp, "Fill " + commant);
                            m_orders.Remove(ord.order_id);
                        }
                        else if (ord.order_state == state.rejected)
                        {
                            log.Error("Reject " + commant);
                            MakeExecutionReport(moi, ord, OrderStatus.Rejected, ord.last_update_timestamp, "Reject " + commant);
                            m_orders.Remove(ord.order_id);
                        }

                        moi.order = ord;
                    }
                }
            }
        }





        void MakeExecutionReport(OrderInfo oi, UserTrade trade, SmartQuant.OrderStatus orderStatus, long unixTime, string commant)
        {

            ExecType execType = ExecType.ExecTrade;
            if (orderStatus == OrderStatus.Cancelled)
            {
                execType = ExecType.ExecCancelled;
            }
            else if (orderStatus == OrderStatus.Filled || orderStatus == OrderStatus.PartiallyFilled)
            {
                execType = ExecType.ExecTrade;
            }
            else if (orderStatus == OrderStatus.New)
            {
                execType = ExecType.ExecNew;
            }
            else if (orderStatus == OrderStatus.Rejected)
            {
                execType = ExecType.ExecRejected;
            }
            else if (orderStatus == OrderStatus.Replaced)
            {
                execType = ExecType.ExecReplace;
            }


            DateTime transactTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime.ToLocalTime();
            oi.order_dll.TransactTime = transactTime;


            ExecutionReport executionReport = new ExecutionReport
            {
                DateTime = this.framework.Clock.DateTime,
                OrderId = oi.order_dll.Id,
                //ProviderOrderId = moi.command.OrderId.ToString(),
                Order = oi.order_dll,
                Instrument = oi.order_dll.Instrument,
                Side = oi.order_dll.Side,
                OrdType = oi.order_dll.Type,
                TimeInForce = oi.order_dll.TimeInForce,

                OrdQty = -1,
                Price = -1,
                StopPx = oi.order_dll.StopPx,
                AvgPx = -1,
                CumQty = -1,
                LeavesQty = -1,
                //TransactTime = transactTime,

                ExecType = execType,
                OrdStatus = orderStatus,

                Text = commant,
                LastPx = trade.price,
                LastQty = trade.amount
            };

            base.EmitExecutionReport(executionReport, false);

            log.Info(
                "unixTime:" + unixTime + "    " +
                "orderStatus:" + orderStatus + "    " +
                "trade.price:" + trade.price + "    " +
                "trade.amount:" + trade.amount
                );
        }

        void MakeExecutionReport(OrderInfo moi, UserOrder order, SmartQuant.OrderStatus orderStatus, long unixTime, string commant)
        {

            ExecType execType = ExecType.ExecTrade;
            if (orderStatus == OrderStatus.Cancelled)
            {
                execType = ExecType.ExecCancelled;
            }
            else if (orderStatus == OrderStatus.Filled || orderStatus == OrderStatus.PartiallyFilled)
            {
                execType = ExecType.ExecTrade;
            }
            else if (orderStatus == OrderStatus.New)
            {
                execType = ExecType.ExecNew;
            }
            else if (orderStatus == OrderStatus.Rejected)
            {
                execType = ExecType.ExecRejected;
            }
            else if (orderStatus == OrderStatus.Replaced)
            {
                execType = ExecType.ExecReplace;
            }


            DateTime transactTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime.ToLocalTime();
            SmartQuant.Order Order_dll = moi.command.Order;
            Order_dll.TransactTime = transactTime;

            double lastPrice = -1;
            double lastQty = -1;
            if (moi.trade != null && moi.trade.Count > 1)
            {
                lastPrice = moi.trade.Last().price;
                lastQty = moi.trade.Last().amount;
            }






            ExecutionReport executionReport = new ExecutionReport
            {
                DateTime = this.framework.Clock.DateTime,
                OrderId = Order_dll.Id,
                ProviderOrderId = moi.command.OrderId.ToString(),
                Order = Order_dll,
                Instrument = Order_dll.Instrument,
                Side = Order_dll.Side,
                OrdType = Order_dll.Type,
                TimeInForce = Order_dll.TimeInForce,

                OrdQty = order.amount,
                Price = order.price,//
                StopPx = Order_dll.StopPx,//
                AvgPx = order.average_price,//
                CumQty = order.filled_amount,//
                LeavesQty = order.amount - order.filled_amount,//
                //TransactTime = transactTime,

                OrdStatus = orderStatus,
                ExecType = execType,

                Text = commant,
                LastPx = lastPrice,
                LastQty = lastQty
            };
            base.EmitExecutionReport(executionReport, false);


            log.Info(
                "unixTime:" + unixTime + "    " +
                "orderStatus:" + orderStatus + "    " +
                "order.amount:" + order.amount + "    " +
                "order.filled_amount:" + order.filled_amount + "    " +
                "order.price:" + order.price + "    " +
                "order.average_price:" + order.average_price
                );
        }



        bool isOK = true;
        public override void Subscribe(Instrument inst)
        {
            if (m_subscribedInst.ContainsKey(inst.Symbol))
            {
                log.Info("already subscribe:" + inst.Symbol);
                return;
            }


            isOK = true;
            log.Info("----------------- Subscribe:" + inst.Symbol + " -----------------");



            Task<bool> rtn1 = api.PublicSubscribeTradeAsync(inst.Symbol, 0, 10, SubscribeInstru_1, out string ch1);
            Task<bool> rtn2 = api.PublicSubscribeBookAsync(inst.Symbol, 0, 10, SubscribeInstru_2, out string ch2);
            Task<bool> rtn3 = api.PrivateSubscribeUserChanges(inst.Symbol, SubscribeInstru_3, out string ch3);
            Task<GetPosition> rtn4 = api.PrivateGetPosition(inst.Symbol);

            m_subscribedInst.TryAdd(inst.Symbol, new MarketInfo(
                inst,
                new BarMaker(
                    int.Parse(ini.ReadOrWrite("bar interval", "10")), 
                    (BarType)Enum.Parse(typeof(BarType), ini.ReadOrWrite("bar type", nameof(BarType.Time))),
                    base.EmitData, inst.Id),
                new string[] { ch1, ch2, ch3 }
                ));

            bool wait1OK = rtn1.Wait(3000);
            if (!wait1OK || !rtn1.Result)
            {
                log.Info("PublicSubscribeTradeAsync fail" + " " + wait1OK);
                isOK = false;
            }


            bool wait2OK = rtn2.Wait(3000);
            if (!wait2OK || !rtn2.Result)
            {
                log.Info("PublicSubscribeBookAsync fail" + " " + wait2OK);
                isOK = false;
            }


            bool wait3OK = rtn3.Wait(3000);
            if (!wait3OK || !rtn3.Result)
            {
                log.Info("PrivateSubscribeUserChanges fail" + " " + wait3OK);
                isOK = false;
            }


            bool wait4OK = rtn4.Wait(3000);
            if (!wait4OK || rtn4.Result.instrument_name != inst.Symbol)
            {
                log.Info("PrivateGetPosition fail" + " " + wait4OK);
                isOK = false;
            }
            else
            {
                EmitData(OQFunc.GetPositionData(rtn4.Result.instrument_name, rtn4.Result.size, framework, this.id));
            }



            if (isOK)
            {
                log.Info("subscribe ok");
            }
            else
            {   
                log.Error("subscribe fail");
            }
        }

        public override void Unsubscribe(Instrument inst)
        {
            if (m_subscribedInst.TryRemove(inst.Symbol, out MarketInfo bi))
            {
                Task<List<string>> rtn = api.UnsubscribePublicAsync(bi.chs);

            }
            else
            {  
                log.Info("unsubscribe fail, no instrument found:" + inst.Symbol);
            }
        }  

    }
}


