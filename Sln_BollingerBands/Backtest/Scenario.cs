using System;
using SmartQuant;
using Microsoft.VisualBasic;
using OpenQunatFunction;

namespace OpenQuant
{
    public class Backtest : Scenario
    {
        private long barSize = 300;

        public Backtest(Framework framework)
            : base(framework)
        {
        }

        public override void Run()
        {


            StrategyMode sm = StrategyMode.Backtest;


            bool isTest = false;
            if (isTest)
            {
                Instrument instrument1 = InstrumentManager.Instruments["AAPL"];
                Instrument instrument2 = InstrumentManager.Instruments["MSFT"];
                strategy = new Strategy_BB(framework, "BollingerBands");
                sm = StrategyMode.Backtest;
                strategy.AddInstrument(instrument1);
                strategy.AddInstrument(instrument2);
                DataSimulator.DateTime1 = new DateTime(2013, 01, 01);
                DataSimulator.DateTime2 = new DateTime(2013, 12, 31);
                BarFactory.Add(instrument1, BarType.Time, barSize);
                BarFactory.Add(instrument2, BarType.Time, barSize);
            }
            else
            {
                IDataProvider dataProvider = null;
                IExecutionProvider executionProvider=null;
                OQFunc.UserInputProviderId(out dataProvider,out executionProvider,this.ProviderManager) ;

                Instrument instrument3 = OQFunc.UserInputInstrument(this.InstrumentManager);

                strategy = new Strategy_BB(framework, "BollingerBands");
                sm = StrategyMode.Live;
                strategy.DataProvider = dataProvider;
                strategy.ExecutionProvider = executionProvider;
                strategy.AddInstrument(instrument3);
            }


            StartStrategy(sm);
        }
    }
}