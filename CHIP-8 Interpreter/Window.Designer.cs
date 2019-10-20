namespace CHIP_8_Interpreter
{
    partial class Window
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
            this.InterpreterOutput = new System.Windows.Forms.PictureBox();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectROM = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.InterpreterSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.InputSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.InterpreterOutput)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // InterpreterOutput
            // 
            this.InterpreterOutput.Location = new System.Drawing.Point(0, 27);
            this.InterpreterOutput.Name = "InterpreterOutput";
            this.InterpreterOutput.Size = new System.Drawing.Size(640, 320);
            this.InterpreterOutput.TabIndex = 1;
            this.InterpreterOutput.TabStop = false;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStrip,
            this.SettingsToolStrip});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(684, 24);
            this.MenuStrip.TabIndex = 2;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // FileToolStrip
            // 
            this.FileToolStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectROM});
            this.FileToolStrip.Name = "FileToolStrip";
            this.FileToolStrip.Size = new System.Drawing.Size(37, 20);
            this.FileToolStrip.Text = "File";
            // 
            // SelectROM
            // 
            this.SelectROM.Name = "SelectROM";
            this.SelectROM.Size = new System.Drawing.Size(135, 22);
            this.SelectROM.Text = "Select ROM";
            this.SelectROM.Click += new System.EventHandler(this.SelectROM_Click);
            // 
            // SettingsToolStrip
            // 
            this.SettingsToolStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InterpreterSettings,
            this.InputSettings});
            this.SettingsToolStrip.Name = "SettingsToolStrip";
            this.SettingsToolStrip.Size = new System.Drawing.Size(61, 20);
            this.SettingsToolStrip.Text = "Settings";
            // 
            // InterpreterSettings
            // 
            this.InterpreterSettings.Name = "InterpreterSettings";
            this.InterpreterSettings.Size = new System.Drawing.Size(174, 22);
            this.InterpreterSettings.Text = "Interpreter Settings";
            // 
            // InputSettings
            // 
            this.InputSettings.Name = "InputSettings";
            this.InputSettings.Size = new System.Drawing.Size(174, 22);
            this.InputSettings.Text = "Input Settings";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 361);
            this.Controls.Add(this.InterpreterOutput);
            this.Controls.Add(this.MenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MenuStrip;
            this.MaximizeBox = false;
            this.Name = "Window";
            this.Text = "CHIP-8 Interpreter";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Window_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Window_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.InterpreterOutput)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox InterpreterOutput;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileToolStrip;
        private System.Windows.Forms.ToolStripMenuItem SelectROM;
        private System.Windows.Forms.ToolStripMenuItem SettingsToolStrip;
        private System.Windows.Forms.ToolStripMenuItem InterpreterSettings;
        private System.Windows.Forms.ToolStripMenuItem InputSettings;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

