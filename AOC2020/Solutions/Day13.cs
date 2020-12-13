using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day13 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            List<string> input = lines.Lines.ToList();
            List<Tuple<int, int>> buses = input[1].Split(',').Select((number, index) => new Tuple<int, string>(index, number)).Where(data => data.Item2 != "x").Select(data => new Tuple<int, int>(data.Item1, int.Parse(data.Item2))).ToList();
            decimal offset = buses.First().Item2;
            decimal timestamp = 0;
            foreach(Tuple<int, int> bus in buses)
            {
                if (bus.Item2 == offset) continue;
                Console.WriteLine($"Starting from {timestamp} with offset {offset}");
                while (true)
                {
                    timestamp += offset;
                    Console.WriteLine($"Check timestamp: {timestamp}");
                    if((timestamp + bus.Item1) % bus.Item2 == 0)
                    {
                        Console.WriteLine("MATCH");
                        break;
                    }
                }
                offset = offset * bus.Item2;
                Console.WriteLine($"New offset is {offset}. Moving on to the next bus");
            }
            Console.WriteLine("All buses done");
            foreach (Tuple<int, int> bus in buses)
            {
                Console.WriteLine($"bus {bus.Item2} ({bus.Item1}) will leave at {timestamp + bus.Item1} => {(timestamp + bus.Item1) % bus.Item2}");
            }
            return timestamp;
        }
    }
}