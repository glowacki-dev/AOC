using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day10 : ISolution<int>
    {
        public object Run(Input<int> lines)
        {
            int currentRating = 0;
            List<List<int>> removables = new List<List<int>>
            {
                new List<int>() { 0 }
            };
            foreach (int rating in lines.Lines.OrderBy(val => val))
            {
                if (rating - currentRating == 3) removables.Add(new List<int>());
                removables.Last().Add(rating);
                currentRating = rating;
            }
            // 0, 1 and 2 lengths can't be changed anyway
            return removables.Where(r => r.Count > 2).Aggregate<List<int>, decimal>(1, (result, element) => result *= PossibleSettingsCount(element));
        }

        private int PossibleSettingsCount(List<int> rems)
        {
            // Can't change the first and last element.
            // Manual precount of 2 and 3 spaced windows
            // with more assumptions that should be valid,
            // but it somehow works fine
            switch(rems.Count)
            {
                case 3:
                    return 2;
                case 4:
                    return 4;
                case 5:
                    return 7;
                default:
                    Console.WriteLine("Invalid size of " + rems.Count);
                    return 1;
            }
        }
    }
}