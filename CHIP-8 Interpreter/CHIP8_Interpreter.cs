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

        // Timers
        private byte delayTimer;
        private byte soundTimer;

        // Variables for awaiting key presses
        private bool awaitingKeyPress;
        private byte registerForKey;

        /*
         *  Initialises the interpreter:
         *      Sets all memory, registers and timers to 0
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
         * Sets the timers to 0
         * Makes it so that the interpreter does not await for a key press
         */
        private void InitialiseInterpreter()
        {
            registers = new byte[0xF];
            memory = new byte[0x10000];
            addressRegister = 0x0000;
            programCounter = 0x0200;
            stackPointer = 0x0000;

            delayTimer = 0x00;
            soundTimer = 0x00;

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
            ProcessOPCode(byte1, byte2, ref screen, ref keyboard);
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
        private void ProcessOPCode(byte byte1, byte byte2, ref CHIP8_Screen screen, ref CHIP8_Keyboard keyboard)
        {
            // Finds the function to execute for the current OP code, gets its arguments, then calls the function
            switch((byte1 & 0xF0) >> 4)  // Filters through the first nibble
            {
                case 0x0:
                    switch(byte1 & 0x0F)  // Filters through the second nibble
                    {
                        case 0x0:
                            switch(byte2)  // Filters through the second byte
                            {
                                case 0xE0:  // 00E0
                                    OPCode_00E0(ref screen);
                                    break;

                                case 0xEE:  // 00EE
                                    OPCode_00EE();
                                    break;
                            }
                            break;
                    }
                    break;

                case 0x1:  // 1NNN
                    OPCode_1NNN((UInt16)(((UInt16)(byte1 & 0x0F) << 8) + (UInt16)byte2));
                    break;

                case 0x2:  // 2NNN
                    OPCode_2NNN((UInt16)(((UInt16)(byte1 & 0x0F) << 8) + (UInt16)byte2));
                    break;

                case 0x3:  // 3XNN
                    OPCode_3XNN((byte)(byte1 & 0x0F), byte2);
                    break;

                case 0x4:  // 4XNN
                    OPCode_4XNN((byte)(byte1 & 0x0F), byte2);
                    break;

                case 0x5:
                    switch(byte2 & 0x0F)  // Filters through the fourth nibble
                    {
                        case 0x0:  // 5XY0
                            OPCode_5XY0((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;
                    }
                    break;

                case 0x6:  // 6XNN
                    OPCode_6XNN((byte)(byte1 & 0x0F), byte2);
                    break;

                case 0x7:  // 7XNN
                    OPCode_7XNN((byte)(byte1 & 0x0F), byte2);
                    break;

                case 0x8:
                    switch(byte2 & 0x0F)  // Filters through the fourth nibble
                    {
                        case 0x0:  // 8XY0
                            OPCode_8XY0((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;

                        case 0x1:  // 8XY1
                            OPCode_8XY1((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;

                        case 0x2:  // 8XY2
                            OPCode_8XY2((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;

                        case 0x3:  // 8XY3
                            OPCode_8XY3((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;

                        case 0x4:  // 8XY4
                            OPCode_8XY4((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;

                        case 0x5:  // 8XY5
                            OPCode_8XY5((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;

                        case 0x6:  // 8XY6
                            OPCode_8XY6((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;

                        case 0x7:  // 8XY7
                            OPCode_8XY7((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;

                        case 0xE:  // 8XYE
                            OPCode_8XYE((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;
                    }
                    break;
                case 0x9:
                    switch(byte2 & 0x0F)  // Filters through the fourth nibble
                    {
                        case 0x0:  // 9XY0
                            OPCode_9XY0((byte)(byte1 & 0x0F), (byte)((byte1 & 0xF0) >> 4));
                            break;
                    }
                    break;

                case 0xA:  // ANNN
                    OPCode_ANNN((UInt16)(((UInt16)(byte1 & 0x0F) << 8) + (UInt16)byte2));
                    break;

                case 0xB:  // BNNN
                    OPCode_BNNN((UInt16)(((UInt16)(byte1 & 0x0F) << 8) + (UInt16)byte2));
                    break;

                case 0xC:  // CXNN
                    OPCode_CXNN((byte)(byte1 & 0x0F), byte2);
                    break;

                case 0xD:  // DXYN
                    OPCode_DXYN((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4), (byte)(byte2 & 0x0F), screen);
                    break;

                case 0xE:
                    switch(byte2)  // Filters through the second byte
                    {
                        case 0x9E:  // EX9E
                            OPCode_EX9E((byte)(byte1 & 0x0F), keyboard);
                            break;

                        case 0xA1:  // EXA1
                            OPCode_EXA1((byte)(byte1 & 0x0F), keyboard);
                            break;
                    }
                    break;

                case 0xF:
                    switch(byte2)  // Filters through the second byte
                    {
                        case 0x07:  // FX07
                            OPCode_FX07((byte)(byte1 & 0x0F));
                            break;

                        case 0x0A:  // FX0A
                            OPCode_FX0A((byte)(byte1 & 0x0F), keyboard);
                            break;

                        case 0x15:  // FX15
                            OPCode_FX15((byte)(byte1 & 0x0F));
                            break;

                        case 0x18:  // FX18
                            OPCode_FX18((byte)(byte1 & 0x0F));
                            break;

                        case 0x1E:  // FX1E
                            OPCode_FX1E((byte)(byte1 & 0x0F));
                            break;

                        case 0x29:  // FX29
                            OPCode_FX29((byte)(byte1 & 0x0F));
                            break;

                        case 0x33:  // FX33
                            OPCode_FX33((byte)(byte1 & 0x0F));
                            break;

                        case 0x55:  // FX55
                            OPCode_FX55((byte)(byte1 & 0x0F));
                            break;

                        case 0x65:  // FX65
                            OPCode_FX65((byte)(byte1 & 0x0F));
                            break;
                    }
                    break;
            }

        }

        /*
         *  OP Codes:
         *      Each method executes the corresponding OP code instruction indicated by its name
         */
        
        private void OPCode_00E0(ref CHIP8_Screen screen)
        {

        }

        private void OPCode_00EE()
        {

        }

        private void OPCode_1NNN(UInt16 address) 
        {

        }

        private void OPCode_2NNN(UInt16 address)
        {

        }

        private void OPCode_3XNN(byte register, byte number)
        {

        }

        private void OPCode_4XNN(byte register, byte number)
        {

        }

        private void OPCode_5XY0(byte register1, byte register2)
        {

        }

        private void OPCode_6XNN(byte register, byte number)
        {

        }

        private void OPCode_7XNN(byte register, byte number)
        {

        }

        private void OPCode_8XY0(byte register1, byte register2)
        {

        }

        private void OPCode_8XY1(byte register1, byte register2)
        {

        }

        private void OPCode_8XY2(byte register1, byte register2)
        {

        }

        private void OPCode_8XY3(byte register1, byte register2)
        {

        }

        private void OPCode_8XY4(byte register1, byte register2)
        {

        }

        private void OPCode_8XY5(byte register1, byte register2)
        {

        }

        private void OPCode_8XY6(byte register1, byte register2)
        {

        }

        private void OPCode_8XY7(byte register1, byte register2)
        {

        }

        private void OPCode_8XYE(byte register1, byte register2)
        {

        }

        private void OPCode_9XY0(byte register1, byte register2)
        {

        }

        private void OPCode_ANNN(UInt16 address)
        {

        }

        private void OPCode_BNNN(UInt16 address2)
        {

        }

        private void OPCode_CXNN(byte register, byte number)
        {

        }

        private void OPCode_DXYN(byte register1, byte register2, byte number, CHIP8_Screen screen)
        {

        }

        private void OPCode_EX9E(byte register, CHIP8_Keyboard keyboard)
        {

        }

        private void OPCode_EXA1(byte register, CHIP8_Keyboard keyboard)
        {

        }

        private void OPCode_FX07(byte register2)
        {

        }

        private void OPCode_FX0A(byte register, CHIP8_Keyboard keyboard)
        {
            // Starts the process of awaiting for a key press
            awaitingKeyPress = true;
            keyboard.StartAwaitKeyPress();

            // Gets the register to store the key press in
            registerForKey = register;
        }

        private void OPCode_FX15(byte register)
        {

        }

        private void OPCode_FX18(byte register)
        {

        }

        private void OPCode_FX1E(byte register)
        {

        }

        private void OPCode_FX29(byte register)
        {

        }

        private void OPCode_FX33(byte register)
        {

        }

        private void OPCode_FX55(byte register)
        {

        }

        private void OPCode_FX65(byte register)
        {

        }

    }

    class ROMTooLarge: Exception
    {
        public ROMTooLarge(string message): base(message)
        {}
    }
}
