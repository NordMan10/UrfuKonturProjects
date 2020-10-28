using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public VirtualMachine(string program, int memorySize)
		{
            Instructions = program;
            Memory = new byte[memorySize];
            AssignedCommands = new Dictionary<char, Action<IVirtualMachine>>();
        }

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
            AssignedCommands[symbol] = execute;
		}
        
        private Dictionary<char, Action<IVirtualMachine>> AssignedCommands { get; set; }
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
        public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
        public void Run()
		{
            while (InstructionPointer < Instructions.Length)
            {
                if (AssignedCommands.ContainsKey(Instructions[InstructionPointer]))
                    AssignedCommands[Instructions[InstructionPointer]](this);
                InstructionPointer++;
            }
		}
	}
}