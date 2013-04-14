namespace ImapX.Sample
{
    partial class FrmGOAuth
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
            this.label3 = new System.Windows.Forms.Label();
            this.wbrAuth = new System.Windows.Forms.WebBrowser();
            this.lblWait = new System.Windows.Forms.Label();
            this.bgwConnect = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(218, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Sign in using Google OAuth 2";
            // 
            // wbrAuth
            // 
            this.wbrAuth.Location = new System.Drawing.Point(12, 47);
            this.wbrAuth.MinimumSize = new System.Drawing.Size(23, 23);
            this.wbrAuth.Name = "wbrAuth";
            this.wbrAuth.ScrollBarsEnabled = false;
            this.wbrAuth.Size = new System.Drawing.Size(675, 369);
            this.wbrAuth.TabIndex = 7;
            this.wbrAuth.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbrAuth_DocumentCompleted);
            // 
            // lblWait
            // 
            this.lblWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWait.Location = new System.Drawing.Point(12, 195);
            this.lblWait.Name = "lblWait";
            this.lblWait.Padding = new System.Windows.Forms.Padding(5);
            this.lblWait.Size = new System.Drawing.Size(676, 62);
            this.lblWait.TabIndex = 8;
            this.lblWait.Text = "please wait while the application retrieves an access token for {0}";
            this.lblWait.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblWait.Visible = false;
            // 
            // bgwConnect
            // 
            this.bgwConnect.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwConnect_DoWork);
            this.bgwConnect.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwConnect_RunWorkerCompleted);
            // 
            // FrmGOAuth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(699, 432);
            this.Controls.Add(this.lblWait);
            this.Controls.Add(this.wbrAuth);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGOAuth";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign in with Google OAuth 2";
            this.Load += new System.EventHandler(this.FrmGOAuth_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.WebBrowser wbrAuth;
        private System.Windows.Forms.Label lblWait;
        private System.ComponentModel.BackgroundWorker bgwConnect;
    }
}