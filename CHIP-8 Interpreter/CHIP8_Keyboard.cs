using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP_8_Interpreter
{
    class CHIP8_Keyboard
    {
        // Data stored about keys
        public bool[] keys;
        private byte latestKey;
        private bool keyUpdate;

        /*
         *  Initialises the keyboard:
         *      Sets all the keys to state false
         */
        public CHIP8_Keyboard()
        {
            // Sets all data about keys to null
            keys = new bool[0xF];
            latestKey = 0;
            keyUpdate = false;
        }

        // Awaits and returns a key press
        public byte GetKey()
        {
            return latestKey;
        }

        // Processes a key being pressed down
        public void KeyDown()
        {}

        // Processes a key being release up
        public void KeyUp()
        {}
    }
}
