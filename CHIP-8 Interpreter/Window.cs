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
            CHIP8_Interpreter interpreter = new CHIP8_Interpreter(ROMDirectory);
        }
    }
}
