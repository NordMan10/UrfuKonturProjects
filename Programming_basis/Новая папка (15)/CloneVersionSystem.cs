using System;
using System.Collections.Generic;

namespace Clones
{
    public class StackItem
    {
        public string Value { get; set; }
        public StackItem Previous { get; set; }
    }

    public class LinkedStack
    {
        public StackItem Head { get; set; }

        public void Push(string value)
        {
            if (Head == null)
            {
                Head = new StackItem { Value = value, Previous = null };
            }
            else
            {
                var newStackItem = new StackItem { Value = value, Previous = Head };
            }
        }

        public string Pop()
        {
            var result = Head.Value;

            return 
        }
    }

    public class Command
    {
        public Dictionary<int, Clone> CommandClones { get; set; }
        public string[] RecievedCommand { get; set; }


        public string Perform()
        {
            var command = RecievedCommand[0];
            var cloneNumber = int.Parse(RecievedCommand[1]);
            var clone = CommandClones[cloneNumber];
            if (CommandClones.ContainsKey(cloneNumber))
            {
                switch (command)
                {
                    case ("learn"):
                        return PerformLearn(clone, RecievedCommand[2]);
                    case ("rollback"):
                        return PerformRollBack(clone);
                    case ("relearn"):
                        return PerformRelearn(clone);
                    case ("clone"):
                        return PerformClone(clone);
                    case ("check"):
                        return PerformCheck(clone);
                }
            }
            return null;
        }

        public string PerformLearn(Clone clone, string program)
        {
            if (!clone.Programs.Contains(program))
            {
                clone.RollBacks.Clear();
                clone.Programs.AddLast(program);
            }
            return null;
        }

        public string PerformRollBack(Clone clone)
        {
            if (clone.Programs.Count > 0)
            {
                clone.RollBacks.AddLast(clone.Programs.Last.Value);
                clone.Programs.RemoveLast();
            }
            return null;
        }

        public string PerformRelearn(Clone clone)
        {
            if (clone.RollBacks.Count > 0)
            {
                var rollBack = clone.RollBacks.Last.Value;
                clone.RollBacks.RemoveLast();
                clone.Programs.AddLast(rollBack);
            }
            return null;
        }

        public string PerformClone(Clone clone)
        {
            var cloneNumber = CommandClones.Count + 1;
            var cloneOfClone = new Clone(new LinkedList<string>(clone.Programs), new LinkedList<string>(clone.RollBacks), cloneNumber);
            CommandClones.Add(cloneOfClone.Number, cloneOfClone);
            return null;
        }

        public string PerformCheck(Clone clone)
        {
            if (clone.Programs.Count > 0)
            {
                return clone.Programs.Last.Value;
            }
            return "basic";
        }
    }

    public class Clone
    {
        public LinkedList<string> Programs { get; set; }
        public LinkedList<string> RollBacks { get; set; }
        public int Number { get; set; }

        public Clone(LinkedList<string> programs, LinkedList<string> rollBacks, int number)
        {
            Programs = programs;
            RollBacks = rollBacks;
            Number = number;
        }
    }

    public class CloneVersionSystem : ICloneVersionSystem
    {
        public Dictionary<int, Clone> Clones { get; set; }

        public CloneVersionSystem()
        {
            Clones = new Dictionary<int, Clone>();
            Clones.Add(1, new Clone(new LinkedList<string>(), new LinkedList<string>(), 1));
        }

        public string Execute(string query)
        {
            var splitCommand = query.Split(' ');
            var command = new Command { CommandClones = Clones, RecievedCommand = splitCommand };
            return command.Perform();
        }
    }
}