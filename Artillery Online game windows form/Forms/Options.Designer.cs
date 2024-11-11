namespace Artillery_Online_game_windows_form
{
    partial class Options
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
            this.IPtextBox = new System.Windows.Forms.TextBox();
            this.PrtTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // IPtextBox
            // 
            this.IPtextBox.Location = new System.Drawing.Point(12, 12);
            this.IPtextBox.Name = "IPtextBox";
            this.IPtextBox.Size = new System.Drawing.Size(92, 20);
            this.IPtextBox.TabIndex = 0;
            this.IPtextBox.Text = "192.168.1.6";
            // 
            // PrtTextBox
            // 
            this.PrtTextBox.Location = new System.Drawing.Point(110, 12);
            this.PrtTextBox.Name = "PrtTextBox";
            this.PrtTextBox.Size = new System.Drawing.Size(47, 20);
            this.PrtTextBox.TabIndex = 1;
            this.PrtTextBox.Text = "912";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(169, 52);
            this.Controls.Add(this.PrtTextBox);
            this.Controls.Add(this.IPtextBox);
            this.Name = "Options";
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox IPtextBox;
        public System.Windows.Forms.TextBox PrtTextBox;
    }
}