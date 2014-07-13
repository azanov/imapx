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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConnect));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnDefaultAuth = new System.Windows.Forms.Button();
            this.picGMailLogin = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.chkValidateCertificate = new System.Windows.Forms.CheckBox();
            this.btnSignIn = new System.Windows.Forms.Button();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.cmbEncryption = new System.Windows.Forms.ComboBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.lblLogin = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblWait = new System.Windows.Forms.Label();
            this.wbrMain = new System.Windows.Forms.WebBrowser();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tltMain = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGMailLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.btnDefaultAuth);
            this.pnlTop.Controls.Add(this.picGMailLogin);
            this.pnlTop.Controls.Add(this.picLogo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(24, 12, 12, 12);
            this.pnlTop.Size = new System.Drawing.Size(544, 84);
            this.pnlTop.TabIndex = 1;
            // 
            // btnDefaultAuth
            // 
            this.btnDefaultAuth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(74)))), ((int)(((byte)(50)))));
            this.btnDefaultAuth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefaultAuth.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefaultAuth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(247)))), ((int)(((byte)(246)))));
            this.btnDefaultAuth.Location = new System.Drawing.Point(337, 22);
            this.btnDefaultAuth.Name = "btnDefaultAuth";
            this.btnDefaultAuth.Size = new System.Drawing.Size(182, 40);
            this.btnDefaultAuth.TabIndex = 2;
            this.btnDefaultAuth.Text = "Default authentication";
            this.btnDefaultAuth.UseVisualStyleBackColor = false;
            this.btnDefaultAuth.Visible = false;
            this.btnDefaultAuth.Click += new System.EventHandler(this.btnDefaultAuth_Click);
            // 
            // picGMailLogin
            // 
            this.picGMailLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picGMailLogin.Image = global::ImapX.Sample.Properties.Resources.sign_in_with_google;
            this.picGMailLogin.Location = new System.Drawing.Point(337, 22);
            this.picGMailLogin.Name = "picGMailLogin";
            this.picGMailLogin.Size = new System.Drawing.Size(182, 40);
            this.picGMailLogin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picGMailLogin.TabIndex = 1;
            this.picGMailLogin.TabStop = false;
            this.picGMailLogin.Click += new System.EventHandler(this.picGMailLogin_Click);
            // 
            // picLogo
            // 
            this.picLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.picLogo.Image = global::ImapX.Sample.Properties.Resources.logo;
            this.picLogo.Location = new System.Drawing.Point(24, 12);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(180, 60);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            this.picLogo.Click += new System.EventHandler(this.picLogo_Click);
            // 
            // pnlLogin
            // 
            this.pnlLogin.BackColor = System.Drawing.Color.Transparent;
            this.pnlLogin.Controls.Add(this.cmbServer);
            this.pnlLogin.Controls.Add(this.chkValidateCertificate);
            this.pnlLogin.Controls.Add(this.btnSignIn);
            this.pnlLogin.Controls.Add(this.lblPassword);
            this.pnlLogin.Controls.Add(this.txtPassword);
            this.pnlLogin.Controls.Add(this.cmbEncryption);
            this.pnlLogin.Controls.Add(this.lblPort);
            this.pnlLogin.Controls.Add(this.cmbPort);
            this.pnlLogin.Controls.Add(this.lblLogin);
            this.pnlLogin.Controls.Add(this.txtLogin);
            this.pnlLogin.Controls.Add(this.lblServer);
            this.pnlLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogin.Location = new System.Drawing.Point(0, 84);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(544, 357);
            this.pnlLogin.TabIndex = 2;
            // 
            // cmbServer
            // 
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Items.AddRange(new object[] {
            "imap.gmail.com",
            "imap-mail.outlook.com",
            "imap.yandex.ru",
            "imap.mail.yahoo.com",
            "imap.aol.com"});
            this.cmbServer.Location = new System.Drawing.Point(208, 89);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(210, 25);
            this.cmbServer.TabIndex = 13;
            this.cmbServer.Text = "imap.gmail.com";
            // 
            // chkValidateCertificate
            // 
            this.chkValidateCertificate.AutoSize = true;
            this.chkValidateCertificate.Checked = true;
            this.chkValidateCertificate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkValidateCertificate.Location = new System.Drawing.Point(403, 126);
            this.chkValidateCertificate.Name = "chkValidateCertificate";
            this.chkValidateCertificate.Size = new System.Drawing.Size(15, 14);
            this.chkValidateCertificate.TabIndex = 3;
            this.tltMain.SetToolTip(this.chkValidateCertificate, "Validate server certificate");
            this.chkValidateCertificate.UseVisualStyleBackColor = true;
            this.chkValidateCertificate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLogin_KeyDown);
            // 
            // btnSignIn
            // 
            this.btnSignIn.Location = new System.Drawing.Point(208, 213);
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(210, 30);
            this.btnSignIn.TabIndex = 6;
            this.btnSignIn.Text = "Sign in";
            this.btnSignIn.UseVisualStyleBackColor = true;
            this.btnSignIn.Click += new System.EventHandler(this.btnSignIn_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(114, 185);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(64, 17);
            this.lblPassword.TabIndex = 12;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(208, 182);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(210, 25);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLogin_KeyDown);
            // 
            // cmbEncryption
            // 
            this.cmbEncryption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncryption.FormattingEnabled = true;
            this.cmbEncryption.Items.AddRange(new object[] {
            "None",
            "SSL",
            "TLS"});
            this.cmbEncryption.Location = new System.Drawing.Point(316, 120);
            this.cmbEncryption.Name = "cmbEncryption";
            this.cmbEncryption.Size = new System.Drawing.Size(79, 25);
            this.cmbEncryption.TabIndex = 2;
            this.cmbEncryption.SelectedIndexChanged += new System.EventHandler(this.cmbEncryption_SelectedIndexChanged);
            this.cmbEncryption.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLogin_KeyDown);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(114, 123);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(32, 17);
            this.lblPort.TabIndex = 9;
            this.lblPort.Text = "Port";
            // 
            // cmbPort
            // 
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Items.AddRange(new object[] {
            "143",
            "993"});
            this.cmbPort.Location = new System.Drawing.Point(208, 120);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(102, 25);
            this.cmbPort.TabIndex = 1;
            this.cmbPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbPort_KeyDown);
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(114, 154);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(40, 17);
            this.lblLogin.TabIndex = 7;
            this.lblLogin.Text = "Login";
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(208, 151);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(210, 25);
            this.txtLogin.TabIndex = 4;
            this.txtLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLogin_KeyDown);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(114, 92);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(45, 17);
            this.lblServer.TabIndex = 3;
            this.lblServer.Text = "Server";
            // 
            // lblWait
            // 
            this.lblWait.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(74)))), ((int)(((byte)(50)))));
            this.lblWait.ForeColor = System.Drawing.Color.White;
            this.lblWait.Location = new System.Drawing.Point(169, 255);
            this.lblWait.Name = "lblWait";
            this.lblWait.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.lblWait.Size = new System.Drawing.Size(212, 37);
            this.lblWait.TabIndex = 15;
            this.lblWait.Text = "Connecting...";
            this.lblWait.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblWait.Visible = false;
            // 
            // wbrMain
            // 
            this.wbrMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbrMain.Location = new System.Drawing.Point(0, 0);
            this.wbrMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbrMain.Name = "wbrMain";
            this.wbrMain.Size = new System.Drawing.Size(544, 441);
            this.wbrMain.TabIndex = 14;
            this.wbrMain.Visible = false;
            this.wbrMain.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbrMain_DocumentCompleted);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 84);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(12);
            this.lblTitle.Size = new System.Drawing.Size(544, 49);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Sign in";
            // 
            // FrmConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(544, 441);
            this.Controls.Add(this.lblWait);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlLogin);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.wbrMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FrmConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImapX";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGMailLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.ComboBox cmbEncryption;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.PictureBox picGMailLogin;
        private System.Windows.Forms.WebBrowser wbrMain;
        private System.Windows.Forms.Button btnDefaultAuth;
        private System.Windows.Forms.Label lblWait;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckBox chkValidateCertificate;
        private System.Windows.Forms.ToolTip tltMain;
        private System.Windows.Forms.ComboBox cmbServer;
    }
}