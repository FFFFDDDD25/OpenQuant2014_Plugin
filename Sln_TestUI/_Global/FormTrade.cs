
using System;
using System.Drawing;
using System.Windows.Forms;
using SmartQuant;
using System.Threading;
using System.Collections.Concurrent;

namespace OpenQuant
{
    public partial class FormTrading : Form
    {
        public delegate void EventCancelOrder(Order order);
        public delegate void EventSendOrder(Instrument inst, OrderType ot, OrderSide os, int qty, double price);
        public event EventSendOrder OnSendOrder;
        public event EventCancelOrder OnCancelOrder;

        ConcurrentDictionary<string, Instrument> dic_inst = new ConcurrentDictionary<string, Instrument>();
        ConcurrentDictionary<string, Level2Snapshot> dic_lv2 = new ConcurrentDictionary<string, Level2Snapshot>();
        ConcurrentDictionary<string, Position> dic_pos_strategy = new ConcurrentDictionary<string, Position>();
        ConcurrentDictionary<string, int> dic_pos_real = new ConcurrentDictionary<string, int>();
        public bool isLive;

        public FormTrading()
        {
            InitializeComponent();

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(300);
                    UpdateUI();
                }
            }).Start();
        }


        private void UpdateUI()
        {
            if (isLive )
            {
                this.gbLiveCmd.Enabled = true;
                this.btnCancel.Enabled = true;
                UpdateUI2();
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.gbLiveCmd.Enabled = false;
                    this.btnCancel.Enabled = false;
                    UpdateUI2();
                });
            }
        }



        private void UpdateUI2()
        {
            if (cbProduct.SelectedItem == null)
                return;

            string symbol = cbProduct.SelectedItem.ToString();

            Level2Snapshot lv2 = null;
            bool haveLv2 = dic_lv2.TryGetValue(symbol, out lv2);
            bool haveLv2Bids = (haveLv2 && lv2.Bids != null) ? true : false;
            bool haveLv2Asks = (haveLv2 && lv2.Asks != null) ? true : false;

            Instrument inst = null;
            bool haveInst = dic_inst.TryGetValue(symbol, out inst);
            bool haveInstTrade = (haveInst && inst.Trade != null) ? true : false;
            bool haveInstBar = (haveInst && inst.Bar != null) ? true : false;

            this.tbTimeBacktest.Text = (haveLv2) ? lv2.DateTime.ToString() : "";

            this.tbBid1.Text = (haveLv2Bids && lv2.Bids.Length > 0) ? lv2.Bids[0].Price.ToString() : "";
            this.tbBid2.Text = (haveLv2Bids && lv2.Bids.Length > 1) ? lv2.Bids[1].Price.ToString() : "";
            this.tbBid3.Text = (haveLv2Bids && lv2.Bids.Length > 2) ? lv2.Bids[2].Price.ToString() : "";
            this.tbAsk1.Text = (haveLv2Asks && lv2.Asks.Length > 0) ? lv2.Asks[0].Price.ToString() : "";
            this.tbAsk2.Text = (haveLv2Asks && lv2.Asks.Length > 1) ? lv2.Asks[1].Price.ToString() : "";
            this.tbAsk3.Text = (haveLv2Asks && lv2.Asks.Length > 2) ? lv2.Asks[2].Price.ToString() : "";
            this.tbBidQty1.Text = (haveLv2Bids && lv2.Bids.Length > 0) ? lv2.Bids[0].Size.ToString() : "";
            this.tbBidQty2.Text = (haveLv2Bids && lv2.Bids.Length > 1) ? lv2.Bids[1].Size.ToString() : "";
            this.tbBidQty3.Text = (haveLv2Bids && lv2.Bids.Length > 2) ? lv2.Bids[2].Size.ToString() : "";
            this.tbAskQty1.Text = (haveLv2Asks && lv2.Asks.Length > 0) ? lv2.Asks[0].Size.ToString() : "";
            this.tbAskQty2.Text = (haveLv2Asks && lv2.Asks.Length > 1) ? lv2.Asks[1].Size.ToString() : "";
            this.tbAskQty3.Text = (haveLv2Asks && lv2.Asks.Length > 2) ? lv2.Asks[2].Size.ToString() : "";

            this.tbOpenTime.Text = haveInstBar ? "open:" + inst.Bar.OpenDateTime.ToString() : "";
            this.tbCloseTime.Text = haveInstBar ? "close:" + inst.Bar.CloseDateTime.ToString() : "";
            this.tbBarType.Text = haveInstBar ? "type:" + inst.Bar.Type.ToString() : "";
            this.tbBarOpen.Text = haveInstBar ? "open:" + inst.Bar.Open.ToString() : "";
            this.tbBarClose.Text = haveInstBar ? "close:" + inst.Bar.Close.ToString() : "";
            this.tbBarHigh.Text = haveInstBar ? "high:" + inst.Bar.High.ToString() : "";
            this.tbBarLow.Text = haveInstBar ? "low:" + inst.Bar.Low.ToString() : "";
            this.tbBarVol.Text = haveInstBar ? "vol:" + inst.Bar.Volume.ToString() : "";

            this.tbLastQty.Text = haveInstTrade ? inst.Trade.Size.ToString() : "";
            this.tbLast.Text = haveInstTrade ? inst.Trade.Price.ToString() : "";

            this.lbPosStrategy.Text = (dic_pos_strategy.ContainsKey(symbol)) ? dic_pos_strategy[symbol].Amount.ToString() : "";
            this.lbPosReal.Text = (dic_pos_real.ContainsKey(symbol)) ? dic_pos_real[symbol].ToString() : "";

            this.tbBid1.BackColor = (this.tbBid1.Text == this.tbLast.Text) ? Color.White : Color.LightGray;
            this.tbBid2.BackColor = (this.tbBid2.Text == this.tbLast.Text) ? Color.White : Color.LightGray;
            this.tbBid3.BackColor = (this.tbBid3.Text == this.tbLast.Text) ? Color.White : Color.LightGray;
            this.tbAsk1.BackColor = (this.tbAsk1.Text == this.tbLast.Text) ? Color.White : Color.LightGray;
            this.tbAsk2.BackColor = (this.tbAsk2.Text == this.tbLast.Text) ? Color.White : Color.LightGray;
            this.tbAsk3.BackColor = (this.tbAsk3.Text == this.tbLast.Text) ? Color.White : Color.LightGray;

            this.tbTest_second.Text = DateTime.Now.Second.ToString();
        }


        public void SetTextValue(Control c, double d)
        {
            if (c.InvokeRequired)
            {
                Invoke(new Action<Control, double>(SetTextValue), c, d);
            }
            else
            {
                string s = c.Text;
                c.Text = d.ToString();
            }
        }

        public void SetLabelValue(Control c, string s, Color color)
        {
            if (c.InvokeRequired)
            {
                Invoke(new Action<Control, string, Color>(SetLabelValue), c, s, color);
            }
            else
            {
                c.Text = s;
                c.ForeColor = color;
            }
        }

        public void SetLabelValue(Control c, string s)
        {
            if (c.InvokeRequired)
            {
                Invoke(new Action<Control, string>(SetLabelValue), c, s);
            }
            else
            {
                c.Text = s;
            }
        }

        private void cbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void tbPrice_Click(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            decimal d;
            if (Decimal.TryParse(tb.Text, out d))
            {
                numPrice.Text = tb.Text;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            string symbol = cbProduct.SelectedItem.ToString();

            Instrument ins = null;
            if (dic_inst.TryGetValue(symbol, out ins))
            {
                int size = int.Parse(this.tbSize.Text);
                double price = double.Parse(this.numPrice.Text);
                OrderSide side = size > 0 ? OrderSide.Buy : OrderSide.Sell;
                OrderType type = this.market_price.Checked ? OrderType.Market : OrderType.Limit;

                for (int i = 0; i < int.Parse(this.tbTimes.Text); i++)
                {
                    this.OnSendOrder(ins, type, side, Math.Abs(size), price);
                }
            }
        }


        public void AddStrategyPosition(Position pos)
        {
            dic_pos_strategy[pos.Instrument.Symbol] = pos;
        }
        public void AddRealPosition(string symbol, int size)
        {
            dic_pos_real[symbol] = size;
        }

        public void AddLevel2(Instrument inst, Level2Snapshot snap)
        {
            dic_lv2[inst.Symbol] = snap;
        }

        public void AddOrder(Order order)
        {
            this.listBox1.Items.Add(order);
        }

        public void AddInstrument(Instrument inst)
        {

            if (!dic_inst.ContainsKey(inst.Symbol))
            {

                if (isLive)
                {
                    this.cbProduct.Items.Add(inst.Symbol);
                }
                else
                {
                    this.Invoke((MethodInvoker)delegate
                    {


                        this.cbProduct.Items.Add(inst.Symbol);
                    });
                }


            }
            dic_inst[inst.Symbol] = inst;
        }



        private void FormTrading_Load(object sender, EventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Order order = (Order)(this.listBox1.SelectedItem);

            this.OnCancelOrder(order);
        }

    }
}
