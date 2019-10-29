using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

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
         *      Loads the sprites for characters into the ROM
         */
        public CHIP8_Interpreter(string ROMDirectory)
        {
            InitialiseInterpreter();
            LoadROM(ROMDirectory);
            LoadCharacters();
        }

        /*
         * Sets all memory and registers to 0
         * Sets the program counter to 0x0200
         * Sets the timers to 0
         * Makes it so that the interpreter does not await for a key press
         */
        private void InitialiseInterpreter()
        {
            registers = new byte[0x10];
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

        // Loads the sprites for characters into ROM
        private void LoadCharacters()
        {
            byte[] bytes = {0xF0, 0x90, 0x90, 0x90, 0xF0,
                            0x20, 0x60, 0x20, 0x20, 0x70,
                            0xF0, 0x10, 0xF0, 0x80, 0xF0,
                            0xF0, 0x10, 0xF0, 0x10, 0xF0,
                            0x90, 0x90, 0xF0, 0x10, 0x10,
                            0xF0, 0x80, 0xF0, 0x10, 0xF0,
                            0xF0, 0x80, 0xF0, 0x90, 0xF0,
                            0xF0, 0x10, 0x20, 0x40, 0x40,
                            0xF0, 0x90, 0xF0, 0x90, 0xF0,
                            0xF0, 0x90, 0xF0, 0x10, 0xF0,
                            0xF0, 0x90, 0xF0, 0x90, 0x90,
                            0xE0, 0x90, 0xE0, 0x90, 0xE0,
                            0xF0, 0x80, 0x80, 0x80, 0xF0,
                            0xE0, 0x90, 0x90, 0x90, 0xE0,
                            0xF0, 0x80, 0xF0, 0x80, 0xF0,
                            0xF0, 0x80, 0xF0, 0x80, 0x80};
            int fontsOffset = 0x40;

            for(int i = 0; i < bytes.Length; i++)
            {
                memory[i + fontsOffset] = bytes[i];
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

            return;
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
                            OPCode_5XY0((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
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
                            OPCode_8XY0((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
                            break;

                        case 0x1:  // 8XY1
                            OPCode_8XY1((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
                            break;

                        case 0x2:  // 8XY2
                            OPCode_8XY2((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
                            break;

                        case 0x3:  // 8XY3
                            OPCode_8XY3((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
                            break;

                        case 0x4:  // 8XY4
                            OPCode_8XY4((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
                            break;

                        case 0x5:  // 8XY5
                            OPCode_8XY5((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
                            break;

                        case 0x6:  // 8XY6
                            OPCode_8XY6((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
                            break;

                        case 0x7:  // 8XY7
                            OPCode_8XY7((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
                            break;

                        case 0xE:  // 8XYE
                            OPCode_8XYE((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
                            break;
                    }
                    break;

                case 0x9:
                    switch(byte2 & 0x0F)  // Filters through the fourth nibble
                    {
                        case 0x0:  // 9XY0
                            OPCode_9XY0((byte)(byte1 & 0x0F), (byte)((byte2 & 0xF0) >> 4));
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
            screen.Clear();
        }

        private void OPCode_00EE()
        {
            // Checks to make sure the stack pointer is in a valid position
            if(stackPointer == 0x0000)
            {
                return;
            }

            // Pops the stack into program counter
            programCounter = (UInt16)(((memory[stackPointer - 2]) << 0x8) + memory[stackPointer - 1]);
            stackPointer -= 2;
        }

        private void OPCode_1NNN(UInt16 address)
        {
            programCounter = address;
        }

        private void OPCode_2NNN(UInt16 address)
        {
            // Pushes the program counter's current value onto the stack
            stackPointer += 2;
            memory[stackPointer - 2] = (byte)((programCounter & 0xFF00) >> 0x8);
            memory[stackPointer - 1] = (byte)(programCounter & 0xFF);

            // Sets the address of the program counter
            programCounter = address;
        }

        private void OPCode_3XNN(byte register, byte number)
        {
            if(registers[register] == number)
            {
                programCounter += 2;
            }
        }

        private void OPCode_4XNN(byte register, byte number)
        {
            if(registers[register] != number)
            {
                programCounter += 2;
            }

        }

        private void OPCode_5XY0(byte register1, byte register2)
        {
            if(registers[register1] == registers[register2])
            {
                programCounter += 2;
            }
        }

        private void OPCode_6XNN(byte register, byte number)
        {
            registers[register] = number;
        }

        private void OPCode_7XNN(byte register, byte number)
        {
            registers[register] += number;
        }

        private void OPCode_8XY0(byte register1, byte register2)
        {
            registers[register1] = registers[register2];
        }

        private void OPCode_8XY1(byte register1, byte register2)
        {
            registers[register1] |= registers[register2];
        }

        private void OPCode_8XY2(byte register1, byte register2)
        {
            registers[register1] &= registers[register2];
        }

        private void OPCode_8XY3(byte register1, byte register2)
        {
            registers[register1] ^= registers[register2];
        }

        private void OPCode_8XY4(byte register1, byte register2)
        {
            UInt16 sum = (UInt16)(registers[register1] + registers[register2]);
            registers[0xF] = (byte)((sum & 0x0100) >> 8);
            registers[register1] = (byte)(sum & 0x00FF);
        }

        private void OPCode_8XY5(byte register1, byte register2)
        {
            registers[0xF] = registers[register1] > registers[register2] ? (byte)0x01 : (byte)0x00;
            registers[register1] -= registers[register2];
            ;
        }

        private void OPCode_8XY6(byte register1, byte register2)
        {
            registers[0XF] = (byte)(registers[register1] & 0x01);
            registers[register1] >>= 1;
        }

        private void OPCode_8XY7(byte register1, byte register2)
        {
            registers[0xF] = registers[register2] > registers[register1] ? (byte)0x01 : (byte)0x00;
            byte difference = (byte)((registers[register2] - registers[register1]) & 0xFF);
            registers[register1] = difference;
        }

        private void OPCode_8XYE(byte register1, byte register2)
        {
            registers[0XF] = (byte)((registers[register1] & 0x80) >> 7);
            registers[register1] <<= 1;
        }

        private void OPCode_9XY0(byte register1, byte register2)
        {
            if(registers[register1] != registers[register2])
            {
                programCounter += 2;
            }
        }

        private void OPCode_ANNN(UInt16 address)
        {
            addressRegister = address;
        }

        private void OPCode_BNNN(UInt16 address)
        {
            programCounter = (UInt16)(registers[0x0] + address);
        }

        private void OPCode_CXNN(byte register, byte number)
        {
            registers[register] = (byte)((byte)new Random().Next(0, 256) & number);
        }

        private void OPCode_DXYN(byte register1, byte register2, byte number, CHIP8_Screen screen)
        {
            // Gets the sprite
            byte[] rows = new byte[number];
            for(int i = 0; i < number; i++)
            {
                rows[i] = memory[addressRegister + i];
            }

            // Draws the sprite
            registers[0xF] = screen.Draw(registers[register1], registers[register2], rows);
        }

        private void OPCode_EX9E(byte register, CHIP8_Keyboard keyboard)
        {
            if(registers[register] < 0x10 && keyboard.keys[registers[register]])
            {
                programCounter += 2;
            }
        }

        private void OPCode_EXA1(byte register, CHIP8_Keyboard keyboard)
        {
            if(registers[register] < 0x10 && !keyboard.keys[registers[register]])
            {
                programCounter += 2;
            }
        }

        private void OPCode_FX07(byte register)
        {
            registers[register] = delayTimer;
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
            // Sets the value of the delay timer
            delayTimer = registers[register];

            // Decrements the delay timer at 60Hz
            Timer delayTimerDecrement = new Timer();
            delayTimerDecrement.Interval = 1000 / 60;
            delayTimerDecrement.Tick += new System.EventHandler((object sender, EventArgs e) =>
            {
                delayTimer--;
                if(delayTimer == 0)
                {
                    delayTimerDecrement.Stop();
                }
            });
            delayTimerDecrement.Start();
        }

        private void OPCode_FX18(byte register)
        {
            // Sets the value of the sound timer
            soundTimer = registers[register];

            // Decrements the sound timer at 60Hz
            Timer soundTimerDecrement = new Timer();
            soundTimerDecrement.Interval = 1000 / 60;
            soundTimerDecrement.Tick += new System.EventHandler((object sender, EventArgs e) =>
            {
                soundTimer--;
                if(soundTimer == 0)
                {
                    soundTimerDecrement.Stop();
                }
            });
            soundTimerDecrement.Start();
        }

        private void OPCode_FX1E(byte register)
        {
            addressRegister += registers[register];
        }

        private void OPCode_FX29(byte register)
        {
            byte registerValue = registers[register];

            // Makes sure the value in the register is in the valid range
            if(registerValue > 0xF)
            {
                return;
            }

            // Sets the value of the address register
            addressRegister = (UInt16)(0x40 + registerValue * 0x5);
        }

        private void OPCode_FX33(byte register)
        {
            // Converts the register's value to BCD
            byte registerValue = registers[register];
            byte[] BCDBytes = new byte[3];
            for(int i = 2; i > -1; i--)
            {
                BCDBytes[i] = (byte)(registerValue % 10);
                registerValue /= 10;
            }

            // Moves the BCD value to memory
            for(int i = 0; i < 3; i++)
            {
                memory[addressRegister + i] = BCDBytes[i];
            }
        }

        private void OPCode_FX55(byte register)
        {
            for(int i = 0; i <= register; i++)
            {
                memory[addressRegister + i] = registers[i];
            }
        }

        private void OPCode_FX65(byte register)
        {
            for(int i = 0; i <= register; i++)
            {
                registers[i] = memory[addressRegister + i];
            }
        }
    }

    class ROMTooLarge: Exception
    {
        public ROMTooLarge(string message): base(message)
        {}
    }
}
