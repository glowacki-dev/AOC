using System;
using System.Collections.Generic;

namespace AOC2020
{
    internal class Day01 : ISolution<int>
    {
        public void Run(IEnumerable<int> lines)
        {
            var numbers = new HashSet<int>(lines);
            foreach(int first in numbers)
            {
                foreach(int second in numbers)
                {
                    if(numbers.Contains(2020 - first - second))
                    {
                        Console.WriteLine(first * second * (2020 - first - second));
                        return;
                    }
                }
            }
        }
    }
} 