using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CHIP_8_Interpreter
{
    class CHIP8_Interpreter
    {
        // Registers and memory for the interpreter
        private byte[] registers;
        private byte[] memory;
        private System.UInt16 addressRegister;
        private System.UInt16 programCounter;
        private System.UInt16 stackPointer;

        // Variables for awaiting key presses
        private bool awaitingKeyPress;
        private byte registerForKey;

        /*
         *  Initialises the interpreter:
         *      Sets all memory and registers to 0
         *      Initialises the program counter at 0x0200
         *      Gets the program's ROM data
         */
        public CHIP8_Interpreter(string ROMDirectory)
        {
            InitialiseInterpreter();
            LoadROM(ROMDirectory);
        }

        /*
         * Sets all memory and registers to 0
         * Sets the program counter to 0x0200
         * Makes it so that the interpreter does not await for a key press
         */
        private void InitialiseInterpreter()
        {
            registers = new byte[0xF];
            memory = new byte[0x10000];
            addressRegister = 0x0000;
            programCounter = 0x0200;
            stackPointer = 0x0000;

            awaitingKeyPress = false;
            registerForKey = 0x10;
        }

        // Loads a ROM file into the interpreter's memory
        private void LoadROM(string ROMDirectory)
        {
            // Gets the ROM data
            byte[] rom = File.ReadAllBytes(ROMDirectory);

            // Makes sure the ROM isn't too big
            if(rom.Length > 0x10000 - 0x0200)
            {
                throw new ROMTooLarge("CHIP-8 ROM is too big to fit in memory");
            }

            // Copies the ROM data to memory
            for(int i = 0; i < rom.Length; i++)
            {
                memory[i + 0x0200] = rom[i];
            }

        }

        // Performs a single clock cycle
        public void PerformCycle(ref CHIP8_Screen screen, ref CHIP8_Keyboard keyboard)
        {
            // Doesn't perform an operation if waiting for a key press
            if(awaitingKeyPress)
            {
                AwaitKey(keyboard);
                return;
            }

            // Gets an OP code from memory
            byte byte1 = memory[programCounter++];
            byte byte2 = memory[programCounter++];

            // Processes the OP code
            ProcessOPCode(byte1, byte2);
        }

        // Checks if a new key has been pressed and, if it has, puts its value into the appropriate register
        private void AwaitKey(CHIP8_Keyboard keyboard)
        {
            // Awaits a key press if necessary
            byte keyPress = keyboard.GetKeyPress();
            if(keyPress != 0x10)
            {
                registers[registerForKey] = keyPress;
                awaitingKeyPress = false;
            }
        }

        // Processes an OP code instruction
        private void ProcessOPCode(byte byte1, byte byte2)
        {
            switch((byte1 & 0xF0) >> 4)  // Filters through the first nibble
            {
                case 0x0:
                    switch(byte1 & 0x0F)  // Filters through the second nibble
                    {
                        case 0x0:
                            switch(byte2)  // Filters through the second byte
                            {
                                case 0xE0:  // 00E0
                                    break;

                                case 0xEE:  // 00EE
                                    break;
                            }
                            break;
                    }
                    break;

                case 0x1:  // 1NNN
                    break;
                case 0x2:  // 2NNN
                    break;
                case 0x3:  // 3XNN
                    break;
                case 0x4:  // 4XNN
                    break;
                case 0x5:
                    switch(byte2 & 0xF)  // Filters through the fourth nibble
                    {
                        case 0x0:  // 5XY0
                            break;
                    }
                    break;
                case 0x6:  // 6XNN
                    break;
                case 0x7:  // 7XNN
                    break;
                case 0x8:
                    switch(byte2 & 0xF)  // Filters through the fourth nibble
                    {
                        case 0x0:  // 8XY0
                            break;
                        case 0x1:  // 8XY1
                            break;
                        case 0x2:  // 8XY2
                            break;
                        case 0x3:  // 8XY3
                            break;
                        case 0x4:  // 8XY4
                            break;
                        case 0x5:  // 8XY5
                            break;
                        case 0x6:  // 8XY6
                            break;
                        case 0x7:  // 8XY7
                            break;
                        case 0xE:  // 8XYE
                            break;
                    }
                    break;
                case 0x9:
                    switch(byte2 & 0xF)  // Filters through the fourth nibble
                    {
                        case 0x0:  // 9XY0
                            break;
                    }
                    break;
                case 0xA:  // ANNN
                    break;
                case 0xB:  // BNNN
                    break;
                case 0xC:  // CXNN
                    break;
                case 0xD:  // DXYN
                    break;
                case 0xE:
                    switch(byte2)  // Filters through the second byte
                    {
                        case 0x9E:  // EX9E
                            break;
                        case 0xA1:  // EXA1
                            break;
                    }
                    break;
                case 0xF:
                    switch(byte2)  // Filters through the second byte
                    {
                        case 0x07:  // FX07
                            break;
                        case 0x0A:  // FX0A
                            break;
                        case 0x15:  // FX15
                            break;
                        case 0x18:  // FX18
                            break;
                        case 0x1E:  // FX1E
                            break;
                        case 0x29:  // FX29
                            break;
                        case 0x33:  // FX33
                            break;
                        case 0x55:  // FX55
                            break;
                        case 0x65:  // FX65
                            break;
                    }
                    break;
            }

        }

        // OP Codes

        private void FX0A(byte byte1, byte byte2, CHIP8_Keyboard keyboard)
        {
            // Starts the process of awaiting for a key press
            awaitingKeyPress = true;
            keyboard.StartAwaitKeyPress();

            // Gets the register to store the key press in
            registerForKey = (byte)(byte1 & 0x0F);
        }

    }

    class ROMTooLarge: Exception
    {
        public ROMTooLarge(string message): base(message)
        {}
    }
}
