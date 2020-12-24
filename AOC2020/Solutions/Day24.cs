using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020
{
    internal class Day24 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            HashSet<string> blacks = new HashSet<string>();
            foreach(string line in lines.Lines)
            {
                Coordinates coordinates = Coordinates.Parse(line);
                if (blacks.Contains(coordinates.ToString())) blacks.Remove(coordinates.ToString());
                else blacks.Add(coordinates.ToString());
            }
            for(int day = 1; day <= 100; day++)
            {
                Dictionary<string, int> whitesThatMayFlip = new Dictionary<string, int>();
                List<string> blacksThatWillFlip = new List<string>();
                foreach(string tile in blacks)
                {
                    int blacksCount = 0;
                    foreach(string neighbour in Neighbours(tile))
                    {
                        if(blacks.Contains(neighbour))
                        {
                            blacksCount++;
                        }
                        else
                        {
                            if (!whitesThatMayFlip.ContainsKey(neighbour)) whitesThatMayFlip.Add(neighbour, 0);
                            whitesThatMayFlip[neighbour]++;
                        }
                    }
                    if(blacksCount == 0 || blacksCount > 2)
                    {
                        blacksThatWillFlip.Add(tile.ToString());
                    }
                }
                foreach(string newWhite in blacksThatWillFlip)
                {
                    blacks.Remove(newWhite);
                }
                foreach(string newBlack in whitesThatMayFlip.Where(arg => arg.Value == 2).Select(arg => arg.Key))
                {
                    blacks.Add(newBlack);
                }
                //Console.WriteLine($"Day {day}: {blacks.Count}");
            }
            return blacks.Count;
        }

        private IEnumerable<string> Neighbours(string tile)
        {
            int[] coordinates = tile.Split(new[] { ' ' }).Select(v => int.Parse(v)).ToArray();
            return new[]
            {
                $"{coordinates[0] - 1} {coordinates[1] + 1} {coordinates[2]}",
                $"{coordinates[0]} {coordinates[1] + 1} {coordinates[2] - 1}",
                $"{coordinates[0] + 1} {coordinates[1]} {coordinates[2] - 1}",
                $"{coordinates[0] + 1} {coordinates[1] - 1} {coordinates[2]}",
                $"{coordinates[0]} {coordinates[1] - 1} {coordinates[2] + 1}",
                $"{coordinates[0] - 1} {coordinates[1]} {coordinates[2] + 1}"
            };
        }

        private class Coordinates
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public int Z { get; private set; }

            public Coordinates(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public override string ToString()
            {
                return $"{X} {Y} {Z}";
            }

            internal static Coordinates Parse(string line)
            {
                int x = 0;
                int y = 0;
                int z = 0;
                Regex directions = new Regex("(se|sw|ne|nw|e|w)");
                foreach(Match match in directions.Matches(line))
                {
                    switch (match.Value)
                    {
                        case "e":
                            x++;
                            y--;
                            break;
                        case "w":
                            x--;
                            y++;
                            break;
                        case "nw":
                            z--;
                            y++;
                            break;
                        case "se":
                            z++;
                            y--;
                            break;
                        case "ne":
                            x++;
                            z--;
                            break;
                        case "sw":
                            x--;
                            z++;
                            break;
                    }
                }
                return new Coordinates(x, y, z);
            }
        }
    }
}