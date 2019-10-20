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
        private byte[,] screenData;
        private Graphics outputScreen;
        private readonly int scaleFactor;

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
            screenData = new byte[64, 32];
            outputScreen = Graphics.FromImage(outputBitmap);
            scaleFactor = outputBitmap.Height / 32;

            // Sets the screen black
            outputScreen.FillRectangle(Brushes.Black, 0, 0, outputBitmap.Width, outputBitmap.Height);
        }

        // Clears the screen
        public void Clear()
        {
            // Sets the screen black
            outputScreen.FillRectangle(new SolidBrush(Color.Black), 0, 0, 64 * scaleFactor, 32 * scaleFactor);

            // Sets all the bits in screenData to 0
            Array.Clear(screenData, 0, screenData.Length);
        }

        /*
         *  Draws a sprite
         *  Returns the new value of the status register
         */
         public byte Draw(int topLeftX, int topLeftY, byte[] rows)
        {
            byte statusRegister = 0;

            // Puts each row of pixels onto the screen
            for(int row = 0; row < rows.Length; row++)
            {
                // Converts the current row to an array of pixels
                byte[] pixels = ByteToBits(rows[row]);

                // Puts each pixel onto the screen
                for(int pixel = 0; pixel < 8; pixel++)
                {
                    // Gets the screen coordinates of the current pixel
                    int pixelX = pixel + topLeftX;
                    int pixelY = row + topLeftY;

                    // Sets the pixel value in screenData
                    screenData[pixelX, pixelY] ^= pixels[pixel];

                    // Puts the pixel on the screen
                    outputScreen.FillRectangle(screenData[pixelX, pixelY]  == 1 ? Brushes.White : Brushes.Black,
                                               scaleFactor * pixelX, scaleFactor * pixelY, scaleFactor, scaleFactor);
                    
                    // Sets the value of the status register
                    statusRegister |= (byte)(pixels[pixel] & ~screenData[pixel + topLeftX, row + topLeftY]);
                }
            }

            return statusRegister;
        }

        // Converts a byte to an array of 8 bits
        private byte[] ByteToBits(byte processByte)
        {
            byte[] bits = new byte[8];
            for(int i = 7; i > -1; i--)
            {
                bits[i] = (byte)(processByte & 0x01);
                processByte >>= 1;
            }

            return bits;
        }
    }

    class InvalidScreenResolution: Exception
    {
        public InvalidScreenResolution(string message) : base(message)
        {}
    }
}
