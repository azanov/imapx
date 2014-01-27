namespace ImapX.Sample
{
    partial class FrmBodyStructure
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
            this.txtStructure = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtStructure
            // 
            this.txtStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStructure.Location = new System.Drawing.Point(12, 12);
            this.txtStructure.Multiline = true;
            this.txtStructure.Name = "txtStructure";
            this.txtStructure.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStructure.Size = new System.Drawing.Size(418, 216);
            this.txtStructure.TabIndex = 0;
            // 
            // FrmBodyStructure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(442, 240);
            this.Controls.Add(this.txtStructure);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmBodyStructure";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Body Structure";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStructure;
    }
}