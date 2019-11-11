using System;
using SmartQuant;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using OpenQunatFunction;

namespace OpenQuant
{
    public class Backtest : Scenario
    {
        public Backtest(Framework framework)
            : base(framework)
        {
        }

        public override void Run()
        {
            bool isLive = (Interaction.InputBox("Live Mode ?", "", "Y")=="Y")?true:false;


            strategy = new Strategy_TestUI(framework, "test ui");

            if (isLive)
            {
                IDataProvider dataProvider = null;
                IExecutionProvider executionProvider = null;
                OQFunc.UserInputProviderId(out dataProvider, out executionProvider, this.ProviderManager);
                strategy.DataProvider = dataProvider;
                strategy.ExecutionProvider = executionProvider;
            }

            Instrument[] insts = OQFunc.UserInputInstruments(this.InstrumentManager);
            foreach (Instrument ins in insts){strategy.AddInstrument(ins);}


            Task.Run(() =>
            {
                _Global.formTrading.ShowDialog();
            });

            System.Threading.Thread.Sleep(1000);

            if (isLive)
            { 
                StartLive();
            }
            else
            {
                foreach (Instrument ins in insts)
                {
                    DataSeries database = DataManager.GetDataSeries(ins.Symbol);
                    if (database == null)
                        throw new Exception("no database");
                    DataSimulator.Series.Add(database);
                }

                StartBacktest();
            }
        }
    }
}