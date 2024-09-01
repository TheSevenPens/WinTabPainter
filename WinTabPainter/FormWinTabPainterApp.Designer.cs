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
            pictureBox_Canvas = new System.Windows.Forms.PictureBox();
            label_TiltValue = new System.Windows.Forms.Label();
            label_ScreenPosValue = new System.Windows.Forms.Label();
            label_PressureValue = new System.Windows.Forms.Label();
            label_ButtonsValue = new System.Windows.Forms.Label();
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
            aboutTabletToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            label_CanvasPos = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            labeltilt_xy = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            button_Clear = new System.Windows.Forms.Button();
            buttonRec = new System.Windows.Forms.Button();
            label_RecCount = new System.Windows.Forms.Label();
            button_replay = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            buttonClearRecording = new System.Windows.Forms.Button();
            buttonSavePackets = new System.Windows.Forms.Button();
            buttonLoadPackets = new System.Windows.Forms.Button();
            buttonCopy = new System.Windows.Forms.Button();
            textBox_config = new System.Windows.Forms.TextBox();
            buttonSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Canvas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_BrushSize).BeginInit();
            menuStrip1.SuspendLayout();
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
            label_PressureNormalized.Location = new System.Drawing.Point(24, 259);
            label_PressureNormalized.Name = "label_PressureNormalized";
            label_PressureNormalized.Size = new System.Drawing.Size(130, 41);
            label_PressureNormalized.TabIndex = 11;
            label_PressureNormalized.Text = "Pressure";
            // 
            // label_Tilt
            // 
            label_Tilt.AutoSize = true;
            label_Tilt.Location = new System.Drawing.Point(24, 316);
            label_Tilt.Name = "label_Tilt";
            label_Tilt.Size = new System.Drawing.Size(167, 41);
            label_Tilt.TabIndex = 13;
            label_Tilt.Text = "Tilt (Alt,Azi)";
            // 
            // label_Buttons
            // 
            label_Buttons.AutoSize = true;
            label_Buttons.Location = new System.Drawing.Point(24, 430);
            label_Buttons.Name = "label_Buttons";
            label_Buttons.Size = new System.Drawing.Size(120, 41);
            label_Buttons.TabIndex = 16;
            label_Buttons.Text = "Buttons";
            // 
            // pictureBox_Canvas
            // 
            pictureBox_Canvas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pictureBox_Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pictureBox_Canvas.Location = new System.Drawing.Point(618, 177);
            pictureBox_Canvas.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pictureBox_Canvas.Name = "pictureBox_Canvas";
            pictureBox_Canvas.Size = new System.Drawing.Size(1325, 999);
            pictureBox_Canvas.TabIndex = 21;
            pictureBox_Canvas.TabStop = false;
            // 
            // label_TiltValue
            // 
            label_TiltValue.AutoSize = true;
            label_TiltValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label_TiltValue.Location = new System.Drawing.Point(211, 316);
            label_TiltValue.Name = "label_TiltValue";
            label_TiltValue.Size = new System.Drawing.Size(63, 36);
            label_TiltValue.TabIndex = 26;
            label_TiltValue.Text = "---";
            // 
            // label_ScreenPosValue
            // 
            label_ScreenPosValue.AutoSize = true;
            label_ScreenPosValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label_ScreenPosValue.Location = new System.Drawing.Point(211, 145);
            label_ScreenPosValue.Name = "label_ScreenPosValue";
            label_ScreenPosValue.Size = new System.Drawing.Size(63, 36);
            label_ScreenPosValue.TabIndex = 22;
            label_ScreenPosValue.Text = "---";
            // 
            // label_PressureValue
            // 
            label_PressureValue.AutoSize = true;
            label_PressureValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label_PressureValue.Location = new System.Drawing.Point(211, 259);
            label_PressureValue.Name = "label_PressureValue";
            label_PressureValue.Size = new System.Drawing.Size(63, 36);
            label_PressureValue.TabIndex = 25;
            label_PressureValue.Text = "---";
            // 
            // label_ButtonsValue
            // 
            label_ButtonsValue.AutoSize = true;
            label_ButtonsValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label_ButtonsValue.Location = new System.Drawing.Point(211, 430);
            label_ButtonsValue.Name = "label_ButtonsValue";
            label_ButtonsValue.Size = new System.Drawing.Size(63, 36);
            label_ButtonsValue.TabIndex = 28;
            label_ButtonsValue.Text = "---";
            // 
            // trackBar_BrushSize
            // 
            trackBar_BrushSize.AutoSize = false;
            trackBar_BrushSize.Location = new System.Drawing.Point(777, 121);
            trackBar_BrushSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            trackBar_BrushSize.Maximum = 100;
            trackBar_BrushSize.Minimum = 1;
            trackBar_BrushSize.Name = "trackBar_BrushSize";
            trackBar_BrushSize.Size = new System.Drawing.Size(450, 36);
            trackBar_BrushSize.TabIndex = 32;
            trackBar_BrushSize.TickStyle = System.Windows.Forms.TickStyle.None;
            trackBar_BrushSize.Value = 1;
            trackBar_BrushSize.Scroll += trackBar_BrushSize_Scroll;
            // 
            // label_BrushSizeValue
            // 
            label_BrushSizeValue.AutoSize = true;
            label_BrushSizeValue.Location = new System.Drawing.Point(1173, 121);
            label_BrushSizeValue.Name = "label_BrushSizeValue";
            label_BrushSizeValue.Size = new System.Drawing.Size(54, 41);
            label_BrushSizeValue.TabIndex = 33;
            label_BrushSizeValue.Text = "---";
            // 
            // label_BrushSize
            // 
            label_BrushSize.AutoSize = true;
            label_BrushSize.Location = new System.Drawing.Point(616, 119);
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
            menuStrip1.Size = new System.Drawing.Size(1956, 51);
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
            pressureCurveToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            pressureCurveToolStripMenuItem.Text = "Brush settings";
            pressureCurveToolStripMenuItem.Click += pressureCurveToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { showShortcutsToolStripMenuItem, aboutToolStripMenuItem, aboutTabletToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(104, 45);
            helpToolStripMenuItem.Text = "Help";
            // 
            // showShortcutsToolStripMenuItem
            // 
            showShortcutsToolStripMenuItem.Name = "showShortcutsToolStripMenuItem";
            showShortcutsToolStripMenuItem.Size = new System.Drawing.Size(471, 54);
            showShortcutsToolStripMenuItem.Text = "Show Shortcuts";
            showShortcutsToolStripMenuItem.Click += showShortcutsToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(471, 54);
            aboutToolStripMenuItem.Text = "About WinTab Painter";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // aboutTabletToolStripMenuItem
            // 
            aboutTabletToolStripMenuItem.Name = "aboutTabletToolStripMenuItem";
            aboutTabletToolStripMenuItem.Size = new System.Drawing.Size(471, 54);
            aboutTabletToolStripMenuItem.Text = "About tablet";
            aboutTabletToolStripMenuItem.Click += aboutTabletToolStripMenuItem_Click;
            // 
            // label_CanvasPos
            // 
            label_CanvasPos.AutoSize = true;
            label_CanvasPos.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label_CanvasPos.Location = new System.Drawing.Point(211, 202);
            label_CanvasPos.Name = "label_CanvasPos";
            label_CanvasPos.Size = new System.Drawing.Size(63, 36);
            label_CanvasPos.TabIndex = 44;
            label_CanvasPos.Text = "---";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(24, 202);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(158, 41);
            label5.TabIndex = 43;
            label5.Text = "CanvasPos";
            // 
            // labeltilt_xy
            // 
            labeltilt_xy.AutoSize = true;
            labeltilt_xy.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            labeltilt_xy.Location = new System.Drawing.Point(211, 373);
            labeltilt_xy.Name = "labeltilt_xy";
            labeltilt_xy.Size = new System.Drawing.Size(63, 36);
            labeltilt_xy.TabIndex = 46;
            labeltilt_xy.Text = "---";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(24, 373);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(126, 41);
            label2.TabIndex = 45;
            label2.Text = "Tilt (X,Y)";
            // 
            // button_Clear
            // 
            button_Clear.Location = new System.Drawing.Point(1697, 104);
            button_Clear.Name = "button_Clear";
            button_Clear.Size = new System.Drawing.Size(188, 58);
            button_Clear.TabIndex = 47;
            button_Clear.Text = "Clear";
            button_Clear.UseVisualStyleBackColor = true;
            button_Clear.Click += button_Clear_Click_1;
            // 
            // buttonRec
            // 
            buttonRec.Location = new System.Drawing.Point(24, 617);
            buttonRec.Name = "buttonRec";
            buttonRec.Size = new System.Drawing.Size(170, 58);
            buttonRec.TabIndex = 48;
            buttonRec.Text = "Record";
            buttonRec.UseVisualStyleBackColor = true;
            buttonRec.Click += buttonRec_Click_1;
            // 
            // label_RecCount
            // 
            label_RecCount.AutoSize = true;
            label_RecCount.Location = new System.Drawing.Point(332, 544);
            label_RecCount.Name = "label_RecCount";
            label_RecCount.Size = new System.Drawing.Size(54, 41);
            label_RecCount.TabIndex = 49;
            label_RecCount.Text = "---";
            // 
            // button_replay
            // 
            button_replay.Location = new System.Drawing.Point(216, 617);
            button_replay.Name = "button_replay";
            button_replay.Size = new System.Drawing.Size(170, 58);
            button_replay.TabIndex = 50;
            button_replay.Text = "Play";
            button_replay.UseVisualStyleBackColor = true;
            button_replay.Click += button_replay_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(24, 544);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(245, 41);
            label1.TabIndex = 51;
            label1.Text = "Packets recorded";
            // 
            // buttonClearRecording
            // 
            buttonClearRecording.Location = new System.Drawing.Point(24, 787);
            buttonClearRecording.Name = "buttonClearRecording";
            buttonClearRecording.Size = new System.Drawing.Size(170, 58);
            buttonClearRecording.TabIndex = 52;
            buttonClearRecording.Text = "Clear";
            buttonClearRecording.UseVisualStyleBackColor = true;
            buttonClearRecording.Click += buttonClearRecording_Click;
            // 
            // buttonSavePackets
            // 
            buttonSavePackets.Location = new System.Drawing.Point(24, 702);
            buttonSavePackets.Name = "buttonSavePackets";
            buttonSavePackets.Size = new System.Drawing.Size(170, 58);
            buttonSavePackets.TabIndex = 53;
            buttonSavePackets.Text = "Save";
            buttonSavePackets.UseVisualStyleBackColor = true;
            buttonSavePackets.Click += buttonSavePackets_Click;
            // 
            // buttonLoadPackets
            // 
            buttonLoadPackets.Location = new System.Drawing.Point(216, 702);
            buttonLoadPackets.Name = "buttonLoadPackets";
            buttonLoadPackets.Size = new System.Drawing.Size(170, 58);
            buttonLoadPackets.TabIndex = 54;
            buttonLoadPackets.Text = "Load";
            buttonLoadPackets.UseVisualStyleBackColor = true;
            buttonLoadPackets.Click += buttonLoadPackets_Click;
            // 
            // buttonCopy
            // 
            buttonCopy.Location = new System.Drawing.Point(1516, 104);
            buttonCopy.Name = "buttonCopy";
            buttonCopy.Size = new System.Drawing.Size(175, 58);
            buttonCopy.TabIndex = 55;
            buttonCopy.Text = "Copy";
            buttonCopy.UseVisualStyleBackColor = true;
            buttonCopy.Click += buttonCopy_Click;
            // 
            // textBox_config
            // 
            textBox_config.Location = new System.Drawing.Point(24, 866);
            textBox_config.Multiline = true;
            textBox_config.Name = "textBox_config";
            textBox_config.ReadOnly = true;
            textBox_config.Size = new System.Drawing.Size(489, 354);
            textBox_config.TabIndex = 56;
            textBox_config.Text = "N/A";
            // 
            // buttonSettings
            // 
            buttonSettings.Location = new System.Drawing.Point(616, 1183);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new System.Drawing.Size(170, 58);
            buttonSettings.TabIndex = 57;
            buttonSettings.Text = "Settings";
            buttonSettings.UseVisualStyleBackColor = true;
            buttonSettings.Click += buttonSettings_Click;
            // 
            // FormWinTabPainterApp
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1956, 1250);
            Controls.Add(buttonSettings);
            Controls.Add(textBox_config);
            Controls.Add(buttonCopy);
            Controls.Add(buttonLoadPackets);
            Controls.Add(buttonSavePackets);
            Controls.Add(buttonClearRecording);
            Controls.Add(label1);
            Controls.Add(button_replay);
            Controls.Add(label_RecCount);
            Controls.Add(buttonRec);
            Controls.Add(button_Clear);
            Controls.Add(labeltilt_xy);
            Controls.Add(label2);
            Controls.Add(label_CanvasPos);
            Controls.Add(label5);
            Controls.Add(label_BrushSize);
            Controls.Add(label_BrushSizeValue);
            Controls.Add(trackBar_BrushSize);
            Controls.Add(label_TiltValue);
            Controls.Add(label_ScreenPosValue);
            Controls.Add(label_PressureValue);
            Controls.Add(label_ButtonsValue);
            Controls.Add(pictureBox_Canvas);
            Controls.Add(label_Tilt);
            Controls.Add(label_ScreenPos);
            Controls.Add(label_PressureNormalized);
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label_ScreenPos;
        private System.Windows.Forms.Label label_PressureNormalized;
        private System.Windows.Forms.Label label_Tilt;
        private System.Windows.Forms.Label label_Buttons;
        private System.Windows.Forms.PictureBox pictureBox_Canvas;
        private System.Windows.Forms.Label label_TiltValue;
        private System.Windows.Forms.Label label_ScreenPosValue;
        private System.Windows.Forms.Label label_PressureValue;
        private System.Windows.Forms.Label label_ButtonsValue;
        private System.Windows.Forms.TrackBar trackBar_BrushSize;
        private System.Windows.Forms.Label label_BrushSizeValue;
        private System.Windows.Forms.Label label_BrushSize;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_File;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_FileSave;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Open;
        private System.Windows.Forms.Label label_CanvasPos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showShortcutsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pressureCurveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutTabletToolStripMenuItem;
        private System.Windows.Forms.Label labeltilt_xy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Button buttonRec;
        private System.Windows.Forms.Label label_RecCount;
        private System.Windows.Forms.Button button_replay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonClearRecording;
        private System.Windows.Forms.Button buttonSavePackets;
        private System.Windows.Forms.Button buttonLoadPackets;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.TextBox textBox_config;
        private System.Windows.Forms.Button buttonSettings;
    }
}

