namespace WinTabPainter
{
    partial class ScaledSlider
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            trackBar_Raw = new System.Windows.Forms.TrackBar();
            textBoxScaleValue = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)trackBar_Raw).BeginInit();
            SuspendLayout();
            // 
            // trackBar_Raw
            // 
            trackBar_Raw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            trackBar_Raw.AutoSize = false;
            trackBar_Raw.Location = new System.Drawing.Point(16, 16);
            trackBar_Raw.Maximum = 100;
            trackBar_Raw.Name = "trackBar_Raw";
            trackBar_Raw.Size = new System.Drawing.Size(531, 37);
            trackBar_Raw.TabIndex = 0;
            trackBar_Raw.TickStyle = System.Windows.Forms.TickStyle.None;
            trackBar_Raw.Scroll += trackBar1_Scroll;
            // 
            // textBoxScaleValue
            // 
            textBoxScaleValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxScaleValue.Location = new System.Drawing.Point(553, 11);
            textBoxScaleValue.Name = "textBoxScaleValue";
            textBoxScaleValue.Size = new System.Drawing.Size(140, 47);
            textBoxScaleValue.TabIndex = 2;
            textBoxScaleValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            textBoxScaleValue.KeyDown += textBoxScaleValue_KeyDown;
            // 
            // ScaledSlider
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(textBoxScaleValue);
            Controls.Add(trackBar_Raw);
            Name = "ScaledSlider";
            Size = new System.Drawing.Size(701, 69);
            ((System.ComponentModel.ISupportInitialize)trackBar_Raw).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar_Raw;
        private System.Windows.Forms.TextBox textBoxScaleValue;
    }
}
