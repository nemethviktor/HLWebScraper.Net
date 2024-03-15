using HLWebScraper.Net.Helpers;

namespace HLWebScraper.Net
{
    partial class FrmMainApp
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMainApp));
            mns_Main = new MenuStrip();
            tsm_Settings = new ToolStripMenuItem();
            tsmi_EditCategorisationCSV = new ToolStripMenuItem();
            tsmi_DarkishMode = new ToolStripMenuItem();
            tsm_About = new ToolStripMenuItem();
            tbl_Main = new TableLayoutPanel();
            tcr_Main = new TabControl();
            tpg_Scrape = new TabPage();
            tbl_ScrapeMainGrid = new TableLayoutPanel();
            lbx_Alphabet = new ListBox();
            btn_StartScrape = new Button();
            btn_Stop = new Button();
            gbx_Log = new GroupBox();
            tbx_Log = new TextBox();
            tpg_Overview = new TabPage();
            gbx_Indicators = new GroupBox();
            tbx_Volume = new TextBox();
            lbl_Volume = new Label();
            tbx_PE = new TextBox();
            lbl_PE = new Label();
            tbx_DivYield = new TextBox();
            lbl_DivYield = new Label();
            lbl_GBP_Curr = new Label();
            lbl_OC = new Label();
            tbx_YearHigh_GBP = new TextBox();
            tbx_YearHigh_OC = new TextBox();
            lbl_YearHigh = new Label();
            tbx_YearLow_GBP = new TextBox();
            tbx_YearLow_OC = new TextBox();
            lbl_YearLow = new Label();
            tbx_MarketCap_GBP = new TextBox();
            tbx_MarketCap_OC = new TextBox();
            lbl_MarketCap = new Label();
            tbx_OpenPrice_GBP = new TextBox();
            tbx_OpenPrice_OC = new TextBox();
            lbl_OpenPrice = new Label();
            gbx_PickSearch = new GroupBox();
            ckb_ISAOnlySearch = new CheckBox();
            lbl_Filter = new Label();
            tbx_Search = new TextBox();
            cbx_Securities = new ComboBox();
            gbx_Overview = new GroupBox();
            tbx_Country = new TextBox();
            tbx_Indicies = new TextBox();
            lbl_Indicies = new Label();
            textBox1 = new TextBox();
            lbl_Country = new Label();
            tbx_Currency = new TextBox();
            lbl_Currency = new Label();
            label1 = new Label();
            pbx_ETFType = new PictureBox();
            tbx_ETFType = new TextBox();
            lbl_ETFType = new Label();
            pbx_Sector = new PictureBox();
            tbx_Sector = new TextBox();
            lbl_Sector = new Label();
            tbx_SEDOL = new TextBox();
            tbx_Ticker = new TextBox();
            tbx_Name = new TextBox();
            lbl_SEDOL = new Label();
            lbl_Ticker = new Label();
            ckb_ISA = new CheckBox();
            llb_URL = new LinkLabel();
            lbl_Name = new Label();
            btn_SaveToCSV = new Button();
            btn_ReloadCategories = new Button();
            ttp_Sector = new ToolTip(components);
            ttp_ETFType = new ToolTip(components);
            lbl_SelectionPicker = new Label();
            mns_Main.SuspendLayout();
            tbl_Main.SuspendLayout();
            tcr_Main.SuspendLayout();
            tpg_Scrape.SuspendLayout();
            tbl_ScrapeMainGrid.SuspendLayout();
            gbx_Log.SuspendLayout();
            tpg_Overview.SuspendLayout();
            gbx_Indicators.SuspendLayout();
            gbx_PickSearch.SuspendLayout();
            gbx_Overview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbx_ETFType).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbx_Sector).BeginInit();
            SuspendLayout();
            // 
            // mns_Main
            // 
            mns_Main.Items.AddRange(new ToolStripItem[] { tsm_Settings, tsm_About });
            mns_Main.Location = new Point(0, 0);
            mns_Main.Name = "mns_Main";
            mns_Main.Size = new Size(796, 24);
            mns_Main.TabIndex = 6;
            mns_Main.Text = "mns_Main";
            // 
            // tsm_Settings
            // 
            tsm_Settings.DropDownItems.AddRange(new ToolStripItem[] { tsmi_EditCategorisationCSV, tsmi_DarkishMode });
            tsm_Settings.Name = "tsm_Settings";
            tsm_Settings.Size = new Size(61, 20);
            tsm_Settings.Text = "Settings";
            // 
            // tsmi_EditCategorisationCSV
            // 
            tsmi_EditCategorisationCSV.Image = Properties.Resources.CsvDelimited001_svg;
            tsmi_EditCategorisationCSV.Name = "tsmi_EditCategorisationCSV";
            tsmi_EditCategorisationCSV.Size = new Size(169, 22);
            tsmi_EditCategorisationCSV.Text = "Edit ETF_Types.csv";
            tsmi_EditCategorisationCSV.ToolTipText = "This will open the ETF_Types.csv in your external editor (Excel, Notepad, whatever). \r\nDon't forget to save the file & Reload (top right button)!";
            tsmi_EditCategorisationCSV.Click += tsmi_EditCategorisationCSV_Click;
            // 
            // tsmi_DarkishMode
            // 
            tsmi_DarkishMode.CheckOnClick = true;
            tsmi_DarkishMode.Name = "tsmi_DarkishMode";
            tsmi_DarkishMode.Size = new Size(169, 22);
            tsmi_DarkishMode.Text = "Dark(ish) Mode";
            tsmi_DarkishMode.Click += tsmi_DarkishMode_Click;
            // 
            // tsm_About
            // 
            tsm_About.Name = "tsm_About";
            tsm_About.Size = new Size(52, 20);
            tsm_About.Text = "About";
            tsm_About.Click += tsm_About_Click;
            // 
            // tbl_Main
            // 
            tbl_Main.ColumnCount = 3;
            tbl_Main.ColumnStyles.Add(new ColumnStyle());
            tbl_Main.ColumnStyles.Add(new ColumnStyle());
            tbl_Main.ColumnStyles.Add(new ColumnStyle());
            tbl_Main.Controls.Add(tcr_Main, 0, 1);
            tbl_Main.Controls.Add(btn_SaveToCSV, 2, 0);
            tbl_Main.Controls.Add(btn_ReloadCategories, 1, 0);
            tbl_Main.Dock = DockStyle.Fill;
            tbl_Main.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tbl_Main.Location = new Point(0, 24);
            tbl_Main.Name = "tbl_Main";
            tbl_Main.RowCount = 2;
            tbl_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tbl_Main.RowStyles.Add(new RowStyle());
            tbl_Main.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tbl_Main.Size = new Size(796, 426);
            tbl_Main.TabIndex = 7;
            // 
            // tcr_Main
            // 
            tbl_Main.SetColumnSpan(tcr_Main, 3);
            tcr_Main.Controls.Add(tpg_Scrape);
            tcr_Main.Controls.Add(tpg_Overview);
            tcr_Main.Dock = DockStyle.Fill;
            tcr_Main.Location = new Point(3, 33);
            tcr_Main.Name = "tcr_Main";
            tcr_Main.SelectedIndex = 0;
            tcr_Main.Size = new Size(790, 390);
            tcr_Main.TabIndex = 6;
            // 
            // tpg_Scrape
            // 
            tpg_Scrape.Controls.Add(tbl_ScrapeMainGrid);
            tpg_Scrape.Location = new Point(4, 24);
            tpg_Scrape.Name = "tpg_Scrape";
            tpg_Scrape.Padding = new Padding(3);
            tpg_Scrape.Size = new Size(782, 362);
            tpg_Scrape.TabIndex = 0;
            tpg_Scrape.Text = "Scrape";
            // 
            // tbl_ScrapeMainGrid
            // 
            tbl_ScrapeMainGrid.ColumnCount = 4;
            tbl_ScrapeMainGrid.ColumnStyles.Add(new ColumnStyle());
            tbl_ScrapeMainGrid.ColumnStyles.Add(new ColumnStyle());
            tbl_ScrapeMainGrid.ColumnStyles.Add(new ColumnStyle());
            tbl_ScrapeMainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tbl_ScrapeMainGrid.Controls.Add(lbl_SelectionPicker, 0, 0);
            tbl_ScrapeMainGrid.Controls.Add(lbx_Alphabet, 1, 0);
            tbl_ScrapeMainGrid.Controls.Add(btn_StartScrape, 2, 0);
            tbl_ScrapeMainGrid.Controls.Add(btn_Stop, 3, 0);
            tbl_ScrapeMainGrid.Controls.Add(gbx_Log, 0, 1);
            tbl_ScrapeMainGrid.Dock = DockStyle.Fill;
            tbl_ScrapeMainGrid.Location = new Point(3, 3);
            tbl_ScrapeMainGrid.Name = "tbl_ScrapeMainGrid";
            tbl_ScrapeMainGrid.RowCount = 2;
            tbl_ScrapeMainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tbl_ScrapeMainGrid.RowStyles.Add(new RowStyle());
            tbl_ScrapeMainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tbl_ScrapeMainGrid.Size = new Size(776, 356);
            tbl_ScrapeMainGrid.TabIndex = 7;
            // 
            // lbx_Alphabet
            // 
            lbx_Alphabet.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lbx_Alphabet.ColumnWidth = 8;
            lbx_Alphabet.FormattingEnabled = true;
            lbx_Alphabet.HorizontalScrollbar = true;
            lbx_Alphabet.ItemHeight = 15;
            lbx_Alphabet.Items.AddRange(new object[] { '0', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' });
            lbx_Alphabet.Location = new Point(171, 3);
            lbx_Alphabet.MultiColumn = true;
            lbx_Alphabet.Name = "lbx_Alphabet";
            lbx_Alphabet.SelectionMode = SelectionMode.MultiSimple;
            lbx_Alphabet.Size = new Size(242, 28);
            lbx_Alphabet.Sorted = true;
            lbx_Alphabet.TabIndex = 3;
            // 
            // btn_StartScrape
            // 
            btn_StartScrape.AutoSize = true;
            btn_StartScrape.Dock = DockStyle.Right;
            btn_StartScrape.Location = new Point(419, 3);
            btn_StartScrape.Name = "btn_StartScrape";
            btn_StartScrape.Size = new Size(94, 28);
            btn_StartScrape.TabIndex = 7;
            btn_StartScrape.Text = "Start Scrape";
            btn_StartScrape.UseVisualStyleBackColor = false;
            btn_StartScrape.Click += btn_StartScrape_Click;
            // 
            // btn_Stop
            // 
            btn_Stop.AutoSize = true;
            btn_Stop.Dock = DockStyle.Right;
            btn_Stop.Location = new Point(732, 3);
            btn_Stop.Name = "btn_Stop";
            btn_Stop.Size = new Size(41, 28);
            btn_Stop.TabIndex = 12;
            btn_Stop.Text = "Stop";
            btn_Stop.UseVisualStyleBackColor = false;
            btn_Stop.Click += btn_Stop_Click;
            // 
            // gbx_Log
            // 
            gbx_Log.AutoSize = true;
            tbl_ScrapeMainGrid.SetColumnSpan(gbx_Log, 4);
            gbx_Log.Controls.Add(tbx_Log);
            gbx_Log.Dock = DockStyle.Fill;
            gbx_Log.Location = new Point(3, 37);
            gbx_Log.Name = "gbx_Log";
            gbx_Log.Size = new Size(770, 316);
            gbx_Log.TabIndex = 8;
            gbx_Log.TabStop = false;
            gbx_Log.Text = "Log";
            // 
            // tbx_Log
            // 
            tbx_Log.Dock = DockStyle.Fill;
            tbx_Log.Location = new Point(3, 19);
            tbx_Log.Margin = new Padding(5);
            tbx_Log.Multiline = true;
            tbx_Log.Name = "tbx_Log";
            tbx_Log.ReadOnly = true;
            tbx_Log.ScrollBars = ScrollBars.Both;
            tbx_Log.Size = new Size(764, 294);
            tbx_Log.TabIndex = 2;
            // 
            // tpg_Overview
            // 
            tpg_Overview.Controls.Add(gbx_Indicators);
            tpg_Overview.Controls.Add(gbx_PickSearch);
            tpg_Overview.Controls.Add(gbx_Overview);
            tpg_Overview.Location = new Point(4, 24);
            tpg_Overview.Name = "tpg_Overview";
            tpg_Overview.Padding = new Padding(3);
            tpg_Overview.Size = new Size(782, 362);
            tpg_Overview.TabIndex = 1;
            tpg_Overview.Text = "Overview";
            tpg_Overview.Enter += tpg_Overview_Enter;
            // 
            // gbx_Indicators
            // 
            gbx_Indicators.Controls.Add(tbx_Volume);
            gbx_Indicators.Controls.Add(lbl_Volume);
            gbx_Indicators.Controls.Add(tbx_PE);
            gbx_Indicators.Controls.Add(lbl_PE);
            gbx_Indicators.Controls.Add(tbx_DivYield);
            gbx_Indicators.Controls.Add(lbl_DivYield);
            gbx_Indicators.Controls.Add(lbl_GBP_Curr);
            gbx_Indicators.Controls.Add(lbl_OC);
            gbx_Indicators.Controls.Add(tbx_YearHigh_GBP);
            gbx_Indicators.Controls.Add(tbx_YearHigh_OC);
            gbx_Indicators.Controls.Add(lbl_YearHigh);
            gbx_Indicators.Controls.Add(tbx_YearLow_GBP);
            gbx_Indicators.Controls.Add(tbx_YearLow_OC);
            gbx_Indicators.Controls.Add(lbl_YearLow);
            gbx_Indicators.Controls.Add(tbx_MarketCap_GBP);
            gbx_Indicators.Controls.Add(tbx_MarketCap_OC);
            gbx_Indicators.Controls.Add(lbl_MarketCap);
            gbx_Indicators.Controls.Add(tbx_OpenPrice_GBP);
            gbx_Indicators.Controls.Add(tbx_OpenPrice_OC);
            gbx_Indicators.Controls.Add(lbl_OpenPrice);
            gbx_Indicators.Location = new Point(430, 67);
            gbx_Indicators.Name = "gbx_Indicators";
            gbx_Indicators.Size = new Size(329, 289);
            gbx_Indicators.TabIndex = 6;
            gbx_Indicators.TabStop = false;
            gbx_Indicators.Text = "Indicators";
            // 
            // tbx_Volume
            // 
            tbx_Volume.Location = new Point(83, 252);
            tbx_Volume.Name = "tbx_Volume";
            tbx_Volume.ReadOnly = true;
            tbx_Volume.Size = new Size(85, 23);
            tbx_Volume.TabIndex = 35;
            // 
            // lbl_Volume
            // 
            lbl_Volume.AutoSize = true;
            lbl_Volume.Location = new Point(19, 255);
            lbl_Volume.Name = "lbl_Volume";
            lbl_Volume.Size = new Size(47, 15);
            lbl_Volume.TabIndex = 34;
            lbl_Volume.Text = "Volume";
            // 
            // tbx_PE
            // 
            tbx_PE.Location = new Point(83, 221);
            tbx_PE.Name = "tbx_PE";
            tbx_PE.ReadOnly = true;
            tbx_PE.Size = new Size(85, 23);
            tbx_PE.TabIndex = 33;
            // 
            // lbl_PE
            // 
            lbl_PE.AutoSize = true;
            lbl_PE.Location = new Point(19, 225);
            lbl_PE.Name = "lbl_PE";
            lbl_PE.Size = new Size(25, 15);
            lbl_PE.TabIndex = 32;
            lbl_PE.Text = "P/E";
            // 
            // tbx_DivYield
            // 
            tbx_DivYield.Location = new Point(83, 191);
            tbx_DivYield.Name = "tbx_DivYield";
            tbx_DivYield.ReadOnly = true;
            tbx_DivYield.Size = new Size(85, 23);
            tbx_DivYield.TabIndex = 31;
            // 
            // lbl_DivYield
            // 
            lbl_DivYield.AutoSize = true;
            lbl_DivYield.Location = new Point(19, 195);
            lbl_DivYield.Name = "lbl_DivYield";
            lbl_DivYield.Size = new Size(56, 15);
            lbl_DivYield.TabIndex = 30;
            lbl_DivYield.Text = "Div. Yield";
            // 
            // lbl_GBP_Curr
            // 
            lbl_GBP_Curr.AutoSize = true;
            lbl_GBP_Curr.Location = new Point(250, 22);
            lbl_GBP_Curr.Name = "lbl_GBP_Curr";
            lbl_GBP_Curr.Size = new Size(29, 15);
            lbl_GBP_Curr.TabIndex = 13;
            lbl_GBP_Curr.Text = "GBP";
            // 
            // lbl_OC
            // 
            lbl_OC.AutoSize = true;
            lbl_OC.Location = new Point(121, 22);
            lbl_OC.Name = "lbl_OC";
            lbl_OC.Size = new Size(62, 15);
            lbl_OC.TabIndex = 12;
            lbl_OC.Text = "Orig. Curr.";
            // 
            // tbx_YearHigh_GBP
            // 
            tbx_YearHigh_GBP.Location = new Point(218, 123);
            tbx_YearHigh_GBP.Name = "tbx_YearHigh_GBP";
            tbx_YearHigh_GBP.ReadOnly = true;
            tbx_YearHigh_GBP.Size = new Size(92, 23);
            tbx_YearHigh_GBP.TabIndex = 11;
            // 
            // tbx_YearHigh_OC
            // 
            tbx_YearHigh_OC.Location = new Point(106, 123);
            tbx_YearHigh_OC.Name = "tbx_YearHigh_OC";
            tbx_YearHigh_OC.ReadOnly = true;
            tbx_YearHigh_OC.Size = new Size(92, 23);
            tbx_YearHigh_OC.TabIndex = 10;
            // 
            // lbl_YearHigh
            // 
            lbl_YearHigh.AutoSize = true;
            lbl_YearHigh.Location = new Point(19, 127);
            lbl_YearHigh.Name = "lbl_YearHigh";
            lbl_YearHigh.Size = new Size(58, 15);
            lbl_YearHigh.TabIndex = 9;
            lbl_YearHigh.Text = "Year High";
            // 
            // tbx_YearLow_GBP
            // 
            tbx_YearLow_GBP.Location = new Point(218, 87);
            tbx_YearLow_GBP.Name = "tbx_YearLow_GBP";
            tbx_YearLow_GBP.ReadOnly = true;
            tbx_YearLow_GBP.Size = new Size(92, 23);
            tbx_YearLow_GBP.TabIndex = 8;
            // 
            // tbx_YearLow_OC
            // 
            tbx_YearLow_OC.Location = new Point(106, 87);
            tbx_YearLow_OC.Name = "tbx_YearLow_OC";
            tbx_YearLow_OC.ReadOnly = true;
            tbx_YearLow_OC.Size = new Size(92, 23);
            tbx_YearLow_OC.TabIndex = 7;
            // 
            // lbl_YearLow
            // 
            lbl_YearLow.AutoSize = true;
            lbl_YearLow.Location = new Point(19, 91);
            lbl_YearLow.Name = "lbl_YearLow";
            lbl_YearLow.Size = new Size(54, 15);
            lbl_YearLow.TabIndex = 6;
            lbl_YearLow.Text = "Year Low";
            // 
            // tbx_MarketCap_GBP
            // 
            tbx_MarketCap_GBP.Location = new Point(218, 156);
            tbx_MarketCap_GBP.Name = "tbx_MarketCap_GBP";
            tbx_MarketCap_GBP.ReadOnly = true;
            tbx_MarketCap_GBP.Size = new Size(92, 23);
            tbx_MarketCap_GBP.TabIndex = 5;
            // 
            // tbx_MarketCap_OC
            // 
            tbx_MarketCap_OC.Location = new Point(106, 156);
            tbx_MarketCap_OC.Name = "tbx_MarketCap_OC";
            tbx_MarketCap_OC.ReadOnly = true;
            tbx_MarketCap_OC.Size = new Size(92, 23);
            tbx_MarketCap_OC.TabIndex = 4;
            // 
            // lbl_MarketCap
            // 
            lbl_MarketCap.AutoSize = true;
            lbl_MarketCap.Location = new Point(19, 160);
            lbl_MarketCap.Name = "lbl_MarketCap";
            lbl_MarketCap.Size = new Size(68, 15);
            lbl_MarketCap.TabIndex = 3;
            lbl_MarketCap.Text = "Market Cap";
            // 
            // tbx_OpenPrice_GBP
            // 
            tbx_OpenPrice_GBP.Location = new Point(218, 54);
            tbx_OpenPrice_GBP.Name = "tbx_OpenPrice_GBP";
            tbx_OpenPrice_GBP.ReadOnly = true;
            tbx_OpenPrice_GBP.Size = new Size(92, 23);
            tbx_OpenPrice_GBP.TabIndex = 2;
            // 
            // tbx_OpenPrice_OC
            // 
            tbx_OpenPrice_OC.Location = new Point(106, 54);
            tbx_OpenPrice_OC.Name = "tbx_OpenPrice_OC";
            tbx_OpenPrice_OC.ReadOnly = true;
            tbx_OpenPrice_OC.Size = new Size(92, 23);
            tbx_OpenPrice_OC.TabIndex = 1;
            // 
            // lbl_OpenPrice
            // 
            lbl_OpenPrice.AutoSize = true;
            lbl_OpenPrice.Location = new Point(19, 58);
            lbl_OpenPrice.Name = "lbl_OpenPrice";
            lbl_OpenPrice.Size = new Size(65, 15);
            lbl_OpenPrice.TabIndex = 0;
            lbl_OpenPrice.Text = "Open Price";
            // 
            // gbx_PickSearch
            // 
            gbx_PickSearch.Controls.Add(ckb_ISAOnlySearch);
            gbx_PickSearch.Controls.Add(lbl_Filter);
            gbx_PickSearch.Controls.Add(tbx_Search);
            gbx_PickSearch.Controls.Add(cbx_Securities);
            gbx_PickSearch.Location = new Point(27, 20);
            gbx_PickSearch.Name = "gbx_PickSearch";
            gbx_PickSearch.Size = new Size(383, 100);
            gbx_PickSearch.TabIndex = 5;
            gbx_PickSearch.TabStop = false;
            gbx_PickSearch.Text = "Pick & Search";
            // 
            // ckb_ISAOnlySearch
            // 
            ckb_ISAOnlySearch.AutoSize = true;
            ckb_ISAOnlySearch.Location = new Point(268, 26);
            ckb_ISAOnlySearch.Name = "ckb_ISAOnlySearch";
            ckb_ISAOnlySearch.Size = new Size(76, 19);
            ckb_ISAOnlySearch.TabIndex = 7;
            ckb_ISAOnlySearch.Text = "ISA Only?";
            ckb_ISAOnlySearch.UseVisualStyleBackColor = true;
            ckb_ISAOnlySearch.CheckedChanged += ckb_ISAOnlySearch_CheckedChanged;
            // 
            // lbl_Filter
            // 
            lbl_Filter.AutoSize = true;
            lbl_Filter.Location = new Point(15, 25);
            lbl_Filter.Name = "lbl_Filter";
            lbl_Filter.Size = new Size(33, 15);
            lbl_Filter.TabIndex = 6;
            lbl_Filter.Text = "Filter";
            // 
            // tbx_Search
            // 
            tbx_Search.Location = new Point(60, 22);
            tbx_Search.Name = "tbx_Search";
            tbx_Search.PlaceholderText = "Type here to search";
            tbx_Search.Size = new Size(196, 23);
            tbx_Search.TabIndex = 5;
            tbx_Search.TextChanged += tbx_Search_TextChanged;
            // 
            // cbx_Securities
            // 
            cbx_Securities.FormattingEnabled = true;
            cbx_Securities.Location = new Point(15, 61);
            cbx_Securities.Name = "cbx_Securities";
            cbx_Securities.Size = new Size(340, 23);
            cbx_Securities.TabIndex = 4;
            cbx_Securities.SelectedValueChanged += cbx_Securities_SelectedValueChanged;
            // 
            // gbx_Overview
            // 
            gbx_Overview.Controls.Add(tbx_Country);
            gbx_Overview.Controls.Add(tbx_Indicies);
            gbx_Overview.Controls.Add(lbl_Indicies);
            gbx_Overview.Controls.Add(textBox1);
            gbx_Overview.Controls.Add(lbl_Country);
            gbx_Overview.Controls.Add(tbx_Currency);
            gbx_Overview.Controls.Add(lbl_Currency);
            gbx_Overview.Controls.Add(label1);
            gbx_Overview.Controls.Add(pbx_ETFType);
            gbx_Overview.Controls.Add(tbx_ETFType);
            gbx_Overview.Controls.Add(lbl_ETFType);
            gbx_Overview.Controls.Add(pbx_Sector);
            gbx_Overview.Controls.Add(tbx_Sector);
            gbx_Overview.Controls.Add(lbl_Sector);
            gbx_Overview.Controls.Add(tbx_SEDOL);
            gbx_Overview.Controls.Add(tbx_Ticker);
            gbx_Overview.Controls.Add(tbx_Name);
            gbx_Overview.Controls.Add(lbl_SEDOL);
            gbx_Overview.Controls.Add(lbl_Ticker);
            gbx_Overview.Controls.Add(ckb_ISA);
            gbx_Overview.Controls.Add(llb_URL);
            gbx_Overview.Controls.Add(lbl_Name);
            gbx_Overview.Location = new Point(27, 126);
            gbx_Overview.Name = "gbx_Overview";
            gbx_Overview.Size = new Size(383, 230);
            gbx_Overview.TabIndex = 4;
            gbx_Overview.TabStop = false;
            gbx_Overview.Text = "Overview";
            // 
            // tbx_Country
            // 
            tbx_Country.Location = new Point(248, 188);
            tbx_Country.Name = "tbx_Country";
            tbx_Country.ReadOnly = true;
            tbx_Country.Size = new Size(115, 23);
            tbx_Country.TabIndex = 29;
            // 
            // tbx_Indicies
            // 
            tbx_Indicies.Location = new Point(69, 188);
            tbx_Indicies.Name = "tbx_Indicies";
            tbx_Indicies.ReadOnly = true;
            tbx_Indicies.Size = new Size(115, 23);
            tbx_Indicies.TabIndex = 28;
            // 
            // lbl_Indicies
            // 
            lbl_Indicies.AutoSize = true;
            lbl_Indicies.Location = new Point(13, 193);
            lbl_Indicies.Name = "lbl_Indicies";
            lbl_Indicies.Size = new Size(47, 15);
            lbl_Indicies.TabIndex = 13;
            lbl_Indicies.Text = "Indicies";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(248, 87);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(115, 23);
            textBox1.TabIndex = 27;
            // 
            // lbl_Country
            // 
            lbl_Country.AutoSize = true;
            lbl_Country.Location = new Point(248, 164);
            lbl_Country.Name = "lbl_Country";
            lbl_Country.Size = new Size(50, 15);
            lbl_Country.TabIndex = 12;
            lbl_Country.Text = "Country";
            // 
            // tbx_Currency
            // 
            tbx_Currency.Location = new Point(309, 123);
            tbx_Currency.Name = "tbx_Currency";
            tbx_Currency.ReadOnly = true;
            tbx_Currency.Size = new Size(54, 23);
            tbx_Currency.TabIndex = 26;
            // 
            // lbl_Currency
            // 
            lbl_Currency.AutoSize = true;
            lbl_Currency.Location = new Point(248, 129);
            lbl_Currency.Name = "lbl_Currency";
            lbl_Currency.Size = new Size(55, 15);
            lbl_Currency.TabIndex = 25;
            lbl_Currency.Text = "Currency";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(189, 94);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 11;
            label1.Text = "Exchange";
            // 
            // pbx_ETFType
            // 
            pbx_ETFType.Image = Properties.Resources.InfoTipInline;
            pbx_ETFType.Location = new Point(196, 164);
            pbx_ETFType.Name = "pbx_ETFType";
            pbx_ETFType.Size = new Size(11, 11);
            pbx_ETFType.SizeMode = PictureBoxSizeMode.AutoSize;
            pbx_ETFType.TabIndex = 24;
            pbx_ETFType.TabStop = false;
            ttp_ETFType.SetToolTip(pbx_ETFType, "This is controlled by the ETF_Types(_example).csv file in the Resources folder. \r\nEdit that file, resave and use the Reload button on the top right to refresh this.");
            // 
            // tbx_ETFType
            // 
            tbx_ETFType.Location = new Point(69, 156);
            tbx_ETFType.Name = "tbx_ETFType";
            tbx_ETFType.ReadOnly = true;
            tbx_ETFType.Size = new Size(115, 23);
            tbx_ETFType.TabIndex = 23;
            // 
            // lbl_ETFType
            // 
            lbl_ETFType.AutoSize = true;
            lbl_ETFType.Location = new Point(13, 160);
            lbl_ETFType.Name = "lbl_ETFType";
            lbl_ETFType.Size = new Size(52, 15);
            lbl_ETFType.TabIndex = 22;
            lbl_ETFType.Text = "ETF Type";
            // 
            // pbx_Sector
            // 
            pbx_Sector.Image = Properties.Resources.InfoTipInline;
            pbx_Sector.Location = new Point(196, 129);
            pbx_Sector.Name = "pbx_Sector";
            pbx_Sector.Size = new Size(11, 11);
            pbx_Sector.SizeMode = PictureBoxSizeMode.AutoSize;
            pbx_Sector.TabIndex = 21;
            pbx_Sector.TabStop = false;
            ttp_Sector.SetToolTip(pbx_Sector, "Sector is either a crude attempt to classify things as Bonds/Gilts/Trusts/ETF/None of those.\r\nIf \"None of those\" then the script pulls the \"Sector\" value from the corporate-info subpage.\r\n");
            // 
            // tbx_Sector
            // 
            tbx_Sector.Location = new Point(69, 123);
            tbx_Sector.Name = "tbx_Sector";
            tbx_Sector.ReadOnly = true;
            tbx_Sector.Size = new Size(115, 23);
            tbx_Sector.TabIndex = 20;
            // 
            // lbl_Sector
            // 
            lbl_Sector.AutoSize = true;
            lbl_Sector.Location = new Point(13, 126);
            lbl_Sector.Name = "lbl_Sector";
            lbl_Sector.Size = new Size(40, 15);
            lbl_Sector.TabIndex = 19;
            lbl_Sector.Text = "Sector";
            // 
            // tbx_SEDOL
            // 
            tbx_SEDOL.Location = new Point(248, 55);
            tbx_SEDOL.Name = "tbx_SEDOL";
            tbx_SEDOL.ReadOnly = true;
            tbx_SEDOL.Size = new Size(115, 23);
            tbx_SEDOL.TabIndex = 18;
            // 
            // tbx_Ticker
            // 
            tbx_Ticker.Location = new Point(69, 54);
            tbx_Ticker.Name = "tbx_Ticker";
            tbx_Ticker.ReadOnly = true;
            tbx_Ticker.Size = new Size(115, 23);
            tbx_Ticker.TabIndex = 17;
            // 
            // tbx_Name
            // 
            tbx_Name.Location = new Point(69, 18);
            tbx_Name.Name = "tbx_Name";
            tbx_Name.ReadOnly = true;
            tbx_Name.Size = new Size(241, 23);
            tbx_Name.TabIndex = 16;
            // 
            // lbl_SEDOL
            // 
            lbl_SEDOL.AutoSize = true;
            lbl_SEDOL.Location = new Point(192, 59);
            lbl_SEDOL.Name = "lbl_SEDOL";
            lbl_SEDOL.Size = new Size(42, 15);
            lbl_SEDOL.TabIndex = 15;
            lbl_SEDOL.Text = "SEDOL";
            // 
            // lbl_Ticker
            // 
            lbl_Ticker.AutoSize = true;
            lbl_Ticker.Location = new Point(13, 58);
            lbl_Ticker.Name = "lbl_Ticker";
            lbl_Ticker.Size = new Size(38, 15);
            lbl_Ticker.TabIndex = 14;
            lbl_Ticker.Text = "Ticker";
            // 
            // ckb_ISA
            // 
            ckb_ISA.AutoSize = true;
            ckb_ISA.Enabled = false;
            ckb_ISA.Location = new Point(15, 93);
            ckb_ISA.Name = "ckb_ISA";
            ckb_ISA.Size = new Size(67, 19);
            ckb_ISA.TabIndex = 12;
            ckb_ISA.Text = "ckb_ISA";
            ckb_ISA.UseVisualStyleBackColor = true;
            // 
            // llb_URL
            // 
            llb_URL.AutoSize = true;
            llb_URL.Location = new Point(327, 22);
            llb_URL.Name = "llb_URL";
            llb_URL.Size = new Size(28, 15);
            llb_URL.TabIndex = 11;
            llb_URL.TabStop = true;
            llb_URL.Text = "URL";
            llb_URL.LinkClicked += llb_URL_LinkClicked;
            // 
            // lbl_Name
            // 
            lbl_Name.AutoSize = true;
            lbl_Name.Location = new Point(13, 22);
            lbl_Name.Name = "lbl_Name";
            lbl_Name.Size = new Size(39, 15);
            lbl_Name.TabIndex = 10;
            lbl_Name.Text = "Name";
            // 
            // btn_SaveToCSV
            // 
            btn_SaveToCSV.AutoSize = true;
            btn_SaveToCSV.Dock = DockStyle.Right;
            btn_SaveToCSV.Location = new Point(684, 3);
            btn_SaveToCSV.Name = "btn_SaveToCSV";
            btn_SaveToCSV.Size = new Size(109, 24);
            btn_SaveToCSV.TabIndex = 14;
            btn_SaveToCSV.Text = "Save Data to CSV";
            btn_SaveToCSV.UseVisualStyleBackColor = false;
            btn_SaveToCSV.Click += btn_SaveToCSV_Click;
            // 
            // btn_ReloadCategories
            // 
            btn_ReloadCategories.AutoSize = true;
            btn_ReloadCategories.Dock = DockStyle.Right;
            btn_ReloadCategories.Location = new Point(471, 3);
            btn_ReloadCategories.Name = "btn_ReloadCategories";
            btn_ReloadCategories.Size = new Size(207, 24);
            btn_ReloadCategories.TabIndex = 13;
            btn_ReloadCategories.Text = "Reload ETF Categorisation from CSV";
            btn_ReloadCategories.UseVisualStyleBackColor = false;
            btn_ReloadCategories.Click += btn_ReloadCategories_Click;
            // 
            // lbl_SelectionPicker
            // 
            lbl_SelectionPicker.AutoSize = true;
            lbl_SelectionPicker.Dock = DockStyle.Fill;
            lbl_SelectionPicker.Location = new Point(3, 0);
            lbl_SelectionPicker.Name = "lbl_SelectionPicker";
            lbl_SelectionPicker.Size = new Size(162, 34);
            lbl_SelectionPicker.TabIndex = 3;
            lbl_SelectionPicker.Text = "Select which letters to scrape \r\n(you prob want all of them):";
            lbl_SelectionPicker.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmMainApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(796, 450);
            Controls.Add(tbl_Main);
            Controls.Add(mns_Main);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mns_Main;
            MaximizeBox = false;
            Name = "FrmMainApp";
            Text = "HL Web Scraper/Parser";
            Load += FrmMainApp_Load;
            mns_Main.ResumeLayout(false);
            mns_Main.PerformLayout();
            tbl_Main.ResumeLayout(false);
            tbl_Main.PerformLayout();
            tcr_Main.ResumeLayout(false);
            tpg_Scrape.ResumeLayout(false);
            tbl_ScrapeMainGrid.ResumeLayout(false);
            tbl_ScrapeMainGrid.PerformLayout();
            gbx_Log.ResumeLayout(false);
            gbx_Log.PerformLayout();
            tpg_Overview.ResumeLayout(false);
            gbx_Indicators.ResumeLayout(false);
            gbx_Indicators.PerformLayout();
            gbx_PickSearch.ResumeLayout(false);
            gbx_PickSearch.PerformLayout();
            gbx_Overview.ResumeLayout(false);
            gbx_Overview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbx_ETFType).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbx_Sector).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip mns_Main;
        private ToolStripMenuItem tsm_Settings;
        private ToolStripMenuItem tsmi_EditCategorisationCSV;
        private ToolStripMenuItem tsm_About;
        private TableLayoutPanel tbl_Main;
        private TabControl tcr_Main;
        private TabPage tpg_Scrape;
        private TableLayoutPanel tbl_ScrapeMainGrid;
        private Button btn_Stop;
        private GroupBox gbx_Log;
        private Button btn_ReloadCategories;
        private Button btn_SaveToCSV;
        private TabPage tpg_Overview;
        private GroupBox gbx_PickSearch;
        private TextBox tbx_Search;
        private ComboBox cbx_Securities;
        private GroupBox gbx_Overview;
        private Label lbl_SEDOL;
        private Label lbl_Ticker;
        private CheckBox ckb_ISA;
        private LinkLabel llb_URL;
        private Label lbl_Name;
        private TextBox tbx_ETFType;
        private Label lbl_Filter;
        private TextBox tbx_SEDOL;
        private TextBox tbx_Ticker;
        private TextBox tbx_Name;
        private TextBox tbx_Sector;
        private Label lbl_Sector;
        private PictureBox pbx_Sector;
        private ToolTip ttp_Sector;
        private Label lbl_ETFType;
        private PictureBox pbx_ETFType;
        private ToolTip ttp_ETFType;
        private GroupBox gbx_Indicators;
        private Label lbl_Indicies;
        private Label lbl_Country;
        private Label label1;
        private TextBox tbx_Currency;
        private Label lbl_Currency;
        private TextBox textBox1;
        private CheckBox ckb_ISAOnlySearch;
        private TextBox tbx_Country;
        private TextBox tbx_Indicies;
        private Label lbl_OpenPrice;
        private TextBox tbx_YearHigh_GBP;
        private TextBox tbx_YearHigh_OC;
        private Label lbl_YearHigh;
        private TextBox tbx_YearLow_GBP;
        private TextBox tbx_YearLow_OC;
        private Label lbl_YearLow;
        private TextBox tbx_MarketCap_GBP;
        private TextBox tbx_MarketCap_OC;
        private Label lbl_MarketCap;
        private TextBox tbx_OpenPrice_GBP;
        private TextBox tbx_OpenPrice_OC;
        private Label lbl_GBP_Curr;
        private Label lbl_OC;
        private TextBox tbx_DivYield;
        private Label lbl_DivYield;
        private TextBox tbx_PE;
        private Label lbl_PE;
        private TextBox tbx_Volume;
        private Label lbl_Volume;
        private ToolStripMenuItem tsmi_DarkishMode;
        private Button btn_StartScrape;
        private TextBox tbx_Log;
        private ListBox lbx_Alphabet;
        private Label lbl_SelectionPicker;
    }
}
