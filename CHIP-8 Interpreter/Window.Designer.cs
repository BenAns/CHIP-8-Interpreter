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
            ((System.ComponentModel.ISupportInitialize)(this.InterpreterOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // InterpreterOutput
            // 
            this.InterpreterOutput.Location = new System.Drawing.Point(142, 141);
            this.InterpreterOutput.Name = "InterpreterOutput";
            this.InterpreterOutput.Size = new System.Drawing.Size(320, 160);
            this.InterpreterOutput.TabIndex = 1;
            this.InterpreterOutput.TabStop = false;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.InterpreterOutput);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "Window";
            this.Text = "CHIP-8 Interpreter";
            ((System.ComponentModel.ISupportInitialize)(this.InterpreterOutput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox InterpreterOutput;
    }
}

