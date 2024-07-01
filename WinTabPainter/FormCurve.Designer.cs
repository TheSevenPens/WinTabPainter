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
            button_Cancel = new System.Windows.Forms.Button();
            pictureBox_Curve = new System.Windows.Forms.PictureBox();
            trackBar_Amount = new System.Windows.Forms.TrackBar();
            labelAmount = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            button_OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Curve).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_Amount).BeginInit();
            SuspendLayout();
            // 
            // button_Cancel
            // 
            button_Cancel.Location = new System.Drawing.Point(440, 874);
            button_Cancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button_Cancel.Name = "button_Cancel";
            button_Cancel.Size = new System.Drawing.Size(157, 69);
            button_Cancel.TabIndex = 0;
            button_Cancel.Text = "Cancel";
            button_Cancel.UseVisualStyleBackColor = true;
            button_Cancel.Click += button_Close_Click;
            // 
            // pictureBox_Curve
            // 
            pictureBox_Curve.Location = new System.Drawing.Point(26, 36);
            pictureBox_Curve.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pictureBox_Curve.Name = "pictureBox_Curve";
            pictureBox_Curve.Size = new System.Drawing.Size(550, 550);
            pictureBox_Curve.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox_Curve.TabIndex = 1;
            pictureBox_Curve.TabStop = false;
            // 
            // trackBar_Amount
            // 
            trackBar_Amount.Location = new System.Drawing.Point(41, 677);
            trackBar_Amount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            trackBar_Amount.Maximum = 100;
            trackBar_Amount.Minimum = -100;
            trackBar_Amount.Name = "trackBar_Amount";
            trackBar_Amount.Size = new System.Drawing.Size(531, 114);
            trackBar_Amount.TabIndex = 2;
            trackBar_Amount.TickStyle = System.Windows.Forms.TickStyle.None;
            trackBar_Amount.Value = 100;
            trackBar_Amount.Scroll += trackBar_Amount_Scroll;
            // 
            // labelAmount
            // 
            labelAmount.AutoSize = true;
            labelAmount.Location = new System.Drawing.Point(259, 803);
            labelAmount.Name = "labelAmount";
            labelAmount.Size = new System.Drawing.Size(84, 41);
            labelAmount.TabIndex = 3;
            labelAmount.Text = "NNN";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(474, 617);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(97, 41);
            label1.TabIndex = 4;
            label1.Text = "Softer";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(46, 617);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(108, 41);
            label2.TabIndex = 5;
            label2.Text = "Harder";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(50, 803);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(137, 41);
            label3.TabIndex = 6;
            label3.Text = "Softness:";
            // 
            // button_OK
            // 
            button_OK.Location = new System.Drawing.Point(249, 874);
            button_OK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button_OK.Name = "button_OK";
            button_OK.Size = new System.Drawing.Size(157, 69);
            button_OK.TabIndex = 7;
            button_OK.Text = "OK";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Click += button_OK_Click;
            // 
            // FormCurve
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(630, 965);
            Controls.Add(button_OK);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(labelAmount);
            Controls.Add(trackBar_Amount);
            Controls.Add(pictureBox_Curve);
            Controls.Add(button_Cancel);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "FormCurve";
            Text = "Curve Editor";
            FormClosed += FormCurve_FormClosed;
            Load += FormCurve_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox_Curve).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_Amount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TrackBar trackBar_Amount;
        private System.Windows.Forms.Label labelAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_OK;
        protected System.Windows.Forms.PictureBox pictureBox_Curve;
    }
}