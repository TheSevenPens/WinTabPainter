namespace WinTabPainter
{
    partial class FormAbout
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
            this.linkLabel_GitHubRepo = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // linkLabel_GitHubRepo
            // 
            this.linkLabel_GitHubRepo.AutoSize = true;
            this.linkLabel_GitHubRepo.Location = new System.Drawing.Point(284, 38);
            this.linkLabel_GitHubRepo.Name = "linkLabel_GitHubRepo";
            this.linkLabel_GitHubRepo.Size = new System.Drawing.Size(640, 32);
            this.linkLabel_GitHubRepo.TabIndex = 0;
            this.linkLabel_GitHubRepo.TabStop = true;
            this.linkLabel_GitHubRepo.Text = "https://github.com/TheSevenPens/WinTabPainter";
            this.linkLabel_GitHubRepo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "GitHub:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(940, 352);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(207, 70);
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel_GitHubRepo);
            this.Name = "FormAbout";
            this.Text = "FormAbout";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabel_GitHubRepo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}