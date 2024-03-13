namespace HLWebScraper.Net
{
    partial class FrmAboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAboutBox));
            tableLayoutPanel = new TableLayoutPanel();
            rtb_AboutBox = new RichTextBox();
            pbx_Logo = new PictureBox();
            tbx_Description = new TextBox();
            btn_OK = new Button();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbx_Logo).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67F));
            tableLayoutPanel.Controls.Add(rtb_AboutBox, 1, 0);
            tableLayoutPanel.Controls.Add(pbx_Logo, 0, 0);
            tableLayoutPanel.Controls.Add(tbx_Description, 1, 1);
            tableLayoutPanel.Controls.Add(btn_OK, 1, 2);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(10, 10);
            tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.Size = new Size(650, 307);
            tableLayoutPanel.TabIndex = 0;
            // 
            // rtb_AboutBox
            // 
            rtb_AboutBox.BorderStyle = BorderStyle.None;
            rtb_AboutBox.Location = new Point(218, 3);
            rtb_AboutBox.Margin = new Padding(4, 3, 4, 3);
            rtb_AboutBox.Name = "rtb_AboutBox";
            rtb_AboutBox.ReadOnly = true;
            rtb_AboutBox.Size = new Size(426, 175);
            rtb_AboutBox.TabIndex = 29;
            rtb_AboutBox.Text = "";
            // 
            // pbx_Logo
            // 
            pbx_Logo.Dock = DockStyle.Fill;
            pbx_Logo.Image = (Image)resources.GetObject("pbx_Logo.Image");
            pbx_Logo.InitialImage = null;
            pbx_Logo.Location = new Point(4, 3);
            pbx_Logo.Margin = new Padding(4, 3, 4, 3);
            pbx_Logo.Name = "pbx_Logo";
            tableLayoutPanel.SetRowSpan(pbx_Logo, 2);
            pbx_Logo.Size = new Size(206, 268);
            pbx_Logo.SizeMode = PictureBoxSizeMode.StretchImage;
            pbx_Logo.TabIndex = 12;
            pbx_Logo.TabStop = false;
            // 
            // tbx_Description
            // 
            tbx_Description.BorderStyle = BorderStyle.None;
            tbx_Description.Dock = DockStyle.Fill;
            tbx_Description.Location = new Point(220, 190);
            tbx_Description.Margin = new Padding(6);
            tbx_Description.Multiline = true;
            tbx_Description.Name = "tbx_Description";
            tbx_Description.ReadOnly = true;
            tbx_Description.ScrollBars = ScrollBars.Both;
            tbx_Description.Size = new Size(424, 78);
            tbx_Description.TabIndex = 23;
            tbx_Description.TabStop = false;
            // 
            // btn_OK
            // 
            btn_OK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn_OK.DialogResult = DialogResult.Cancel;
            btn_OK.Location = new Point(558, 277);
            btn_OK.Margin = new Padding(4, 3, 4, 3);
            btn_OK.Name = "btn_OK";
            btn_OK.Size = new Size(88, 27);
            btn_OK.TabIndex = 24;
            btn_OK.Text = "&OK";
            btn_OK.Click += Btn_OK_Click;
            // 
            // FrmAboutBox
            // 
            AcceptButton = btn_OK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(670, 327);
            Controls.Add(tableLayoutPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAboutBox";
            Padding = new Padding(10);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmAboutBox";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbx_Logo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox pbx_Logo;
        private System.Windows.Forms.TextBox tbx_Description;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.RichTextBox rtb_AboutBox;
    }
}
