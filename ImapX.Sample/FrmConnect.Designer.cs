using ImapX.Sample.Controls;

namespace ImapX.Sample
{
    partial class FrmConnect
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbcMain = new ImapX.Sample.Controls.TabControlEx();
            this.tbpConnect = new System.Windows.Forms.TabPage();
            this.tlpConnect = new System.Windows.Forms.TableLayoutPanel();
            this.pnlConnect = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cmbHost = new System.Windows.Forms.ComboBox();
            this.chkValidateCertificate = new System.Windows.Forms.CheckBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.cmbSecurity = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbpSignIn = new System.Windows.Forms.TabPage();
            this.tlpSignIn = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSignIn = new System.Windows.Forms.Panel();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnSignIn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCurrentHost = new System.Windows.Forms.Label();
            this.cueBannerExtender1 = new ImapX.Sample.Extenders.CueBannerExtender();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tbcMain.SuspendLayout();
            this.tbpConnect.SuspendLayout();
            this.tlpConnect.SuspendLayout();
            this.pnlConnect.SuspendLayout();
            this.tbpSignIn.SuspendLayout();
            this.tlpSignIn.SuspendLayout();
            this.pnlSignIn.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.pictureBox1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(1, 1);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(523, 100);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Header_MouseDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::ImapX.Sample.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(230, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Header_MouseDown);
            // 
            // tbcMain
            // 
            this.tbcMain.Controls.Add(this.tbpConnect);
            this.tbcMain.Controls.Add(this.tbpSignIn);
            this.tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcMain.Location = new System.Drawing.Point(1, 1);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.RemoveGap = true;
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(523, 521);
            this.tbcMain.TabIndex = 4;
            // 
            // tbpConnect
            // 
            this.tbpConnect.Controls.Add(this.tlpConnect);
            this.tbpConnect.Location = new System.Drawing.Point(-3, 19);
            this.tbpConnect.Name = "tbpConnect";
            this.tbpConnect.Padding = new System.Windows.Forms.Padding(3, 80, 3, 3);
            this.tbpConnect.Size = new System.Drawing.Size(529, 505);
            this.tbpConnect.TabIndex = 0;
            this.tbpConnect.Text = "Connect";
            this.tbpConnect.UseVisualStyleBackColor = true;
            // 
            // tlpConnect
            // 
            this.tlpConnect.BackgroundImage = global::ImapX.Sample.Properties.Resources.pattern8;
            this.tlpConnect.ColumnCount = 3;
            this.tlpConnect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpConnect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlpConnect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpConnect.Controls.Add(this.pnlConnect, 1, 1);
            this.tlpConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpConnect.Location = new System.Drawing.Point(3, 80);
            this.tlpConnect.Name = "tlpConnect";
            this.tlpConnect.RowCount = 3;
            this.tlpConnect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpConnect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpConnect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpConnect.Size = new System.Drawing.Size(523, 422);
            this.tlpConnect.TabIndex = 14;
            // 
            // pnlConnect
            // 
            this.pnlConnect.BackColor = System.Drawing.Color.White;
            this.pnlConnect.Controls.Add(this.label2);
            this.pnlConnect.Controls.Add(this.txtPort);
            this.pnlConnect.Controls.Add(this.btnConnect);
            this.pnlConnect.Controls.Add(this.cmbHost);
            this.pnlConnect.Controls.Add(this.chkValidateCertificate);
            this.pnlConnect.Controls.Add(this.lblHost);
            this.pnlConnect.Controls.Add(this.cmbSecurity);
            this.pnlConnect.Controls.Add(this.label3);
            this.pnlConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConnect.Location = new System.Drawing.Point(81, 87);
            this.pnlConnect.Name = "pnlConnect";
            this.pnlConnect.Size = new System.Drawing.Size(360, 247);
            this.pnlConnect.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // txtPort
            // 
            this.cueBannerExtender1.SetCueBannerText(this.txtPort, "");
            this.txtPort.Location = new System.Drawing.Point(108, 103);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(214, 25);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "993";
            this.txtPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Connect_KeyDown);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(33, 177);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(289, 30);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cmbHost
            // 
            this.cueBannerExtender1.SetCueBannerText(this.cmbHost, "");
            this.cmbHost.FormattingEnabled = true;
            this.cmbHost.Items.AddRange(new object[] {
            "imap.gmail.com",
            "imap-mail.outlook.com",
            "imap.mail.yahoo.com",
            "imap.yandex.ru",
            "imap.aol.com"});
            this.cmbHost.Location = new System.Drawing.Point(108, 41);
            this.cmbHost.Name = "cmbHost";
            this.cmbHost.Size = new System.Drawing.Size(214, 25);
            this.cmbHost.TabIndex = 0;
            this.cmbHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Connect_KeyDown);
            // 
            // chkValidateCertificate
            // 
            this.chkValidateCertificate.AutoSize = true;
            this.chkValidateCertificate.Checked = true;
            this.chkValidateCertificate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkValidateCertificate.Location = new System.Drawing.Point(108, 134);
            this.chkValidateCertificate.Name = "chkValidateCertificate";
            this.chkValidateCertificate.Size = new System.Drawing.Size(174, 21);
            this.chkValidateCertificate.TabIndex = 3;
            this.chkValidateCertificate.Text = "Validate server certificate";
            this.chkValidateCertificate.UseVisualStyleBackColor = true;
            this.chkValidateCertificate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Connect_KeyDown);
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(30, 44);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(35, 17);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Host";
            // 
            // cmbSecurity
            // 
            this.cueBannerExtender1.SetCueBannerText(this.cmbSecurity, "");
            this.cmbSecurity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSecurity.DropDownWidth = 200;
            this.cmbSecurity.FormattingEnabled = true;
            this.cmbSecurity.Location = new System.Drawing.Point(108, 72);
            this.cmbSecurity.Name = "cmbSecurity";
            this.cmbSecurity.Size = new System.Drawing.Size(214, 25);
            this.cmbSecurity.TabIndex = 1;
            this.cmbSecurity.SelectedIndexChanged += new System.EventHandler(this.cmbSecurity_SelectedIndexChanged);
            this.cmbSecurity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Connect_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Security";
            // 
            // tbpSignIn
            // 
            this.tbpSignIn.Controls.Add(this.tlpSignIn);
            this.tbpSignIn.Location = new System.Drawing.Point(-3, 19);
            this.tbpSignIn.Name = "tbpSignIn";
            this.tbpSignIn.Padding = new System.Windows.Forms.Padding(3, 80, 3, 3);
            this.tbpSignIn.Size = new System.Drawing.Size(529, 505);
            this.tbpSignIn.TabIndex = 1;
            this.tbpSignIn.Text = "Sign in";
            this.tbpSignIn.UseVisualStyleBackColor = true;
            // 
            // tlpSignIn
            // 
            this.tlpSignIn.BackgroundImage = global::ImapX.Sample.Properties.Resources.pattern8;
            this.tlpSignIn.ColumnCount = 3;
            this.tlpSignIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpSignIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlpSignIn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpSignIn.Controls.Add(this.pnlSignIn, 1, 1);
            this.tlpSignIn.Controls.Add(this.lblCurrentHost, 1, 0);
            this.tlpSignIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSignIn.Location = new System.Drawing.Point(3, 80);
            this.tlpSignIn.Name = "tlpSignIn";
            this.tlpSignIn.RowCount = 3;
            this.tlpSignIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpSignIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpSignIn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpSignIn.Size = new System.Drawing.Size(523, 422);
            this.tlpSignIn.TabIndex = 15;
            // 
            // pnlSignIn
            // 
            this.pnlSignIn.BackColor = System.Drawing.Color.White;
            this.pnlSignIn.Controls.Add(this.lblLanguage);
            this.pnlSignIn.Controls.Add(this.cmbLanguage);
            this.pnlSignIn.Controls.Add(this.btnDisconnect);
            this.pnlSignIn.Controls.Add(this.txtUser);
            this.pnlSignIn.Controls.Add(this.label1);
            this.pnlSignIn.Controls.Add(this.txtPassword);
            this.pnlSignIn.Controls.Add(this.btnSignIn);
            this.pnlSignIn.Controls.Add(this.label4);
            this.pnlSignIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSignIn.Location = new System.Drawing.Point(81, 87);
            this.pnlSignIn.Name = "pnlSignIn";
            this.pnlSignIn.Size = new System.Drawing.Size(360, 247);
            this.pnlSignIn.TabIndex = 14;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(33, 177);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(95, 30);
            this.btnDisconnect.TabIndex = 4;
            this.btnDisconnect.Text = "disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // txtUser
            // 
            this.cueBannerExtender1.SetCueBannerText(this.txtUser, "");
            this.txtUser.Location = new System.Drawing.Point(108, 41);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(214, 25);
            this.txtUser.TabIndex = 0;
            this.txtUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SignIn_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Password";
            // 
            // txtPassword
            // 
            this.cueBannerExtender1.SetCueBannerText(this.txtPassword, "");
            this.txtPassword.Location = new System.Drawing.Point(108, 72);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(214, 25);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SignIn_KeyDown);
            // 
            // btnSignIn
            // 
            this.btnSignIn.Location = new System.Drawing.Point(134, 177);
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(188, 30);
            this.btnSignIn.TabIndex = 3;
            this.btnSignIn.Text = "sign in";
            this.btnSignIn.UseVisualStyleBackColor = true;
            this.btnSignIn.Click += new System.EventHandler(this.btnSignIn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "User";
            // 
            // lblCurrentHost
            // 
            this.lblCurrentHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(173)))), ((int)(((byte)(231)))));
            this.lblCurrentHost.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCurrentHost.ForeColor = System.Drawing.Color.White;
            this.lblCurrentHost.Location = new System.Drawing.Point(81, 48);
            this.lblCurrentHost.Name = "lblCurrentHost";
            this.lblCurrentHost.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.lblCurrentHost.Size = new System.Drawing.Size(360, 36);
            this.lblCurrentHost.TabIndex = 17;
            this.lblCurrentHost.Text = "imap.gmail.com";
            this.lblCurrentHost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cueBannerExtender1
            // 
            this.cueBannerExtender1.ShowOnFocus = false;
            // 
            // cmbLanguage
            // 
            this.cueBannerExtender1.SetCueBannerText(this.cmbLanguage, "");
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(108, 103);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(214, 25);
            this.cmbLanguage.TabIndex = 2;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(30, 106);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(65, 17);
            this.lblLanguage.TabIndex = 5;
            this.lblLanguage.Text = "Language";
            // 
            // FrmConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(525, 523);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.tbcMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FrmConnect";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImapX - Connect";
            this.Load += new System.EventHandler(this.FrmConnect_Load);
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tbcMain.ResumeLayout(false);
            this.tbpConnect.ResumeLayout(false);
            this.tlpConnect.ResumeLayout(false);
            this.pnlConnect.ResumeLayout(false);
            this.pnlConnect.PerformLayout();
            this.tbpSignIn.ResumeLayout(false);
            this.tlpSignIn.ResumeLayout(false);
            this.pnlSignIn.ResumeLayout(false);
            this.pnlSignIn.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.PictureBox pictureBox1;
        private TabControlEx tbcMain;
        private System.Windows.Forms.TabPage tbpConnect;
        private System.Windows.Forms.TabPage tbpSignIn;
        private System.Windows.Forms.TableLayoutPanel tlpConnect;
        private System.Windows.Forms.Panel pnlConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cmbHost;
        private System.Windows.Forms.CheckBox chkValidateCertificate;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.ComboBox cmbSecurity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tlpSignIn;
        private System.Windows.Forms.Panel pnlSignIn;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblCurrentHost;
        private Extenders.CueBannerExtender cueBannerExtender1;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox cmbLanguage;
    }
}