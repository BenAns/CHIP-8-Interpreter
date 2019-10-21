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
        private string ROMDirectory = "";
        private bool interpreting = false;
        private const int clockSpeed = 500;
        private CHIP8_Interpreter interpreter;
        private CHIP8_Screen screen;
        private CHIP8_Keyboard keyboard;
        private Timer clockTimer;

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
            StartInterpreteter();
        }

        // Starts the interpreter
        private void StartInterpreteter()
        {
            // Ends any previous interpreter session
            interpreting = false;

            // Initialises the interpreter
            interpreter = InitialiseInterpreter();
            if(interpreter == null)
            {
                return;
            }

            // Initialises the interpreter output
            screen = InitialiseScreen();
            if(screen == null)
            {
                return;
            }

            // Initialises the interpreter keyboard
            keyboard = InitialiseKeyboard();
            if(keyboard == null)
            {
                return;
            }

            // Sets up the clock to run at 500Hz and starts interpreting
            interpreting = true;
            clockTimer = new Timer();
            clockTimer.Interval = 1000 / clockSpeed;
            clockTimer.Tick += new System.EventHandler(ClockTick);
            clockTimer.Start();
        }

        // Performs one clock tick of interpreting
        private void ClockTick(object sender, EventArgs e)
        {
            interpreter.PerformCycle(ref screen, ref keyboard);
            Refresh();

            if(!interpreting)
            {
                clockTimer.Stop();
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

        // Initialises the interpreter keyboard
        private CHIP8_Keyboard InitialiseKeyboard()
        {
            try
            {
                return new CHIP8_Keyboard();
            }
            catch
            {
                MessageBox.Show("Error: Could not initialise the interpreter keyboard", "Interpreter keyboard error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            keyboard.KeyDown(e);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            keyboard.KeyUp(e);
        }
    }
}
