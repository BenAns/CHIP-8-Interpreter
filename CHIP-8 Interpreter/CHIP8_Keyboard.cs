using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHIP_8_Interpreter
{
    class CHIP8_Keyboard
    {
        // Data stored about keys
        public bool[] keys;
        public Keys[] configuration;
        private byte latestKeyDown;
        private bool keyDownUpdate;
        private object update = new object();

        /*
         *  Initialises the keyboard:
         *      Sets all the keys to state false
         */
        public CHIP8_Keyboard()
        {
            // Sets all data about keys to null
            keys = new bool[0x10];
            latestKeyDown = 0;
            keyDownUpdate = false;

            // Sets the key configuration
            configuration = new Keys[16] {Keys.X, Keys.D1, Keys.D2, Keys.D3, Keys.Q, Keys.W, Keys.E, Keys.A, Keys.S, Keys.D, Keys.Z, Keys.C, Keys.D4, Keys.R, Keys.F, Keys.V};
        }

        // Starts awaiting for a new key press
        public void StartAwaitKeyPress()
        {
            keyDownUpdate = false;
        }

        /*
         * Returns a new key press that has been awaited for
         * If a new key has not been pressed, returns 0x10
         */
        public byte GetKeyPress()
        {
            if(!keyDownUpdate)
            {
                return 0x10;
            }
            else
            {
                return latestKeyDown;
            }
        }

        // Processes a key being pressed down
        public void KeyDown(KeyEventArgs e)
        {
            SetStatus(e, true);
        }

        // Processes a key being release up
        public void KeyUp(KeyEventArgs e)
        {
            SetStatus(e, false);
        }

        // Sets the status of a key
        private void SetStatus(KeyEventArgs e, bool isDown)
        {
            int keyNumber = Array.IndexOf(configuration, e.KeyCode);
            if(keyNumber != -1)
            {
                if(isDown && !keys[keyNumber])
                {
                    latestKeyDown = (byte)keyNumber;
                    keyDownUpdate = true;
                }
                keys[keyNumber] = isDown;
            }
        }
    }
}
