namespace OpenQuant
{
    partial class FormTrading
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbProduct = new System.Windows.Forms.ComboBox();
            this.tbAskQty3 = new System.Windows.Forms.TextBox();
            this.tbAsk3 = new System.Windows.Forms.TextBox();
            this.labMarkAsk2 = new System.Windows.Forms.Label();
            this.tbAsk2 = new System.Windows.Forms.TextBox();
            this.labMarkAsk1 = new System.Windows.Forms.Label();
            this.tbAskQty2 = new System.Windows.Forms.TextBox();
            this.tbAsk1 = new System.Windows.Forms.TextBox();
            this.labMarkAsk0 = new System.Windows.Forms.Label();
            this.tbAskQty1 = new System.Windows.Forms.TextBox();
            this.tbBidQty3 = new System.Windows.Forms.TextBox();
            this.tbBid3 = new System.Windows.Forms.TextBox();
            this.labMarkBid2 = new System.Windows.Forms.Label();
            this.tbBidQty2 = new System.Windows.Forms.TextBox();
            this.tbBid2 = new System.Windows.Forms.TextBox();
            this.labMarkBid1 = new System.Windows.Forms.Label();
            this.tbBidQty1 = new System.Windows.Forms.TextBox();
            this.tbBid1 = new System.Windows.Forms.TextBox();
            this.labMarkBid0 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLastQty = new System.Windows.Forms.TextBox();
            this.tbLast = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.market_price = new System.Windows.Forms.RadioButton();
            this.limit_price = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.tbSize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lbPosStrategy = new System.Windows.Forms.Label();
            this.tbTest_second = new System.Windows.Forms.Label();
            this.tbTimes = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbPosReal = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbTimeBacktest = new System.Windows.Forms.TextBox();
            this.tbBarType = new System.Windows.Forms.TextBox();
            this.tbBarVol = new System.Windows.Forms.TextBox();
            this.tbBarLow = new System.Windows.Forms.TextBox();
            this.tbBarHigh = new System.Windows.Forms.TextBox();
            this.tbBarClose = new System.Windows.Forms.TextBox();
            this.tbBarOpen = new System.Windows.Forms.TextBox();
            this.tbCloseTime = new System.Windows.Forms.TextBox();
            this.tbOpenTime = new System.Windows.Forms.TextBox();
            this.gbLiveCmd = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tbAsk3.SuspendLayout();
            this.tbAsk2.SuspendLayout();
            this.tbAsk1.SuspendLayout();
            this.tbBid3.SuspendLayout();
            this.tbBid2.SuspendLayout();
            this.tbBid1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTimes)).BeginInit();
            this.gbLiveCmd.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbProduct
            // 
            this.cbProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbProduct.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.cbProduct.FormattingEnabled = true;
            this.cbProduct.Location = new System.Drawing.Point(12, 12);
            this.cbProduct.Name = "cbProduct";
            this.cbProduct.Size = new System.Drawing.Size(137, 23);
            this.cbProduct.TabIndex = 0;
            this.cbProduct.SelectedIndexChanged += new System.EventHandler(this.cbProduct_SelectedIndexChanged);
            // 
            // tbAskQty3
            // 
            this.tbAskQty3.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbAskQty3.Location = new System.Drawing.Point(13, 54);
            this.tbAskQty3.Name = "tbAskQty3";
            this.tbAskQty3.ReadOnly = true;
            this.tbAskQty3.Size = new System.Drawing.Size(54, 23);
            this.tbAskQty3.TabIndex = 1;
            this.tbAskQty3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbAsk3
            // 
            this.tbAsk3.Controls.Add(this.labMarkAsk2);
            this.tbAsk3.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbAsk3.Location = new System.Drawing.Point(67, 54);
            this.tbAsk3.Name = "tbAsk3";
            this.tbAsk3.ReadOnly = true;
            this.tbAsk3.Size = new System.Drawing.Size(65, 23);
            this.tbAsk3.TabIndex = 2;
            this.tbAsk3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbAsk3.Click += new System.EventHandler(this.tbPrice_Click);
            // 
            // labMarkAsk2
            // 
            this.labMarkAsk2.BackColor = System.Drawing.Color.Transparent;
            this.labMarkAsk2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMarkAsk2.Location = new System.Drawing.Point(52, 3);
            this.labMarkAsk2.Name = "labMarkAsk2";
            this.labMarkAsk2.Size = new System.Drawing.Size(8, 15);
            this.labMarkAsk2.TabIndex = 30;
            this.labMarkAsk2.Text = "*";
            this.labMarkAsk2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labMarkAsk2.Visible = false;
            // 
            // tbAsk2
            // 
            this.tbAsk2.Controls.Add(this.labMarkAsk1);
            this.tbAsk2.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbAsk2.Location = new System.Drawing.Point(67, 77);
            this.tbAsk2.Name = "tbAsk2";
            this.tbAsk2.ReadOnly = true;
            this.tbAsk2.Size = new System.Drawing.Size(65, 23);
            this.tbAsk2.TabIndex = 4;
            this.tbAsk2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbAsk2.Click += new System.EventHandler(this.tbPrice_Click);
            // 
            // labMarkAsk1
            // 
            this.labMarkAsk1.BackColor = System.Drawing.Color.Transparent;
            this.labMarkAsk1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMarkAsk1.Location = new System.Drawing.Point(52, 3);
            this.labMarkAsk1.Name = "labMarkAsk1";
            this.labMarkAsk1.Size = new System.Drawing.Size(8, 15);
            this.labMarkAsk1.TabIndex = 31;
            this.labMarkAsk1.Text = "*";
            this.labMarkAsk1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labMarkAsk1.Visible = false;
            // 
            // tbAskQty2
            // 
            this.tbAskQty2.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbAskQty2.Location = new System.Drawing.Point(13, 77);
            this.tbAskQty2.Name = "tbAskQty2";
            this.tbAskQty2.ReadOnly = true;
            this.tbAskQty2.Size = new System.Drawing.Size(54, 23);
            this.tbAskQty2.TabIndex = 3;
            this.tbAskQty2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbAsk1
            // 
            this.tbAsk1.Controls.Add(this.labMarkAsk0);
            this.tbAsk1.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbAsk1.Location = new System.Drawing.Point(67, 100);
            this.tbAsk1.Name = "tbAsk1";
            this.tbAsk1.ReadOnly = true;
            this.tbAsk1.Size = new System.Drawing.Size(65, 23);
            this.tbAsk1.TabIndex = 6;
            this.tbAsk1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbAsk1.Click += new System.EventHandler(this.tbPrice_Click);
            // 
            // labMarkAsk0
            // 
            this.labMarkAsk0.BackColor = System.Drawing.Color.Transparent;
            this.labMarkAsk0.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMarkAsk0.Location = new System.Drawing.Point(52, 3);
            this.labMarkAsk0.Name = "labMarkAsk0";
            this.labMarkAsk0.Size = new System.Drawing.Size(8, 15);
            this.labMarkAsk0.TabIndex = 32;
            this.labMarkAsk0.Text = "*";
            this.labMarkAsk0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labMarkAsk0.Visible = false;
            // 
            // tbAskQty1
            // 
            this.tbAskQty1.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbAskQty1.Location = new System.Drawing.Point(13, 100);
            this.tbAskQty1.Name = "tbAskQty1";
            this.tbAskQty1.ReadOnly = true;
            this.tbAskQty1.Size = new System.Drawing.Size(54, 23);
            this.tbAskQty1.TabIndex = 5;
            this.tbAskQty1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbBidQty3
            // 
            this.tbBidQty3.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBidQty3.Location = new System.Drawing.Point(132, 169);
            this.tbBidQty3.Name = "tbBidQty3";
            this.tbBidQty3.ReadOnly = true;
            this.tbBidQty3.Size = new System.Drawing.Size(54, 23);
            this.tbBidQty3.TabIndex = 12;
            this.tbBidQty3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbBid3
            // 
            this.tbBid3.Controls.Add(this.labMarkBid2);
            this.tbBid3.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBid3.Location = new System.Drawing.Point(67, 169);
            this.tbBid3.Name = "tbBid3";
            this.tbBid3.ReadOnly = true;
            this.tbBid3.Size = new System.Drawing.Size(65, 23);
            this.tbBid3.TabIndex = 11;
            this.tbBid3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbBid3.Click += new System.EventHandler(this.tbPrice_Click);
            // 
            // labMarkBid2
            // 
            this.labMarkBid2.BackColor = System.Drawing.Color.Transparent;
            this.labMarkBid2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMarkBid2.Location = new System.Drawing.Point(52, 3);
            this.labMarkBid2.Name = "labMarkBid2";
            this.labMarkBid2.Size = new System.Drawing.Size(8, 15);
            this.labMarkBid2.TabIndex = 35;
            this.labMarkBid2.Text = "*";
            this.labMarkBid2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labMarkBid2.Visible = false;
            // 
            // tbBidQty2
            // 
            this.tbBidQty2.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBidQty2.Location = new System.Drawing.Point(132, 146);
            this.tbBidQty2.Name = "tbBidQty2";
            this.tbBidQty2.ReadOnly = true;
            this.tbBidQty2.Size = new System.Drawing.Size(54, 23);
            this.tbBidQty2.TabIndex = 10;
            this.tbBidQty2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbBid2
            // 
            this.tbBid2.Controls.Add(this.labMarkBid1);
            this.tbBid2.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBid2.Location = new System.Drawing.Point(67, 146);
            this.tbBid2.Name = "tbBid2";
            this.tbBid2.ReadOnly = true;
            this.tbBid2.Size = new System.Drawing.Size(65, 23);
            this.tbBid2.TabIndex = 9;
            this.tbBid2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbBid2.Click += new System.EventHandler(this.tbPrice_Click);
            // 
            // labMarkBid1
            // 
            this.labMarkBid1.BackColor = System.Drawing.Color.Transparent;
            this.labMarkBid1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMarkBid1.Location = new System.Drawing.Point(52, 3);
            this.labMarkBid1.Name = "labMarkBid1";
            this.labMarkBid1.Size = new System.Drawing.Size(8, 15);
            this.labMarkBid1.TabIndex = 34;
            this.labMarkBid1.Text = "*";
            this.labMarkBid1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labMarkBid1.Visible = false;
            // 
            // tbBidQty1
            // 
            this.tbBidQty1.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBidQty1.Location = new System.Drawing.Point(132, 123);
            this.tbBidQty1.Name = "tbBidQty1";
            this.tbBidQty1.ReadOnly = true;
            this.tbBidQty1.Size = new System.Drawing.Size(54, 23);
            this.tbBidQty1.TabIndex = 8;
            this.tbBidQty1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbBid1
            // 
            this.tbBid1.Controls.Add(this.labMarkBid0);
            this.tbBid1.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBid1.Location = new System.Drawing.Point(67, 123);
            this.tbBid1.Name = "tbBid1";
            this.tbBid1.ReadOnly = true;
            this.tbBid1.Size = new System.Drawing.Size(65, 23);
            this.tbBid1.TabIndex = 7;
            this.tbBid1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbBid1.Click += new System.EventHandler(this.tbPrice_Click);
            // 
            // labMarkBid0
            // 
            this.labMarkBid0.BackColor = System.Drawing.Color.Transparent;
            this.labMarkBid0.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMarkBid0.Location = new System.Drawing.Point(52, 3);
            this.labMarkBid0.Name = "labMarkBid0";
            this.labMarkBid0.Size = new System.Drawing.Size(8, 15);
            this.labMarkBid0.TabIndex = 33;
            this.labMarkBid0.Text = "*";
            this.labMarkBid0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labMarkBid0.Visible = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 1);
            this.label1.TabIndex = 13;
            this.label1.Text = "label1";
            // 
            // tbLastQty
            // 
            this.tbLastQty.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbLastQty.Location = new System.Drawing.Point(132, 195);
            this.tbLastQty.Name = "tbLastQty";
            this.tbLastQty.ReadOnly = true;
            this.tbLastQty.Size = new System.Drawing.Size(54, 23);
            this.tbLastQty.TabIndex = 15;
            this.tbLastQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbLast
            // 
            this.tbLast.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbLast.Location = new System.Drawing.Point(67, 195);
            this.tbLast.Name = "tbLast";
            this.tbLast.ReadOnly = true;
            this.tbLast.Size = new System.Drawing.Size(65, 23);
            this.tbLast.TabIndex = 14;
            this.tbLast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbLast.Click += new System.EventHandler(this.tbPrice_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(13, 219);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 1);
            this.label2.TabIndex = 16;
            this.label2.Text = "label2";
            // 
            // market_price
            // 
            this.market_price.AutoSize = true;
            this.market_price.Checked = true;
            this.market_price.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.market_price.Location = new System.Drawing.Point(90, 17);
            this.market_price.Name = "market_price";
            this.market_price.Size = new System.Drawing.Size(72, 21);
            this.market_price.TabIndex = 22;
            this.market_price.TabStop = true;
            this.market_price.Text = "Market";
            this.market_price.UseVisualStyleBackColor = true;
            // 
            // limit_price
            // 
            this.limit_price.AutoSize = true;
            this.limit_price.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.limit_price.Location = new System.Drawing.Point(15, 17);
            this.limit_price.Name = "limit_price";
            this.limit_price.Size = new System.Drawing.Size(60, 21);
            this.limit_price.TabIndex = 23;
            this.limit_price.TabStop = true;
            this.limit_price.Text = "Limit";
            this.limit_price.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(11, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 15);
            this.label7.TabIndex = 27;
            this.label7.Text = "Size";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(87, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 15);
            this.label8.TabIndex = 28;
            this.label8.Text = "Price";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExecute
            // 
            this.btnExecute.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.btnExecute.Location = new System.Drawing.Point(14, 73);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(146, 34);
            this.btnExecute.TabIndex = 29;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(14, 199);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(49, 15);
            this.label15.TabIndex = 36;
            this.label15.Text = "Last/Vol";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numPrice
            // 
            this.numPrice.DecimalPlaces = 4;
            this.numPrice.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.numPrice.Location = new System.Drawing.Point(121, 44);
            this.numPrice.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numPrice.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(113, 23);
            this.numPrice.TabIndex = 17;
            this.numPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbSize
            // 
            this.tbSize.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbSize.Location = new System.Drawing.Point(41, 44);
            this.tbSize.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.tbSize.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.tbSize.Name = "tbSize";
            this.tbSize.Size = new System.Drawing.Size(40, 23);
            this.tbSize.TabIndex = 42;
            this.tbSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(138, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 15);
            this.label3.TabIndex = 43;
            this.label3.Text = "OQ Pos";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPosStrategy
            // 
            this.lbPosStrategy.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.lbPosStrategy.Location = new System.Drawing.Point(138, 59);
            this.lbPosStrategy.Name = "lbPosStrategy";
            this.lbPosStrategy.Size = new System.Drawing.Size(55, 20);
            this.lbPosStrategy.TabIndex = 44;
            this.lbPosStrategy.Text = "?";
            this.lbPosStrategy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTest_second
            // 
            this.tbTest_second.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbTest_second.Location = new System.Drawing.Point(12, 160);
            this.tbTest_second.Name = "tbTest_second";
            this.tbTest_second.Size = new System.Drawing.Size(55, 20);
            this.tbTest_second.TabIndex = 45;
            this.tbTest_second.Text = "?";
            this.tbTest_second.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTimes
            // 
            this.tbTimes.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbTimes.Location = new System.Drawing.Point(212, 81);
            this.tbTimes.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.tbTimes.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.tbTimes.Name = "tbTimes";
            this.tbTimes.Size = new System.Drawing.Size(40, 23);
            this.tbTimes.TabIndex = 47;
            this.tbTimes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(166, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 15);
            this.label4.TabIndex = 46;
            this.label4.Text = "Times";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(138, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 48;
            this.label5.Text = "Real Pos";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPosReal
            // 
            this.lbPosReal.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.lbPosReal.Location = new System.Drawing.Point(135, 100);
            this.lbPosReal.Name = "lbPosReal";
            this.lbPosReal.Size = new System.Drawing.Size(55, 20);
            this.lbPosReal.TabIndex = 49;
            this.lbPosReal.Text = "?";
            this.lbPosReal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(14, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 15);
            this.label6.TabIndex = 50;
            this.label6.Text = "seconds";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(334, 342);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(111, 21);
            this.btnCancel.TabIndex = 52;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // tbTimeBacktest
            // 
            this.tbTimeBacktest.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbTimeBacktest.Location = new System.Drawing.Point(192, 12);
            this.tbTimeBacktest.Name = "tbTimeBacktest";
            this.tbTimeBacktest.ReadOnly = true;
            this.tbTimeBacktest.Size = new System.Drawing.Size(266, 23);
            this.tbTimeBacktest.TabIndex = 79;
            this.tbTimeBacktest.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbBarType
            // 
            this.tbBarType.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBarType.Location = new System.Drawing.Point(266, 47);
            this.tbBarType.Name = "tbBarType";
            this.tbBarType.ReadOnly = true;
            this.tbBarType.Size = new System.Drawing.Size(80, 23);
            this.tbBarType.TabIndex = 78;
            this.tbBarType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbBarVol
            // 
            this.tbBarVol.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBarVol.Location = new System.Drawing.Point(375, 178);
            this.tbBarVol.Name = "tbBarVol";
            this.tbBarVol.ReadOnly = true;
            this.tbBarVol.Size = new System.Drawing.Size(83, 23);
            this.tbBarVol.TabIndex = 77;
            this.tbBarVol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbBarLow
            // 
            this.tbBarLow.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBarLow.Location = new System.Drawing.Point(364, 207);
            this.tbBarLow.Name = "tbBarLow";
            this.tbBarLow.ReadOnly = true;
            this.tbBarLow.Size = new System.Drawing.Size(117, 23);
            this.tbBarLow.TabIndex = 76;
            this.tbBarLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbBarHigh
            // 
            this.tbBarHigh.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBarHigh.Location = new System.Drawing.Point(364, 145);
            this.tbBarHigh.Name = "tbBarHigh";
            this.tbBarHigh.ReadOnly = true;
            this.tbBarHigh.Size = new System.Drawing.Size(117, 23);
            this.tbBarHigh.TabIndex = 75;
            this.tbBarHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbBarClose
            // 
            this.tbBarClose.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBarClose.Location = new System.Drawing.Point(473, 176);
            this.tbBarClose.Name = "tbBarClose";
            this.tbBarClose.ReadOnly = true;
            this.tbBarClose.Size = new System.Drawing.Size(102, 23);
            this.tbBarClose.TabIndex = 74;
            this.tbBarClose.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbBarOpen
            // 
            this.tbBarOpen.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbBarOpen.Location = new System.Drawing.Point(266, 176);
            this.tbBarOpen.Name = "tbBarOpen";
            this.tbBarOpen.ReadOnly = true;
            this.tbBarOpen.Size = new System.Drawing.Size(95, 23);
            this.tbBarOpen.TabIndex = 73;
            this.tbBarOpen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbCloseTime
            // 
            this.tbCloseTime.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbCloseTime.Location = new System.Drawing.Point(266, 107);
            this.tbCloseTime.Name = "tbCloseTime";
            this.tbCloseTime.ReadOnly = true;
            this.tbCloseTime.Size = new System.Drawing.Size(266, 23);
            this.tbCloseTime.TabIndex = 72;
            this.tbCloseTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbOpenTime
            // 
            this.tbOpenTime.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.tbOpenTime.Location = new System.Drawing.Point(266, 76);
            this.tbOpenTime.Name = "tbOpenTime";
            this.tbOpenTime.ReadOnly = true;
            this.tbOpenTime.Size = new System.Drawing.Size(266, 23);
            this.tbOpenTime.TabIndex = 71;
            this.tbOpenTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gbLiveCmd
            // 
            this.gbLiveCmd.Controls.Add(this.btnExecute);
            this.gbLiveCmd.Controls.Add(this.label7);
            this.gbLiveCmd.Controls.Add(this.label8);
            this.gbLiveCmd.Controls.Add(this.numPrice);
            this.gbLiveCmd.Controls.Add(this.market_price);
            this.gbLiveCmd.Controls.Add(this.limit_price);
            this.gbLiveCmd.Controls.Add(this.tbSize);
            this.gbLiveCmd.Controls.Add(this.label4);
            this.gbLiveCmd.Controls.Add(this.tbTimes);
            this.gbLiveCmd.Location = new System.Drawing.Point(2, 238);
            this.gbLiveCmd.Name = "gbLiveCmd";
            this.gbLiveCmd.Size = new System.Drawing.Size(261, 119);
            this.gbLiveCmd.TabIndex = 80;
            this.gbLiveCmd.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(297, 230);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 15);
            this.label12.TabIndex = 81;
            this.label12.Text = "Orders";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(300, 248);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(169, 88);
            this.listBox1.TabIndex = 51;
            // 
            // FormTrading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 369);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.gbLiveCmd);
            this.Controls.Add(this.tbTimeBacktest);
            this.Controls.Add(this.tbBarType);
            this.Controls.Add(this.tbBarVol);
            this.Controls.Add(this.tbBarLow);
            this.Controls.Add(this.tbBarHigh);
            this.Controls.Add(this.tbBarClose);
            this.Controls.Add(this.tbBarOpen);
            this.Controls.Add(this.tbCloseTime);
            this.Controls.Add(this.tbOpenTime);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbPosReal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbTest_second);
            this.Controls.Add(this.lbPosStrategy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbLastQty);
            this.Controls.Add(this.tbLast);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbBidQty3);
            this.Controls.Add(this.tbBid3);
            this.Controls.Add(this.tbBidQty2);
            this.Controls.Add(this.tbBid2);
            this.Controls.Add(this.tbBidQty1);
            this.Controls.Add(this.tbBid1);
            this.Controls.Add(this.tbAsk1);
            this.Controls.Add(this.tbAskQty1);
            this.Controls.Add(this.tbAsk2);
            this.Controls.Add(this.tbAskQty2);
            this.Controls.Add(this.tbAsk3);
            this.Controls.Add(this.tbAskQty3);
            this.Controls.Add(this.cbProduct);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTrading";
            this.Text = "FormProduct";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormTrading_Load);
            this.tbAsk3.ResumeLayout(false);
            this.tbAsk2.ResumeLayout(false);
            this.tbAsk1.ResumeLayout(false);
            this.tbBid3.ResumeLayout(false);
            this.tbBid2.ResumeLayout(false);
            this.tbBid1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTimes)).EndInit();
            this.gbLiveCmd.ResumeLayout(false);
            this.gbLiveCmd.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox cbProduct;
        public System.Windows.Forms.TextBox tbAskQty3;
        public System.Windows.Forms.TextBox tbAsk3;
        public System.Windows.Forms.TextBox tbAsk2;
        public System.Windows.Forms.TextBox tbAskQty2;
        public System.Windows.Forms.TextBox tbAsk1;
        public System.Windows.Forms.TextBox tbAskQty1;
        public System.Windows.Forms.TextBox tbBidQty3;
        public System.Windows.Forms.TextBox tbBid3;
        public System.Windows.Forms.TextBox tbBidQty2;
        public System.Windows.Forms.TextBox tbBid2;
        public System.Windows.Forms.TextBox tbBidQty1;
        public System.Windows.Forms.TextBox tbBid1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tbLastQty;
        public System.Windows.Forms.TextBox tbLast;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.RadioButton market_price;
        public System.Windows.Forms.RadioButton limit_price;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.Button btnExecute;
        public System.Windows.Forms.Label labMarkAsk2;
        public System.Windows.Forms.Label labMarkAsk1;
        public System.Windows.Forms.Label labMarkAsk0;
        public System.Windows.Forms.Label labMarkBid0;
        public System.Windows.Forms.Label labMarkBid1;
        public System.Windows.Forms.Label labMarkBid2;
        public System.Windows.Forms.Label label15;
        public System.Windows.Forms.NumericUpDown numPrice;
        public System.Windows.Forms.NumericUpDown tbSize;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label lbPosStrategy;
        public System.Windows.Forms.Label tbTest_second;
        public System.Windows.Forms.NumericUpDown tbTimes;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label lbPosReal;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox tbTimeBacktest;
        public System.Windows.Forms.TextBox tbBarType;
        public System.Windows.Forms.TextBox tbBarVol;
        public System.Windows.Forms.TextBox tbBarLow;
        public System.Windows.Forms.TextBox tbBarHigh;
        public System.Windows.Forms.TextBox tbBarClose;
        public System.Windows.Forms.TextBox tbBarOpen;
        public System.Windows.Forms.TextBox tbCloseTime;
        public System.Windows.Forms.TextBox tbOpenTime;
        private System.Windows.Forms.GroupBox gbLiveCmd;
        public System.Windows.Forms.Label label12;
        private System.Windows.Forms.ListBox listBox1;
    }
}