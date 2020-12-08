using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day08 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            List<Instruction> instructions = lines.Lines.Select(line => Instruction.Parse(line)).ToList();
            return new Computer(instructions).Execute();
        }

        private class Instruction
        {
            public Instruction(string operation, int argument)
            {
                Operation = operation;
                Argument = argument;
                Executed = false;
                Changed = false;
            }

            public string Operation { get; set; }
            public int Argument { get; }
            public bool Executed { get; set; }
            public bool Changed { get; set; }

            internal static Instruction Parse(string line)
            {
                string[] arguments = line.Split(' ');
                return new Instruction(arguments[0], int.Parse(arguments[1]));
            }

            override public string ToString()
            {
                return $"{Operation}({Argument}) [Executed: {Executed}] [Changed: {Changed}]";
            }

            private bool Change()
            {
                switch (Operation)
                {
                    case "nop":
                        Operation = "jmp";
                        break;
                    case "jmp":
                        Operation = "nop";
                        break;
                    default:
                        return false;
                }
                Changed = !Changed;
                return true;
            }

            internal bool Fix()
            {
                if (Changed) return false;
                return Change();
            }
            internal bool UnFix()
            {
                if (!Changed) return false;
                return Change();
            }
        }

        private class Computer
        {
            private List<Instruction> instructions;
            Stack<Instruction> stack = new Stack<Instruction>();
            int position = 0;
            int accumulator = 0;
            bool changed = false;
            private Instruction CurrentInstruction
            {
                get
                {
                    return stack.Peek();
                }
            }

            public Computer(List<Instruction> instructions)
            {
                this.instructions = instructions;
                stack.Push(instructions[position]);
            }

            internal object Execute()
            {
                while (true)
                {
                    if (Step())
                    {
                        if (position >= instructions.Count) return accumulator;
                        stack.Push(instructions[position]);
                    }
                    else
                    {
                        stack.Pop(); // Tip of stack will be the instruction that would be executed the second time - we didn't execute it yet
                        Backtrace();
                    }
                }
            }

            private void Backtrace()
            {
                while (true)
                {
                    if (Undo()) break;
                }
            }

            private bool Undo()
            {
                if (!CurrentInstruction.Executed)
                {
                    stack.Pop();
                    return false;
                }
                switch (CurrentInstruction.Operation)
                {
                    case "nop":
                        position--;
                        break;
                    case "acc":
                        accumulator -= CurrentInstruction.Argument;
                        position--;
                        break;
                    case "jmp":
                        position -= CurrentInstruction.Argument;
                        break;
                }
                CurrentInstruction.Executed = false;
                return AttemptFix();
            }

            private bool AttemptFix()
            {
                if (changed)
                {
                    if (CurrentInstruction.UnFix())
                    {
                        changed = false;
                    }
                    return false;
                }
                else
                {
                    if (CurrentInstruction.Fix())
                    {
                        changed = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            private bool Step()
            {
                if (CurrentInstruction.Executed) return false;
                CurrentInstruction.Executed = true;
                switch (CurrentInstruction.Operation)
                {
                    case "nop":
                        position++;
                        break;
                    case "acc":
                        accumulator += CurrentInstruction.Argument;
                        position++;
                        break;
                    case "jmp":
                        position += CurrentInstruction.Argument;
                        break;
                }
                return true;
            }
        }
    }
}