namespace WinTabPainter
{
    partial class FormWinTabPainterApp
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
            this.label_ScreenPos = new System.Windows.Forms.Label();
            this.label_Pressure = new System.Windows.Forms.Label();
            this.label_Tilt = new System.Windows.Forms.Label();
            this.label_Buttons = new System.Windows.Forms.Label();
            this.label_Device = new System.Windows.Forms.Label();
            this.pictureBox_Canvas = new System.Windows.Forms.PictureBox();
            this.label_TiltValue = new System.Windows.Forms.Label();
            this.label_ScreenPosValue = new System.Windows.Forms.Label();
            this.label_PressureValue = new System.Windows.Forms.Label();
            this.label_ButtonsValue = new System.Windows.Forms.Label();
            this.label_DeviceValue = new System.Windows.Forms.Label();
            this.trackBar_BrushSize = new System.Windows.Forms.TrackBar();
            this.label_BrushSizeValue = new System.Windows.Forms.Label();
            this.label_BrushSize = new System.Windows.Forms.Label();
            this.trackBarPressureCurve = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label_PressureAdjusted = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuItem_File = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_FileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showShortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar_PositionSmoothing = new System.Windows.Forms.TrackBar();
            this.label_CanvasPos = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBar_PressureSmoothing = new System.Windows.Forms.TrackBar();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pressureCurveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Canvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_BrushSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPressureCurve)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_PositionSmoothing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_PressureSmoothing)).BeginInit();
            this.SuspendLayout();
            // 
            // label_ScreenPos
            // 
            this.label_ScreenPos.AutoSize = true;
            this.label_ScreenPos.Location = new System.Drawing.Point(23, 110);
            this.label_ScreenPos.Name = "label_ScreenPos";
            this.label_ScreenPos.Size = new System.Drawing.Size(153, 32);
            this.label_ScreenPos.TabIndex = 8;
            this.label_ScreenPos.Text = "ScreenPos";
            // 
            // label_Pressure
            // 
            this.label_Pressure.AutoSize = true;
            this.label_Pressure.Location = new System.Drawing.Point(23, 210);
            this.label_Pressure.Name = "label_Pressure";
            this.label_Pressure.Size = new System.Drawing.Size(172, 32);
            this.label_Pressure.TabIndex = 11;
            this.label_Pressure.Text = "Pressure (N)";
            // 
            // label_Tilt
            // 
            this.label_Tilt.AutoSize = true;
            this.label_Tilt.Location = new System.Drawing.Point(23, 310);
            this.label_Tilt.Name = "label_Tilt";
            this.label_Tilt.Size = new System.Drawing.Size(53, 32);
            this.label_Tilt.TabIndex = 13;
            this.label_Tilt.Text = "Tilt";
            // 
            // label_Buttons
            // 
            this.label_Buttons.AutoSize = true;
            this.label_Buttons.Location = new System.Drawing.Point(23, 360);
            this.label_Buttons.Name = "label_Buttons";
            this.label_Buttons.Size = new System.Drawing.Size(111, 32);
            this.label_Buttons.TabIndex = 16;
            this.label_Buttons.Text = "Buttons";
            // 
            // label_Device
            // 
            this.label_Device.AutoSize = true;
            this.label_Device.Location = new System.Drawing.Point(23, 410);
            this.label_Device.Name = "label_Device";
            this.label_Device.Size = new System.Drawing.Size(101, 32);
            this.label_Device.TabIndex = 18;
            this.label_Device.Text = "Device";
            // 
            // pictureBox_Canvas
            // 
            this.pictureBox_Canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Canvas.Location = new System.Drawing.Point(513, 134);
            this.pictureBox_Canvas.Name = "pictureBox_Canvas";
            this.pictureBox_Canvas.Size = new System.Drawing.Size(1261, 756);
            this.pictureBox_Canvas.TabIndex = 21;
            this.pictureBox_Canvas.TabStop = false;
            // 
            // label_TiltValue
            // 
            this.label_TiltValue.AutoSize = true;
            this.label_TiltValue.Location = new System.Drawing.Point(199, 310);
            this.label_TiltValue.Name = "label_TiltValue";
            this.label_TiltValue.Size = new System.Drawing.Size(41, 32);
            this.label_TiltValue.TabIndex = 26;
            this.label_TiltValue.Text = "---";
            // 
            // label_ScreenPosValue
            // 
            this.label_ScreenPosValue.AutoSize = true;
            this.label_ScreenPosValue.Location = new System.Drawing.Point(199, 110);
            this.label_ScreenPosValue.Name = "label_ScreenPosValue";
            this.label_ScreenPosValue.Size = new System.Drawing.Size(41, 32);
            this.label_ScreenPosValue.TabIndex = 22;
            this.label_ScreenPosValue.Text = "---";
            // 
            // label_PressureValue
            // 
            this.label_PressureValue.AutoSize = true;
            this.label_PressureValue.Location = new System.Drawing.Point(199, 210);
            this.label_PressureValue.Name = "label_PressureValue";
            this.label_PressureValue.Size = new System.Drawing.Size(41, 32);
            this.label_PressureValue.TabIndex = 25;
            this.label_PressureValue.Text = "---";
            // 
            // label_ButtonsValue
            // 
            this.label_ButtonsValue.AutoSize = true;
            this.label_ButtonsValue.Location = new System.Drawing.Point(199, 360);
            this.label_ButtonsValue.Name = "label_ButtonsValue";
            this.label_ButtonsValue.Size = new System.Drawing.Size(41, 32);
            this.label_ButtonsValue.TabIndex = 28;
            this.label_ButtonsValue.Text = "---";
            // 
            // label_DeviceValue
            // 
            this.label_DeviceValue.AutoSize = true;
            this.label_DeviceValue.Location = new System.Drawing.Point(199, 410);
            this.label_DeviceValue.Name = "label_DeviceValue";
            this.label_DeviceValue.Size = new System.Drawing.Size(41, 32);
            this.label_DeviceValue.TabIndex = 29;
            this.label_DeviceValue.Text = "---";
            // 
            // trackBar_BrushSize
            // 
            this.trackBar_BrushSize.AutoSize = false;
            this.trackBar_BrushSize.Location = new System.Drawing.Point(658, 83);
            this.trackBar_BrushSize.Maximum = 100;
            this.trackBar_BrushSize.Minimum = 1;
            this.trackBar_BrushSize.Name = "trackBar_BrushSize";
            this.trackBar_BrushSize.Size = new System.Drawing.Size(424, 45);
            this.trackBar_BrushSize.TabIndex = 32;
            this.trackBar_BrushSize.Value = 1;
            this.trackBar_BrushSize.Scroll += new System.EventHandler(this.trackBar_BrushSize_Scroll);
            // 
            // label_BrushSizeValue
            // 
            this.label_BrushSizeValue.AutoSize = true;
            this.label_BrushSizeValue.Location = new System.Drawing.Point(1104, 83);
            this.label_BrushSizeValue.Name = "label_BrushSizeValue";
            this.label_BrushSizeValue.Size = new System.Drawing.Size(41, 32);
            this.label_BrushSizeValue.TabIndex = 33;
            this.label_BrushSizeValue.Text = "---";
            // 
            // label_BrushSize
            // 
            this.label_BrushSize.AutoSize = true;
            this.label_BrushSize.Location = new System.Drawing.Point(506, 83);
            this.label_BrushSize.Name = "label_BrushSize";
            this.label_BrushSize.Size = new System.Drawing.Size(146, 32);
            this.label_BrushSize.TabIndex = 34;
            this.label_BrushSize.Text = "Brush size";
            // 
            // trackBarPressureCurve
            // 
            this.trackBarPressureCurve.AutoSize = false;
            this.trackBarPressureCurve.Location = new System.Drawing.Point(12, 528);
            this.trackBarPressureCurve.Maximum = 100;
            this.trackBarPressureCurve.Minimum = -100;
            this.trackBarPressureCurve.Name = "trackBarPressureCurve";
            this.trackBarPressureCurve.Size = new System.Drawing.Size(424, 45);
            this.trackBarPressureCurve.TabIndex = 35;
            this.trackBarPressureCurve.Value = 1;
            this.trackBarPressureCurve.Scroll += new System.EventHandler(this.trackBarPressureCurve_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 477);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(387, 32);
            this.label1.TabIndex = 36;
            this.label1.Text = "Pressure Curve (Hard<->Soft)";
            // 
            // label_PressureAdjusted
            // 
            this.label_PressureAdjusted.AutoSize = true;
            this.label_PressureAdjusted.Location = new System.Drawing.Point(199, 260);
            this.label_PressureAdjusted.Name = "label_PressureAdjusted";
            this.label_PressureAdjusted.Size = new System.Drawing.Size(41, 32);
            this.label_PressureAdjusted.TabIndex = 38;
            this.label_PressureAdjusted.Text = "---";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 260);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 32);
            this.label3.TabIndex = 37;
            this.label3.Text = "Pressure (A)";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_File,
            this.imageToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1786, 52);
            this.menuStrip1.TabIndex = 40;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuItem_File
            // 
            this.MenuItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Open,
            this.MenuItem_FileSave,
            this.MenuItem_SaveAs});
            this.MenuItem_File.Name = "MenuItem_File";
            this.MenuItem_File.Size = new System.Drawing.Size(87, 48);
            this.MenuItem_File.Text = "File";
            // 
            // MenuItem_Open
            // 
            this.MenuItem_Open.Name = "MenuItem_Open";
            this.MenuItem_Open.Size = new System.Drawing.Size(285, 54);
            this.MenuItem_Open.Text = "Open";
            this.MenuItem_Open.Click += new System.EventHandler(this.MenuItem_Open_Click);
            // 
            // MenuItem_FileSave
            // 
            this.MenuItem_FileSave.Name = "MenuItem_FileSave";
            this.MenuItem_FileSave.Size = new System.Drawing.Size(285, 54);
            this.MenuItem_FileSave.Text = "Save";
            this.MenuItem_FileSave.Click += new System.EventHandler(this.MenuFileSave_Click);
            // 
            // MenuItem_SaveAs
            // 
            this.MenuItem_SaveAs.Name = "MenuItem_SaveAs";
            this.MenuItem_SaveAs.Size = new System.Drawing.Size(285, 54);
            this.MenuItem_SaveAs.Text = "Save As";
            this.MenuItem_SaveAs.Click += new System.EventHandler(this.MenuItem_SaveAs_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(125, 48);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(251, 54);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showShortcutsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(104, 48);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // showShortcutsToolStripMenuItem
            // 
            this.showShortcutsToolStripMenuItem.Name = "showShortcutsToolStripMenuItem";
            this.showShortcutsToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.showShortcutsToolStripMenuItem.Text = "Show Shortcuts";
            this.showShortcutsToolStripMenuItem.Click += new System.EventHandler(this.showShortcutsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 598);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(448, 32);
            this.label2.TabIndex = 42;
            this.label2.Text = "Position Smoothing (None<->Max)";
            // 
            // trackBar_PositionSmoothing
            // 
            this.trackBar_PositionSmoothing.AutoSize = false;
            this.trackBar_PositionSmoothing.Location = new System.Drawing.Point(12, 649);
            this.trackBar_PositionSmoothing.Maximum = 100;
            this.trackBar_PositionSmoothing.Name = "trackBar_PositionSmoothing";
            this.trackBar_PositionSmoothing.Size = new System.Drawing.Size(424, 45);
            this.trackBar_PositionSmoothing.TabIndex = 41;
            this.trackBar_PositionSmoothing.Scroll += new System.EventHandler(this.trackBar_PositionSmoothing_Scroll);
            // 
            // label_CanvasPos
            // 
            this.label_CanvasPos.AutoSize = true;
            this.label_CanvasPos.Location = new System.Drawing.Point(199, 160);
            this.label_CanvasPos.Name = "label_CanvasPos";
            this.label_CanvasPos.Size = new System.Drawing.Size(41, 32);
            this.label_CanvasPos.TabIndex = 44;
            this.label_CanvasPos.Text = "---";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 32);
            this.label5.TabIndex = 43;
            this.label5.Text = "CanvasPos";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 728);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(458, 32);
            this.label4.TabIndex = 46;
            this.label4.Text = "Pressure Smoothing (None<->Max)";
            // 
            // trackBar_PressureSmoothing
            // 
            this.trackBar_PressureSmoothing.AutoSize = false;
            this.trackBar_PressureSmoothing.Location = new System.Drawing.Point(12, 779);
            this.trackBar_PressureSmoothing.Maximum = 100;
            this.trackBar_PressureSmoothing.Name = "trackBar_PressureSmoothing";
            this.trackBar_PressureSmoothing.Size = new System.Drawing.Size(424, 45);
            this.trackBar_PressureSmoothing.TabIndex = 45;
            this.trackBar_PressureSmoothing.Scroll += new System.EventHandler(this.trackBar_PressureSmoothing_Scroll);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pressureCurveToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(149, 48);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // pressureCurveToolStripMenuItem
            // 
            this.pressureCurveToolStripMenuItem.Name = "pressureCurveToolStripMenuItem";
            this.pressureCurveToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.pressureCurveToolStripMenuItem.Text = "Pressure Curve";
            this.pressureCurveToolStripMenuItem.Click += new System.EventHandler(this.pressureCurveToolStripMenuItem_Click);
            // 
            // FormWinTabPainterApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1786, 945);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBar_PressureSmoothing);
            this.Controls.Add(this.label_CanvasPos);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBar_PositionSmoothing);
            this.Controls.Add(this.label_PressureAdjusted);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarPressureCurve);
            this.Controls.Add(this.label_BrushSize);
            this.Controls.Add(this.label_BrushSizeValue);
            this.Controls.Add(this.trackBar_BrushSize);
            this.Controls.Add(this.label_DeviceValue);
            this.Controls.Add(this.label_TiltValue);
            this.Controls.Add(this.label_ScreenPosValue);
            this.Controls.Add(this.label_PressureValue);
            this.Controls.Add(this.label_ButtonsValue);
            this.Controls.Add(this.pictureBox_Canvas);
            this.Controls.Add(this.label_Tilt);
            this.Controls.Add(this.label_ScreenPos);
            this.Controls.Add(this.label_Pressure);
            this.Controls.Add(this.label_Device);
            this.Controls.Add(this.label_Buttons);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 1000);
            this.Name = "FormWinTabPainterApp";
            this.Text = "WinTab Painter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Canvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_BrushSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPressureCurve)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_PositionSmoothing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_PressureSmoothing)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_ScreenPos;
        private System.Windows.Forms.Label label_Pressure;
        private System.Windows.Forms.Label label_Tilt;
        private System.Windows.Forms.Label label_Buttons;
        private System.Windows.Forms.Label label_Device;
        private System.Windows.Forms.PictureBox pictureBox_Canvas;
        private System.Windows.Forms.Label label_TiltValue;
        private System.Windows.Forms.Label label_ScreenPosValue;
        private System.Windows.Forms.Label label_PressureValue;
        private System.Windows.Forms.Label label_ButtonsValue;
        private System.Windows.Forms.Label label_DeviceValue;
        private System.Windows.Forms.TrackBar trackBar_BrushSize;
        private System.Windows.Forms.Label label_BrushSizeValue;
        private System.Windows.Forms.Label label_BrushSize;
        private System.Windows.Forms.TrackBar trackBarPressureCurve;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_PressureAdjusted;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_File;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_FileSave;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Open;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar_PositionSmoothing;
        private System.Windows.Forms.Label label_CanvasPos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trackBar_PressureSmoothing;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showShortcutsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pressureCurveToolStripMenuItem;
    }
}

