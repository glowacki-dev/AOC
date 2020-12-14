using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day14 : ISolution<string>
    {
        public Dictionary<long, long> memory { get; private set; }
        public long maskOne { get; private set; }
        public string maskFloat { get; private set; }

        public object Run(Input<string> lines)
        {
            memory = new Dictionary<long, long>();
            foreach(string line in lines.Lines)
            {
                string[] split = line.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                string command = split[0];
                string value = split[1];
                if (command.StartsWith("mask")) SetMask(value);
                if (command.StartsWith("mem")) SetMem(command, value);
            }
            return memory.Values.Sum();
        }

        private void SetMem(string command, string value)
        {
            long address = long.Parse(command.Split(new char[] { '[', ']' })[1]);
            long val = long.Parse(value);
            address |= maskOne;
            WriteValue(address, maskFloat, val);
        }

        private void SetMask(string value)
        {
            //Console.WriteLine($"Mask: {value}");
            maskOne = Convert.ToInt64(value.Replace("X", "0"), 2);
            maskFloat = value.Replace("1", "0");
        }

        private void WriteValue(long address, string mask, long value)
        {
            if(!mask.Contains("X"))
            {
                //Console.WriteLine($"Memory[{address} | {Convert.ToString(address, 2)}] = {value}");
                memory[address] = value;
            }
            else
            {
                int index = mask.IndexOf("X");
                string newMask = mask.Remove(index, 1).Insert(index, "0");
                WriteValue(address | ((long)1 << (mask.Length - 1 - index)), newMask, value);
                WriteValue(address & ~((long)1 << mask.Length - 1 - index), newMask, value);
            }
        }
    }
}