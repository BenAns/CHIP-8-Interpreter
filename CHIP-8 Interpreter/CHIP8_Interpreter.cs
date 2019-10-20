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
        private System.Int16 addressRegister;
        private System.Int16 programCounter;

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
         */
        private void InitialiseInterpreter()
        {
            registers = new byte[0xF];
            memory = new byte[0x10000];
            addressRegister = 0x0000;
            programCounter = 0x0200;
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
    }

    class ROMTooLarge: Exception
    {
        public ROMTooLarge(string message): base(message)
        {}
    }
}
