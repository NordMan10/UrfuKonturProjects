using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
        public static Dictionary<int, int> LoopsLocation { get; set; }
        public static Stack<int> LeftBracets { get; set; }
        public static bool IsFirst = true;

        private static void WriteLoopsLocation(IVirtualMachine vm)
        {
            LoopsLocation = new Dictionary<int, int>();
            LeftBracets = new Stack<int>();
            var chars = new char[] { '[', ']' };
            var newCharIndex = -1;
            while (true)
            {
                newCharIndex = vm.Instructions.IndexOfAny(new char[] { '[', ']' }, newCharIndex + 1);
                if (newCharIndex == -1) break;
                if (vm.Instructions[newCharIndex] == '[')
                    LeftBracets.Push(newCharIndex);
                else
                {
                    var tempIndex = LeftBracets.Pop();
                    LoopsLocation[tempIndex] = newCharIndex;
                    LoopsLocation[newCharIndex] = tempIndex;
                }
            }
        }

        public static void RegisterTo(IVirtualMachine vm)
		{
            
            WriteLoopsLocation(vm);
            IsFirst = false;
                
            vm.RegisterCommand('[', b =>
            {
                if (b.Memory[b.MemoryPointer] == 0)
                    b.InstructionPointer = LoopsLocation[b.InstructionPointer];
            });
			vm.RegisterCommand(']', b =>
            {
                if (b.Memory[b.MemoryPointer] != 0)
                    b.InstructionPointer = LoopsLocation[b.InstructionPointer];
            });
        }
	}
}