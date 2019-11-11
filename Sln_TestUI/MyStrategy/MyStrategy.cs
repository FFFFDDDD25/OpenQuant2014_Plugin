using System;
using System.Drawing;
using System.Threading;
using SmartQuant;
using SmartQuant.Indicators;
using System.Threading.Tasks;
using OpenQuant;
using OpenQunatFunction;
using Microsoft.VisualBasic;

namespace OpenQuant
{
    public class Strategy_TestUI : InstrumentStrategy
    {
        static bool isSubscribed = false;
        static bool isWriteDatabase = false;
        protected override void OnStrategyStart()
        {
            lock (this)
            {
                if (isSubscribed)
                {
                    return;
                }
                else
                {
                    isSubscribed = true;

                    if (this.Mode == StrategyMode.Live)
                    {
                        isWriteDatabase = (Interaction.InputBox("Write Database ?", "", "N") == "N") ? false : true;
                    }
                    _Global.formTrading.isLive = (this.Mode == StrategyMode.Live) ? true : false;
                    _Global.formTrading.OnSendOrder += SendOrder;
                    _Global.formTrading.OnCancelOrder += CancelOrder;
                }
            }
        }

        public Strategy_TestUI(Framework framework, string name)
            : base(framework, name)
        {
        }

        public void CancelOrder(Order order)
        {
            Cancel(order);
        }

        public void SendOrder(Instrument inst, OrderType ot, OrderSide os, int qty, double price)
        {
            Order order = null;


            if (os == OrderSide.Buy)
            {
                if (ot == OrderType.Market)
                    order = BuyOrder(inst, qty, "ui test");
                else if (ot == OrderType.Limit)
                    order = BuyLimit(inst, qty, price, "ui test");
            }
            else if (os == OrderSide.Sell)
            {
                if (ot == OrderType.Market)
                    order = SellOrder(inst, qty, "ui test");
                else if (ot == OrderType.Limit)
                    order = SellLimit(inst, qty, price, "ui test");
            }

            OQFunc.SendOrder(order, this);
            _Global.formTrading.AddOrder(order);


        }

        public void AddDatabase(Instrument instrument, DataObject obj)
        {
            if (isWriteDatabase)
            {
                DataSeries database = DataManager.GetDataSeries(instrument.Symbol);
                if (database == null)
                    database = DataManager.AddDataSeries(instrument.Symbol);

                database.Add(obj);
            }
            else if(this.Mode==StrategyMode.Backtest)
            {
                Thread.Sleep(10);
            }
        }

        protected override void OnBid(Instrument instrument, Bid bid)
        {
            AddDatabase(instrument, bid);
            _Global.formTrading.AddInstrument(instrument);
        }


        protected override void OnBar(Instrument instrument, Bar bar)
        {
            AddDatabase(instrument, bar);
        }

        protected override void OnAsk(Instrument instrument, Ask ask)
        {
            AddDatabase(instrument, ask);
            _Global.formTrading.AddInstrument(instrument);
        }

        protected override void OnTrade(Instrument instrument, Trade trade)
        {
            AddDatabase(instrument, trade);
        }

        protected override void OnLevel2(Instrument instrument, Level2Snapshot snapshot)
        {
            AddDatabase(instrument, snapshot);
            _Global.formTrading.AddLevel2(instrument, snapshot);
        }

        protected override void OnPositionChanged(Position position)
        {
            _Global.formTrading.AddStrategyPosition(position);
        }

        protected override void OnStrategyStop()
        {
            if(this.Mode==StrategyMode.Live)
            { 
                _Global.formTrading.Close();
            }
        }

        protected override void OnAccountData(AccountData accountData)
        {
            string symbol = "";
            int? size = null;

            foreach (AccountDataField field in accountData.Fields)
            {
                if (field.Name == "symbol")
                    symbol = field.Value.ToString();
                else if (field.Name == "size")
                    size = int.Parse(field.Value.ToString());
            }

            if (symbol != "" && size != null)
                _Global.formTrading.AddRealPosition(symbol, int.Parse(size.ToString()));
        }

    }
}
