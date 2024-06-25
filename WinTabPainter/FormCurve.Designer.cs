namespace WinTabPainter
{
    partial class FormCurve
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
            this.button_Close = new System.Windows.Forms.Button();
            this.pictureBox_Curve = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Curve)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(734, 547);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(148, 75);
            this.button_Close.TabIndex = 0;
            this.button_Close.Text = "close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // pictureBox_Curve
            // 
            this.pictureBox_Curve.Location = new System.Drawing.Point(42, 44);
            this.pictureBox_Curve.Name = "pictureBox_Curve";
            this.pictureBox_Curve.Size = new System.Drawing.Size(500, 500);
            this.pictureBox_Curve.TabIndex = 1;
            this.pictureBox_Curve.TabStop = false;
            // 
            // FormCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 634);
            this.Controls.Add(this.pictureBox_Curve);
            this.Controls.Add(this.button_Close);
            this.Name = "FormCurve";
            this.Text = "Curve Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCurve_FormClosed);
            this.Load += new System.EventHandler(this.FormCurve_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Curve)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.PictureBox pictureBox_Curve;
    }
}