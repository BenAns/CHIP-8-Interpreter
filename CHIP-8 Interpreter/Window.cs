using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHIP_8_Interpreter
{
    public partial class Window : Form
    {
        public string ROMDirectory = "";

        public Window()
        {
            InitializeComponent();

            // Sets the resolution
            int width = InterpreterOutput.ClientRectangle.Width;
            int height = InterpreterOutput.ClientRectangle.Height;
            this.Width = InterpreterOutput.ClientRectangle.X + width;
            this.Height = InterpreterOutput.ClientRectangle.Y + height;
        }

        // Selects the ROM file and starts the interpreter
        private void SelectROM_Click(object sender, EventArgs e)
        {
            // Gets the ROM file's path
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            ROMDirectory = fileDialog.FileName;

            // Starts the interpreter
            StartInterpretation();
        }

        // Starts the interpreter
        private void StartInterpretation()
        {
            // Initialises the interpreter
            CHIP8_Interpreter interpreter = InitialiseInterpreter();
            if(interpreter == null)
            {
                return;
            }

            // Initialises the interpreter output
            CHIP8_Screen screen = InitialiseScreen();
            if(screen == null)
            {
                return;
            }
        }

        // Initialises the interpreter
        private CHIP8_Interpreter InitialiseInterpreter()
        {
            try
            {
                return new CHIP8_Interpreter(ROMDirectory);
            }
            catch(ROMTooLarge)
            {
                MessageBox.Show("Error: ROM file is too large - Make sure you have selected the correct file, try to make the file shorter", "ROM file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch
            {
                MessageBox.Show("Error: Invalid ROM path - Makes sure it exists, is accessible and is not being used", "ROM file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Initialises the interpreter output
        private CHIP8_Screen InitialiseScreen()
        {
            try
            {
                Bitmap outputScreen = new Bitmap(InterpreterOutput.ClientRectangle.Width, InterpreterOutput.ClientRectangle.Height);
                InterpreterOutput.Image = outputScreen;
                return new CHIP8_Screen(ref outputScreen);
            }
            catch(InvalidScreenResolution)
            {
                MessageBox.Show("Error: Screen resolution is invalid - Make sure the width and height are multiples of 64 and 32 respectively", "Interpreter output error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch
            {
                MessageBox.Show("Error: Could not initialise the interpreter output", "Interpreter output error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
