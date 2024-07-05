namespace WinTabPainter
{
    partial class FormTablet
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
            label1 = new System.Windows.Forms.Label();
            label_tabletname_val = new System.Windows.Forms.Label();
            button_Close = new System.Windows.Forms.Button();
            textBox1 = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(43, 30);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(97, 41);
            label1.TabIndex = 0;
            label1.Text = "Tablet";
            // 
            // label_tabletname_val
            // 
            label_tabletname_val.AutoSize = true;
            label_tabletname_val.Location = new System.Drawing.Point(177, 30);
            label_tabletname_val.Name = "label_tabletname_val";
            label_tabletname_val.Size = new System.Drawing.Size(196, 41);
            label_tabletname_val.TabIndex = 1;
            label_tabletname_val.Text = "TABLETNAME";
            // 
            // button_Close
            // 
            button_Close.Location = new System.Drawing.Point(950, 668);
            button_Close.Name = "button_Close";
            button_Close.Size = new System.Drawing.Size(188, 58);
            button_Close.TabIndex = 2;
            button_Close.Text = "Close";
            button_Close.UseVisualStyleBackColor = true;
            button_Close.Click += button_Close_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(57, 97);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(1085, 539);
            textBox1.TabIndex = 3;
            // 
            // FormTablet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1183, 753);
            Controls.Add(textBox1);
            Controls.Add(button_Close);
            Controls.Add(label_tabletname_val);
            Controls.Add(label1);
            Name = "FormTablet";
            Text = "FormTablet";
            Load += FormTablet_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_tabletname_val;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.TextBox textBox1;
    }
}