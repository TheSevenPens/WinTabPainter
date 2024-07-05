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
            label_ScreenPos = new System.Windows.Forms.Label();
            label_PressureNormalized = new System.Windows.Forms.Label();
            label_Tilt = new System.Windows.Forms.Label();
            label_Buttons = new System.Windows.Forms.Label();
            label_Device = new System.Windows.Forms.Label();
            pictureBox_Canvas = new System.Windows.Forms.PictureBox();
            label_TiltValue = new System.Windows.Forms.Label();
            label_ScreenPosValue = new System.Windows.Forms.Label();
            label_PressureValue = new System.Windows.Forms.Label();
            label_ButtonsValue = new System.Windows.Forms.Label();
            label_DeviceValue = new System.Windows.Forms.Label();
            trackBar_BrushSize = new System.Windows.Forms.TrackBar();
            label_BrushSizeValue = new System.Windows.Forms.Label();
            label_BrushSize = new System.Windows.Forms.Label();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            MenuItem_File = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_FileSave = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pressureCurveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showShortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            label2 = new System.Windows.Forms.Label();
            trackBar_PositionSmoothing = new System.Windows.Forms.TrackBar();
            label_CanvasPos = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            trackBar_PressureSmoothing = new System.Windows.Forms.TrackBar();
            label_pressuresmoothingval = new System.Windows.Forms.Label();
            label_position_smoothingval = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Canvas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_BrushSize).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_PositionSmoothing).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_PressureSmoothing).BeginInit();
            SuspendLayout();
            // 
            // label_ScreenPos
            // 
            label_ScreenPos.AutoSize = true;
            label_ScreenPos.Location = new System.Drawing.Point(24, 145);
            label_ScreenPos.Name = "label_ScreenPos";
            label_ScreenPos.Size = new System.Drawing.Size(154, 41);
            label_ScreenPos.TabIndex = 8;
            label_ScreenPos.Text = "ScreenPos";
            // 
            // label_PressureNormalized
            // 
            label_PressureNormalized.AutoSize = true;
            label_PressureNormalized.Location = new System.Drawing.Point(24, 278);
            label_PressureNormalized.Name = "label_PressureNormalized";
            label_PressureNormalized.Size = new System.Drawing.Size(130, 41);
            label_PressureNormalized.TabIndex = 11;
            label_PressureNormalized.Text = "Pressure";
            // 
            // label_Tilt
            // 
            label_Tilt.AutoSize = true;
            label_Tilt.Location = new System.Drawing.Point(24, 410);
            label_Tilt.Name = "label_Tilt";
            label_Tilt.Size = new System.Drawing.Size(58, 41);
            label_Tilt.TabIndex = 13;
            label_Tilt.Text = "Tilt";
            // 
            // label_Buttons
            // 
            label_Buttons.AutoSize = true;
            label_Buttons.Location = new System.Drawing.Point(24, 476);
            label_Buttons.Name = "label_Buttons";
            label_Buttons.Size = new System.Drawing.Size(120, 41);
            label_Buttons.TabIndex = 16;
            label_Buttons.Text = "Buttons";
            // 
            // label_Device
            // 
            label_Device.AutoSize = true;
            label_Device.Location = new System.Drawing.Point(24, 542);
            label_Device.Name = "label_Device";
            label_Device.Size = new System.Drawing.Size(106, 41);
            label_Device.TabIndex = 18;
            label_Device.Text = "Device";
            // 
            // pictureBox_Canvas
            // 
            pictureBox_Canvas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pictureBox_Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pictureBox_Canvas.Location = new System.Drawing.Point(545, 177);
            pictureBox_Canvas.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pictureBox_Canvas.Name = "pictureBox_Canvas";
            pictureBox_Canvas.Size = new System.Drawing.Size(1340, 999);
            pictureBox_Canvas.TabIndex = 21;
            pictureBox_Canvas.TabStop = false;
            // 
            // label_TiltValue
            // 
            label_TiltValue.AutoSize = true;
            label_TiltValue.Location = new System.Drawing.Point(211, 410);
            label_TiltValue.Name = "label_TiltValue";
            label_TiltValue.Size = new System.Drawing.Size(54, 41);
            label_TiltValue.TabIndex = 26;
            label_TiltValue.Text = "---";
            // 
            // label_ScreenPosValue
            // 
            label_ScreenPosValue.AutoSize = true;
            label_ScreenPosValue.Location = new System.Drawing.Point(211, 145);
            label_ScreenPosValue.Name = "label_ScreenPosValue";
            label_ScreenPosValue.Size = new System.Drawing.Size(54, 41);
            label_ScreenPosValue.TabIndex = 22;
            label_ScreenPosValue.Text = "---";
            // 
            // label_PressureValue
            // 
            label_PressureValue.AutoSize = true;
            label_PressureValue.Location = new System.Drawing.Point(211, 278);
            label_PressureValue.Name = "label_PressureValue";
            label_PressureValue.Size = new System.Drawing.Size(54, 41);
            label_PressureValue.TabIndex = 25;
            label_PressureValue.Text = "---";
            // 
            // label_ButtonsValue
            // 
            label_ButtonsValue.AutoSize = true;
            label_ButtonsValue.Location = new System.Drawing.Point(211, 476);
            label_ButtonsValue.Name = "label_ButtonsValue";
            label_ButtonsValue.Size = new System.Drawing.Size(54, 41);
            label_ButtonsValue.TabIndex = 28;
            label_ButtonsValue.Text = "---";
            // 
            // label_DeviceValue
            // 
            label_DeviceValue.AutoSize = true;
            label_DeviceValue.Location = new System.Drawing.Point(211, 542);
            label_DeviceValue.Name = "label_DeviceValue";
            label_DeviceValue.Size = new System.Drawing.Size(54, 41);
            label_DeviceValue.TabIndex = 29;
            label_DeviceValue.Text = "---";
            // 
            // trackBar_BrushSize
            // 
            trackBar_BrushSize.AutoSize = false;
            trackBar_BrushSize.Location = new System.Drawing.Point(699, 110);
            trackBar_BrushSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            trackBar_BrushSize.Maximum = 100;
            trackBar_BrushSize.Minimum = 1;
            trackBar_BrushSize.Name = "trackBar_BrushSize";
            trackBar_BrushSize.Size = new System.Drawing.Size(450, 60);
            trackBar_BrushSize.TabIndex = 32;
            trackBar_BrushSize.TickStyle = System.Windows.Forms.TickStyle.None;
            trackBar_BrushSize.Value = 1;
            trackBar_BrushSize.Scroll += trackBar_BrushSize_Scroll;
            // 
            // label_BrushSizeValue
            // 
            label_BrushSizeValue.AutoSize = true;
            label_BrushSizeValue.Location = new System.Drawing.Point(1173, 110);
            label_BrushSizeValue.Name = "label_BrushSizeValue";
            label_BrushSizeValue.Size = new System.Drawing.Size(54, 41);
            label_BrushSizeValue.TabIndex = 33;
            label_BrushSizeValue.Text = "---";
            // 
            // label_BrushSize
            // 
            label_BrushSize.AutoSize = true;
            label_BrushSize.Location = new System.Drawing.Point(538, 110);
            label_BrushSize.Name = "label_BrushSize";
            label_BrushSize.Size = new System.Drawing.Size(150, 41);
            label_BrushSize.TabIndex = 34;
            label_BrushSize.Text = "Brush size";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { MenuItem_File, imageToolStripMenuItem, settingsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            menuStrip1.Size = new System.Drawing.Size(1898, 51);
            menuStrip1.TabIndex = 40;
            menuStrip1.Text = "menuStrip1";
            // 
            // MenuItem_File
            // 
            MenuItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MenuItem_Open, MenuItem_FileSave, MenuItem_SaveAs });
            MenuItem_File.Name = "MenuItem_File";
            MenuItem_File.Size = new System.Drawing.Size(87, 45);
            MenuItem_File.Text = "File";
            // 
            // MenuItem_Open
            // 
            MenuItem_Open.Name = "MenuItem_Open";
            MenuItem_Open.Size = new System.Drawing.Size(285, 54);
            MenuItem_Open.Text = "Open";
            MenuItem_Open.Click += MenuItem_Open_Click;
            // 
            // MenuItem_FileSave
            // 
            MenuItem_FileSave.Name = "MenuItem_FileSave";
            MenuItem_FileSave.Size = new System.Drawing.Size(285, 54);
            MenuItem_FileSave.Text = "Save";
            MenuItem_FileSave.Click += MenuFileSave_Click;
            // 
            // MenuItem_SaveAs
            // 
            MenuItem_SaveAs.Name = "MenuItem_SaveAs";
            MenuItem_SaveAs.Size = new System.Drawing.Size(285, 54);
            MenuItem_SaveAs.Text = "Save As";
            MenuItem_SaveAs.Click += MenuItem_SaveAs_Click;
            // 
            // imageToolStripMenuItem
            // 
            imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { clearToolStripMenuItem });
            imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            imageToolStripMenuItem.Size = new System.Drawing.Size(125, 45);
            imageToolStripMenuItem.Text = "Image";
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new System.Drawing.Size(251, 54);
            clearToolStripMenuItem.Text = "Clear";
            clearToolStripMenuItem.Click += clearToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { pressureCurveToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new System.Drawing.Size(149, 45);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // pressureCurveToolStripMenuItem
            // 
            pressureCurveToolStripMenuItem.Name = "pressureCurveToolStripMenuItem";
            pressureCurveToolStripMenuItem.Size = new System.Drawing.Size(380, 54);
            pressureCurveToolStripMenuItem.Text = "Pressure Curve";
            pressureCurveToolStripMenuItem.Click += pressureCurveToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { showShortcutsToolStripMenuItem, aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(104, 45);
            helpToolStripMenuItem.Text = "Help";
            // 
            // showShortcutsToolStripMenuItem
            // 
            showShortcutsToolStripMenuItem.Name = "showShortcutsToolStripMenuItem";
            showShortcutsToolStripMenuItem.Size = new System.Drawing.Size(390, 54);
            showShortcutsToolStripMenuItem.Text = "Show Shortcuts";
            showShortcutsToolStripMenuItem.Click += showShortcutsToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(390, 54);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(24, 791);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(279, 41);
            label2.TabIndex = 42;
            label2.Text = "Position Smoothing";
            // 
            // trackBar_PositionSmoothing
            // 
            trackBar_PositionSmoothing.AutoSize = false;
            trackBar_PositionSmoothing.Location = new System.Drawing.Point(13, 858);
            trackBar_PositionSmoothing.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            trackBar_PositionSmoothing.Maximum = 100;
            trackBar_PositionSmoothing.Name = "trackBar_PositionSmoothing";
            trackBar_PositionSmoothing.Size = new System.Drawing.Size(450, 60);
            trackBar_PositionSmoothing.TabIndex = 41;
            trackBar_PositionSmoothing.TickStyle = System.Windows.Forms.TickStyle.None;
            trackBar_PositionSmoothing.Scroll += trackBar_PositionSmoothing_Scroll;
            // 
            // label_CanvasPos
            // 
            label_CanvasPos.AutoSize = true;
            label_CanvasPos.Location = new System.Drawing.Point(211, 212);
            label_CanvasPos.Name = "label_CanvasPos";
            label_CanvasPos.Size = new System.Drawing.Size(54, 41);
            label_CanvasPos.TabIndex = 44;
            label_CanvasPos.Text = "---";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(24, 212);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(158, 41);
            label5.TabIndex = 43;
            label5.Text = "CanvasPos";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(24, 963);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(285, 41);
            label4.TabIndex = 46;
            label4.Text = "Pressure Smoothing";
            // 
            // trackBar_PressureSmoothing
            // 
            trackBar_PressureSmoothing.AutoSize = false;
            trackBar_PressureSmoothing.Location = new System.Drawing.Point(13, 1030);
            trackBar_PressureSmoothing.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            trackBar_PressureSmoothing.Maximum = 100;
            trackBar_PressureSmoothing.Name = "trackBar_PressureSmoothing";
            trackBar_PressureSmoothing.Size = new System.Drawing.Size(450, 60);
            trackBar_PressureSmoothing.TabIndex = 45;
            trackBar_PressureSmoothing.TickStyle = System.Windows.Forms.TickStyle.None;
            trackBar_PressureSmoothing.Scroll += trackBar_PressureSmoothing_Scroll;
            // 
            // label_pressuresmoothingval
            // 
            label_pressuresmoothingval.AutoSize = true;
            label_pressuresmoothingval.Location = new System.Drawing.Point(351, 963);
            label_pressuresmoothingval.Name = "label_pressuresmoothingval";
            label_pressuresmoothingval.Size = new System.Drawing.Size(84, 41);
            label_pressuresmoothingval.TabIndex = 47;
            label_pressuresmoothingval.Text = "NNN";
            // 
            // label_position_smoothingval
            // 
            label_position_smoothingval.AutoSize = true;
            label_position_smoothingval.Location = new System.Drawing.Point(351, 791);
            label_position_smoothingval.Name = "label_position_smoothingval";
            label_position_smoothingval.Size = new System.Drawing.Size(84, 41);
            label_position_smoothingval.TabIndex = 48;
            label_position_smoothingval.Text = "NNN";
            // 
            // FormWinTabPainterApp
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1898, 1250);
            Controls.Add(label_position_smoothingval);
            Controls.Add(label_pressuresmoothingval);
            Controls.Add(label4);
            Controls.Add(trackBar_PressureSmoothing);
            Controls.Add(label_CanvasPos);
            Controls.Add(label5);
            Controls.Add(label2);
            Controls.Add(trackBar_PositionSmoothing);
            Controls.Add(label_BrushSize);
            Controls.Add(label_BrushSizeValue);
            Controls.Add(trackBar_BrushSize);
            Controls.Add(label_DeviceValue);
            Controls.Add(label_TiltValue);
            Controls.Add(label_ScreenPosValue);
            Controls.Add(label_PressureValue);
            Controls.Add(label_ButtonsValue);
            Controls.Add(pictureBox_Canvas);
            Controls.Add(label_Tilt);
            Controls.Add(label_ScreenPos);
            Controls.Add(label_PressureNormalized);
            Controls.Add(label_Device);
            Controls.Add(label_Buttons);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MinimumSize = new System.Drawing.Size(1060, 1294);
            Name = "FormWinTabPainterApp";
            Text = "WinTab Painter";
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox_Canvas).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_BrushSize).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_PositionSmoothing).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_PressureSmoothing).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label_ScreenPos;
        private System.Windows.Forms.Label label_PressureNormalized;
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
        private System.Windows.Forms.Label label_pressuresmoothingval;
        private System.Windows.Forms.Label label_position_smoothingval;
    }
}

