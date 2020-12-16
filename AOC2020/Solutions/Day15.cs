using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day15 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            List<int> input = lines.Lines.First().Split(',').Select(i => int.Parse(i)).ToList();
            int turnNumber = 0;
            Dictionary<int, int> lastOccurence = new Dictionary<int, int>();
            Dictionary<int, int> turnsApart = new Dictionary<int, int>();
            int lastNumber = 0;
            foreach(int startingNumber in input)
            {
                lastOccurence[startingNumber] = turnNumber;
                lastNumber = startingNumber;
                turnNumber++;
            }
            while(turnNumber < 30000000)
            {
                if(turnsApart.ContainsKey(lastNumber))
                {
                    lastNumber = turnsApart[lastNumber];
                }
                else
                {
                    lastNumber = 0;
                }

                if (lastOccurence.ContainsKey(lastNumber))
                {
                    turnsApart[lastNumber] = turnNumber - lastOccurence[lastNumber];
                }
                lastOccurence[lastNumber] = turnNumber;
                turnNumber++;
            }
            return lastNumber;
        }
    }
}