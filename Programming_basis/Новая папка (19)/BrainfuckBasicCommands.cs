using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
        private static void Shift(IVirtualMachine vm)
        {
            vm.RegisterCommand('>', b =>
            {
                if (b.MemoryPointer == b.Memory.Length - 1) b.MemoryPointer = 0;
                else b.MemoryPointer++;
            });
            vm.RegisterCommand('<', b =>
            {
                if (b.MemoryPointer == 0) b.MemoryPointer = b.Memory.Length - 1;
                else b.MemoryPointer--;
            });
        }

        private static void IncAndDec(IVirtualMachine vm)
        {
            vm.RegisterCommand('+', b =>
            {
                var element = b.Memory[b.MemoryPointer];
                if (element == 255) element = 0;
                else element++;
                b.Memory[b.MemoryPointer] = element;
            });
            vm.RegisterCommand('-', b =>
            {
                var element = b.Memory[b.MemoryPointer];
                if (element == 0) element = 255;
                else element--;
                b.Memory[b.MemoryPointer] = element;
            });
        }

        private static void ReadAndWrite(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand(',', b => b.Memory[b.MemoryPointer] = (byte)read());
            vm.RegisterCommand('.', b => write((char)b.Memory[b.MemoryPointer]));
        }

        public static string CharsSequince(char startChar, char endChar)
        {
            var sequince = "";
            for (var i = startChar; i <= endChar; i++)
                sequince += i;
            return sequince;
        }

        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
            ReadAndWrite(vm, read, write);
            IncAndDec(vm);
            Shift(vm);

            var symbols = string.Concat(CharsSequince('A', 'Z'), CharsSequince('a', 'z'), CharsSequince('0', '9'));

            foreach (var symbol in symbols)
            {
                vm.RegisterCommand(symbol, b =>
                {
                    b.Memory[b.MemoryPointer] = (byte)symbol;
                });
            }
        }
	}
}