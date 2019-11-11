using System;
using System.Drawing;
using SmartQuant;
using SmartQuant.Indicators;

namespace OpenQuant
{
    public class Strategy_BB : InstrumentStrategy
    {
        private BBU bbu;
        private BBL bbl;
        private SMA sma;
        private Order exitOrder;
        private Group barsGroup;
        private Group fillGroup;
        private Group equityGroup;
        private Group bbuGroup;
        private Group bblGroup;
        private Group smaGroup;


        static IF.IniFile ini = new IF.IniFile("Strategy_BB");

        [Parameter]
        public double AllocationPerInstrument = 100000;

        //[Parameter]
        public double Qty;//= 10;

        [Parameter]
        public int Length = 10;

        [Parameter]
        public double K = 2;

        public Strategy_BB(Framework framework, string name)
            : base(framework, name)
        {
            Qty = double.Parse(ini.ReadOrWrite("Order Qty", "10"));
        }

        protected override void OnStrategyStart()
        {
            Portfolio.Account.Deposit(AllocationPerInstrument, CurrencyId.USD, "Initial allocation");

            bbu = new BBU(Bars, Length, K);
            bbl = new BBL(Bars, Length, K);
            sma = new SMA(Bars, Length);

            AddGroups();
        }

        protected override void OnBar(Instrument instrument, Bar bar)
        {
            Console.WriteLine("OnBar Strategy_BB");


            // Add bar to bar series.
            Bars.Add(bar);

            // Add bar to group.
            Log(bar, barsGroup);

            // Add upper bollinger band value to group.
            if (bbu.Count > 0)
                Log(bbu.Last, bbuGroup);

            // Add lower bollinger band value to group.
            if (bbl.Count > 0)
                Log(bbl.Last, bblGroup);

            // Add simple moving average value bands to group.
            if (sma.Count > 0)
                Log(sma.Last, smaGroup);

            // Calculate performance.
            Portfolio.Performance.Update();

            // Add equity to group.
            Log(Portfolio.Value, equityGroup);

            // Check strategy logic.
            if (!HasPosition(instrument))
            {
                if (bbu.Count > 0 && bar.Close >= bbu.Last)
                {
                    Order enterOrder = SellOrder(Instrument, Qty, "Enter");
                    Send(enterOrder);
                }
                else if (bbl.Count > 0 && bar.Close <= bbl.Last)
                {
                    Order enterOrder = BuyOrder(Instrument, Qty, "Enter");
                    Send(enterOrder);
                }
            }
            else
                UpdateExitLimit();
        }


        protected override void OnFill(Fill fill)
        {
            Console.WriteLine("Fill Side " + fill.Side);
            Console.WriteLine("Fill Price "+fill.Price);
            Console.WriteLine("Fill Qty " + fill.Qty);
            Console.WriteLine("Fill Status " + fill.Order.Status);


            Log(fill, fillGroup);
        }

        protected override void OnPositionOpened(Position position)
        {
            UpdateExitLimit();
        }

        private void AddGroups()
        {
            // Create bars group.
            barsGroup = new Group("Bars");
            barsGroup.Add("Pad", DataObjectType.String, 0);
            barsGroup.Add("SelectorKey", Instrument.Symbol);

            // Create fills group.
            fillGroup = new Group("Fills");
            fillGroup.Add("Pad", 0);
            fillGroup.Add("SelectorKey", Instrument.Symbol);

            // Create equity group.
            equityGroup = new Group("Equity");
            equityGroup.Add("Pad", 1);
            equityGroup.Add("SelectorKey", Instrument.Symbol);

            // Create BBU group.
            bbuGroup = new Group("BBU");
            bbuGroup.Add("Pad", 0);
            bbuGroup.Add("SelectorKey", Instrument.Symbol);
            bbuGroup.Add("Color", Color.Blue);

            // Create BBL group.
            bblGroup = new Group("BBL");
            bblGroup.Add("Pad", 0);
            bblGroup.Add("SelectorKey", Instrument.Symbol);
            bblGroup.Add("Color", Color.Blue);

            // Create SMA group.
            smaGroup = new Group("SMA");
            smaGroup.Add("Pad", 0);
            smaGroup.Add("SelectorKey", Instrument.Symbol);
            smaGroup.Add("Color", Color.Yellow);

            // Add groups to manager.
            GroupManager.Add(barsGroup);
            GroupManager.Add(fillGroup);
            GroupManager.Add(equityGroup);
            GroupManager.Add(bbuGroup);
            GroupManager.Add(bblGroup);
            GroupManager.Add(smaGroup);
        }

        private void UpdateExitLimit()
        {
            if (exitOrder != null && !exitOrder.IsDone)
            {
                Cancel(exitOrder);
            }

            if (HasPosition(Instrument))
            {
                if (Position.Side == PositionSide.Long)
                    exitOrder = SellLimitOrder(Instrument, Qty, sma.Last, "Exit");
                else
                    exitOrder = BuyLimitOrder(Instrument, Qty, sma.Last, "Exit");

                Send(exitOrder);
            }
        }

    }
}
