using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CHIP_8_Interpreter
{
    class CHIP8_Screen
    {
        // Variables used to hold data about the current screen state
        private byte[] screenData;
        private Bitmap outputScreen;
        int scaleFactor;

        /*
         *  Initialises the screen:
         *      Makes sure the resolution is valid
         *      Sets the screen data to 0
         *      Sets outputScreen ready to display
         */
        public CHIP8_Screen(ref Bitmap outputBitmap)
        {
            // Checks that the width and height are multiples are 64 and 32 respectively
            if(outputBitmap.Width != 2 * outputBitmap.Height || outputBitmap.Height % 32 != 0 || outputBitmap.Height == 0)
            {
                throw new InvalidScreenResolution("Invalid screen resolution");
            }

            // Initialises the screen variables
            screenData = new byte[64 * 32];
            outputScreen = outputBitmap;
            scaleFactor = outputScreen.Height / 32;

            // Sets the screen black
            using(Graphics graphics = Graphics.FromImage(outputScreen))
            {
                graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, outputScreen.Width, outputScreen.Height);
            }
        }
    }

    class InvalidScreenResolution: Exception
    {
        public InvalidScreenResolution(string message) : base(message)
        {}
    }
}
