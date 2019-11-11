
using SmartQuant;
using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace OpenQunatFunction
{
    static public class OQFunc
    {
        static public AccountData GetPositionData(string symbol, int size, Framework framework, byte providerID)
        {
            AccountData data = new AccountData(framework.Clock.DateTime, AccountDataType.AccountValue, "accountName", providerID, providerID); // or AccountDataType.AccountValue
            data.Fields.Add("symbol", symbol);
            data.Fields.Add("size", size);
            return data;
        }

        static public Instrument CreateInstrument(string symbol, InstrumentManager im)
        {
            Instrument instru;

            instru = new Instrument(InstrumentType.Stock, symbol);

            if (!im.Contains(symbol))
                im.Add(instru, false);

            instru = im.Instruments[symbol];

            return instru;
        }


        static public void UserInputProviderId(out IDataProvider dataProvider, out IExecutionProvider executionProvider, ProviderManager pm)
        {
            string providerId = Interaction.InputBox("輸入交易所代號", "", "89");//89=deribit by user define
            dataProvider = pm.GetDataProvider(int.Parse(providerId));
            executionProvider = pm.GetExecutionProvider(int.Parse(providerId));
            if (dataProvider == null || executionProvider == null)
            {
                throw new Exception("provider is null");
            }
        }


        static public string UserInputString(string question, string default_ans)
        {
            return Interaction.InputBox(question, "", default_ans);//89=deribit by user define
        }


        static public Instrument UserInputInstrument(InstrumentManager im)
        {
            string symbol = Interaction.InputBox("輸入商品代號", "", "BTC-PERPETUAL");
            return CreateInstrument(symbol,im);
        }

        static public Order SendOrder(Order order,Strategy strategy)
        {
            if (order.IsNotSent)
                strategy.Send(order);
            return order;
        }

        static public Instrument[] UserInputInstruments(InstrumentManager im)
        {
            List<Instrument> rtn = new List<Instrument>();

            string[] symbols = Interaction.InputBox("輸入商品代號", "", "BTC-PERPETUAL,ETH-PERPETUAL,BTC-25SEP20-8000-C,BTC-25SEP20-8000-P").Split(',');//

            foreach (string s in symbols)
            {
                rtn.Add(CreateInstrument(s, im));
            }

            return rtn.ToArray();
        }
    }
}

