namespace ASCOM.iOptronZEQ25
{
    partial class Form1
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
            this.buttonChoose = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelDriverId = new System.Windows.Forms.Label();
            this.comboBoxSpeed = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.labelSlew = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblHOME = new System.Windows.Forms.Label();
            this.lblPARK = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTraffic = new System.Windows.Forms.Button();
            this.buttonHome = new System.Windows.Forms.Button();
            this.checkBoxTrack = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelAlt = new System.Windows.Forms.Label();
            this.labelAz = new System.Windows.Forms.Label();
            this.labelDec = new System.Windows.Forms.Label();
            this.labelRa = new System.Windows.Forms.Label();
            this.labelLst = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxPierSideWest = new System.Windows.Forms.PictureBox();
            this.buttonSlewUp = new System.Windows.Forms.Button();
            this.buttonSlewRight = new System.Windows.Forms.Button();
            this.buttonSlewLeft = new System.Windows.Forms.Button();
            this.buttonSlewDown = new System.Windows.Forms.Button();
            this.buttonSlewStop = new System.Windows.Forms.Button();
            this.pictureBoxPierSideEast = new System.Windows.Forms.PictureBox();
            this.ledPierEast = new ASCOM.Controls.LedIndicator();
            this.ledPierWest = new ASCOM.Controls.LedIndicator();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideWest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideEast)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonChoose
            // 
            this.buttonChoose.Location = new System.Drawing.Point(71, 559);
            this.buttonChoose.Margin = new System.Windows.Forms.Padding(4);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(96, 28);
            this.buttonChoose.TabIndex = 0;
            this.buttonChoose.Text = "Choose";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(71, 595);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(4);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(96, 28);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelDriverId
            // 
            this.labelDriverId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDriverId.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.iOptronZEQ25.Properties.Settings.Default, "DriverId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.labelDriverId.ForeColor = System.Drawing.Color.Red;
            this.labelDriverId.Location = new System.Drawing.Point(13, 627);
            this.labelDriverId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDriverId.Name = "labelDriverId";
            this.labelDriverId.Size = new System.Drawing.Size(159, 25);
            this.labelDriverId.TabIndex = 2;
            //this.labelDriverId.Text = global::ASCOM.iOptronZEQ25.Properties.Settings.Default.DriverId;
            this.labelDriverId.Text = "DriverId";
            this.labelDriverId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxSpeed
            // 
            this.comboBoxSpeed.BackColor = System.Drawing.Color.Black;
            this.comboBoxSpeed.ForeColor = System.Drawing.Color.Red;
            this.comboBoxSpeed.FormattingEnabled = true;
            this.comboBoxSpeed.Items.AddRange(new object[] {
            "   1x",
            "   2x",
            "   8x",
            "  16x",
            "  64x",
            " 128x",
            " 256x",
            " 512x",
            "1024x"});
            this.comboBoxSpeed.Location = new System.Drawing.Point(36, 389);
            this.comboBoxSpeed.Name = "comboBoxSpeed";
            this.comboBoxSpeed.Size = new System.Drawing.Size(80, 24);
            this.comboBoxSpeed.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(13, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 25);
            this.label6.TabIndex = 16;
            this.label6.Text = "V2";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Controls.Add(this.labelSlew, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(106, 439);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(65, 25);
            this.tableLayoutPanel5.TabIndex = 14;
            // 
            // labelSlew
            // 
            this.labelSlew.AutoSize = true;
            this.labelSlew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelSlew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSlew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelSlew.Location = new System.Drawing.Point(4, 0);
            this.labelSlew.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSlew.Name = "labelSlew";
            this.labelSlew.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.labelSlew.Size = new System.Drawing.Size(57, 25);
            this.labelSlew.TabIndex = 0;
            this.labelSlew.Text = "SLEW";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.lblHOME, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblPARK, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(18, 82);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(153, 25);
            this.tableLayoutPanel4.TabIndex = 9;
            // 
            // lblHOME
            // 
            this.lblHOME.AutoSize = true;
            this.lblHOME.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblHOME.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblHOME.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHOME.Location = new System.Drawing.Point(4, 0);
            this.lblHOME.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHOME.Name = "lblHOME";
            this.lblHOME.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.lblHOME.Size = new System.Drawing.Size(51, 25);
            this.lblHOME.TabIndex = 0;
            this.lblHOME.Text = "HOME";
            // 
            // lblPARK
            // 
            this.lblPARK.AutoSize = true;
            this.lblPARK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblPARK.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPARK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPARK.Location = new System.Drawing.Point(102, 0);
            this.lblPARK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPARK.Name = "lblPARK";
            this.lblPARK.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.lblPARK.Size = new System.Drawing.Size(47, 25);
            this.lblPARK.TabIndex = 1;
            this.lblPARK.Text = "PARK";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.buttonTraffic, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonHome, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(18, 471);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(153, 80);
            this.tableLayoutPanel3.TabIndex = 15;
            // 
            // buttonTraffic
            // 
            this.buttonTraffic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTraffic.Location = new System.Drawing.Point(80, 44);
            this.buttonTraffic.Margin = new System.Windows.Forms.Padding(4);
            this.buttonTraffic.Name = "buttonTraffic";
            this.buttonTraffic.Size = new System.Drawing.Size(69, 32);
            this.buttonTraffic.TabIndex = 3;
            this.buttonTraffic.Text = "Traffic";
            this.buttonTraffic.UseVisualStyleBackColor = true;
            // 
            // buttonHome
            // 
            this.buttonHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonHome.Location = new System.Drawing.Point(80, 4);
            this.buttonHome.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(69, 32);
            this.buttonHome.TabIndex = 1;
            this.buttonHome.Text = "Home";
            this.buttonHome.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrack
            // 
            this.checkBoxTrack.AutoSize = true;
            this.checkBoxTrack.ForeColor = System.Drawing.Color.White;
            this.checkBoxTrack.Location = new System.Drawing.Point(25, 443);
            this.checkBoxTrack.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxTrack.Name = "checkBoxTrack";
            this.checkBoxTrack.Size = new System.Drawing.Size(66, 21);
            this.checkBoxTrack.TabIndex = 12;
            this.checkBoxTrack.Text = "Track";
            this.checkBoxTrack.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.8125F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.1875F));
            this.tableLayoutPanel1.Controls.Add(this.labelAlt, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelAz, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelDec, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelRa, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLst, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(17, 114);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(155, 123);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // labelAlt
            // 
            this.labelAlt.AutoSize = true;
            this.labelAlt.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelAlt.ForeColor = System.Drawing.Color.Red;
            this.labelAlt.Location = new System.Drawing.Point(67, 98);
            this.labelAlt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAlt.Name = "labelAlt";
            this.labelAlt.Size = new System.Drawing.Size(84, 25);
            this.labelAlt.TabIndex = 9;
            this.labelAlt.Text = "00:00:00:00";
            // 
            // labelAz
            // 
            this.labelAz.AutoSize = true;
            this.labelAz.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelAz.ForeColor = System.Drawing.Color.Red;
            this.labelAz.Location = new System.Drawing.Point(59, 73);
            this.labelAz.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAz.Name = "labelAz";
            this.labelAz.Size = new System.Drawing.Size(92, 25);
            this.labelAz.TabIndex = 7;
            this.labelAz.Text = "000:00:00:00";
            // 
            // labelDec
            // 
            this.labelDec.AutoSize = true;
            this.labelDec.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDec.ForeColor = System.Drawing.Color.Red;
            this.labelDec.Location = new System.Drawing.Point(59, 48);
            this.labelDec.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDec.Name = "labelDec";
            this.labelDec.Size = new System.Drawing.Size(92, 25);
            this.labelDec.TabIndex = 5;
            this.labelDec.Text = "+00:00:00:00";
            // 
            // labelRa
            // 
            this.labelRa.AutoSize = true;
            this.labelRa.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelRa.ForeColor = System.Drawing.Color.Red;
            this.labelRa.Location = new System.Drawing.Point(67, 24);
            this.labelRa.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRa.Name = "labelRa";
            this.labelRa.Size = new System.Drawing.Size(84, 24);
            this.labelRa.TabIndex = 3;
            this.labelRa.Text = "00:00:00:00";
            // 
            // labelLst
            // 
            this.labelLst.AutoSize = true;
            this.labelLst.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelLst.ForeColor = System.Drawing.Color.Red;
            this.labelLst.Location = new System.Drawing.Point(67, 0);
            this.labelLst.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLst.Name = "labelLst";
            this.labelLst.Size = new System.Drawing.Size(84, 24);
            this.labelLst.TabIndex = 1;
            this.labelLst.Text = "00:00:00:00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "LST";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(4, 73);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Az";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "RA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(4, 48);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Dec";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(4, 98);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Alt";
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Location = new System.Drawing.Point(107, 3);
            this.picASCOM.Margin = new System.Windows.Forms.Padding(4);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 13;
            this.picASCOM.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBoxPierSideWest, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonSlewUp, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonSlewRight, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonSlewLeft, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonSlewDown, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.buttonSlewStop, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.pictureBoxPierSideEast, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.ledPierEast, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.ledPierWest, 0, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(15, 245);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(153, 137);
            this.tableLayoutPanel2.TabIndex = 18;
            // 
            // pictureBoxPierSideWest
            // 
            this.pictureBoxPierSideWest.Location = new System.Drawing.Point(4, 4);
            this.pictureBoxPierSideWest.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxPierSideWest.Name = "pictureBoxPierSideWest";
            this.pictureBoxPierSideWest.Size = new System.Drawing.Size(43, 37);
            this.pictureBoxPierSideWest.TabIndex = 6;
            this.pictureBoxPierSideWest.TabStop = false;
            // 
            // buttonSlewUp
            // 
            this.buttonSlewUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlewUp.Location = new System.Drawing.Point(55, 4);
            this.buttonSlewUp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSlewUp.Name = "buttonSlewUp";
            this.buttonSlewUp.Size = new System.Drawing.Size(43, 37);
            this.buttonSlewUp.TabIndex = 0;
            this.buttonSlewUp.Text = "N";
            this.buttonSlewUp.UseVisualStyleBackColor = true;
            // 
            // buttonSlewRight
            // 
            this.buttonSlewRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlewRight.Location = new System.Drawing.Point(106, 49);
            this.buttonSlewRight.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSlewRight.Name = "buttonSlewRight";
            this.buttonSlewRight.Size = new System.Drawing.Size(43, 37);
            this.buttonSlewRight.TabIndex = 3;
            this.buttonSlewRight.Text = "E";
            this.buttonSlewRight.UseVisualStyleBackColor = true;
            // 
            // buttonSlewLeft
            // 
            this.buttonSlewLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlewLeft.Location = new System.Drawing.Point(4, 49);
            this.buttonSlewLeft.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSlewLeft.Name = "buttonSlewLeft";
            this.buttonSlewLeft.Size = new System.Drawing.Size(43, 37);
            this.buttonSlewLeft.TabIndex = 1;
            this.buttonSlewLeft.Text = "W";
            this.buttonSlewLeft.UseVisualStyleBackColor = true;
            // 
            // buttonSlewDown
            // 
            this.buttonSlewDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlewDown.Location = new System.Drawing.Point(55, 94);
            this.buttonSlewDown.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSlewDown.Name = "buttonSlewDown";
            this.buttonSlewDown.Size = new System.Drawing.Size(43, 39);
            this.buttonSlewDown.TabIndex = 4;
            this.buttonSlewDown.Text = "S";
            this.buttonSlewDown.UseVisualStyleBackColor = true;
            // 
            // buttonSlewStop
            // 
            this.buttonSlewStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlewStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonSlewStop.Location = new System.Drawing.Point(55, 49);
            this.buttonSlewStop.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSlewStop.Name = "buttonSlewStop";
            this.buttonSlewStop.Size = new System.Drawing.Size(43, 37);
            this.buttonSlewStop.TabIndex = 2;
            this.buttonSlewStop.Text = "Ä";
            this.buttonSlewStop.UseVisualStyleBackColor = true;
            // 
            // pictureBoxPierSideEast
            // 
            this.pictureBoxPierSideEast.Location = new System.Drawing.Point(106, 4);
            this.pictureBoxPierSideEast.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxPierSideEast.Name = "pictureBoxPierSideEast";
            this.pictureBoxPierSideEast.Size = new System.Drawing.Size(43, 37);
            this.pictureBoxPierSideEast.TabIndex = 5;
            this.pictureBoxPierSideEast.TabStop = false;
            // 
            // ledPierEast
            // 
            this.ledPierEast.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ledPierEast.LabelText = "";
            this.ledPierEast.Location = new System.Drawing.Point(116, 103);
            this.ledPierEast.Margin = new System.Windows.Forms.Padding(4);
            this.ledPierEast.Name = "ledPierEast";
            this.ledPierEast.Size = new System.Drawing.Size(23, 20);
            this.ledPierEast.TabIndex = 6;
            this.ledPierEast.TabStop = false;
            // 
            // ledPierWest
            // 
            this.ledPierWest.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ledPierWest.LabelText = "";
            this.ledPierWest.Location = new System.Drawing.Point(15, 103);
            this.ledPierWest.Margin = new System.Windows.Forms.Padding(4);
            this.ledPierWest.Name = "ledPierWest";
            this.ledPierWest.Size = new System.Drawing.Size(21, 20);
            this.ledPierWest.TabIndex = 5;
            this.ledPierWest.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(182, 753);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.comboBoxSpeed);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.checkBoxTrack);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.labelDriverId);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonChoose);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "TEMPLATEDEVICETYPE Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideWest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideEast)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelDriverId;
        private System.Windows.Forms.ComboBox comboBoxSpeed;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        internal System.Windows.Forms.Label labelSlew;
        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        internal System.Windows.Forms.Label lblHOME;
        internal System.Windows.Forms.Label lblPARK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonTraffic;
        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.CheckBox checkBoxTrack;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelAlt;
        private System.Windows.Forms.Label labelAz;
        private System.Windows.Forms.Label labelDec;
        private System.Windows.Forms.Label labelRa;
        private System.Windows.Forms.Label labelLst;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBoxPierSideWest;
        private System.Windows.Forms.Button buttonSlewUp;
        private System.Windows.Forms.Button buttonSlewRight;
        private System.Windows.Forms.Button buttonSlewLeft;
        private System.Windows.Forms.Button buttonSlewDown;
        private System.Windows.Forms.Button buttonSlewStop;
        private System.Windows.Forms.PictureBox pictureBoxPierSideEast;
        private Controls.LedIndicator ledPierEast;
        private Controls.LedIndicator ledPierWest;
    }
}

