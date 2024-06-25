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
            this.button_Cancel = new System.Windows.Forms.Button();
            this.pictureBox_Curve = new System.Windows.Forms.PictureBox();
            this.trackBar_Amount = new System.Windows.Forms.TrackBar();
            this.labelAmount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Curve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Amount)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(409, 737);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(148, 75);
            this.button_Cancel.TabIndex = 0;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // pictureBox_Curve
            // 
            this.pictureBox_Curve.Location = new System.Drawing.Point(42, 32);
            this.pictureBox_Curve.Name = "pictureBox_Curve";
            this.pictureBox_Curve.Size = new System.Drawing.Size(500, 500);
            this.pictureBox_Curve.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Curve.TabIndex = 1;
            this.pictureBox_Curve.TabStop = false;
            // 
            // trackBar_Amount
            // 
            this.trackBar_Amount.Location = new System.Drawing.Point(42, 597);
            this.trackBar_Amount.Maximum = 100;
            this.trackBar_Amount.Minimum = -100;
            this.trackBar_Amount.Name = "trackBar_Amount";
            this.trackBar_Amount.Size = new System.Drawing.Size(500, 114);
            this.trackBar_Amount.TabIndex = 2;
            this.trackBar_Amount.Value = 100;
            this.trackBar_Amount.Scroll += new System.EventHandler(this.trackBar_Amount_Scroll);
            // 
            // labelAmount
            // 
            this.labelAmount.AutoSize = true;
            this.labelAmount.Location = new System.Drawing.Point(244, 684);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(74, 32);
            this.labelAmount.TabIndex = 3;
            this.labelAmount.Text = "NNN";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(450, 552);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "Softer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 552);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "Harder";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 684);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 32);
            this.label3.TabIndex = 6;
            this.label3.Text = "Softness:";
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(230, 737);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(148, 75);
            this.button_OK.TabIndex = 7;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // FormCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 851);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelAmount);
            this.Controls.Add(this.trackBar_Amount);
            this.Controls.Add(this.pictureBox_Curve);
            this.Controls.Add(this.button_Cancel);
            this.Name = "FormCurve";
            this.Text = "Curve Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCurve_FormClosed);
            this.Load += new System.EventHandler(this.FormCurve_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Curve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Amount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.PictureBox pictureBox_Curve;
        private System.Windows.Forms.TrackBar trackBar_Amount;
        private System.Windows.Forms.Label labelAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_OK;
    }
}