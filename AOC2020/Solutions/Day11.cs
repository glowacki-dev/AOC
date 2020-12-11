using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day11 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            string[] input = lines.Lines.ToArray();
            char[,] map = new char[input[0].Length, input.Length];
            Iterate(map, (x, y) => map[x, y] = input[y][x]);
            Simulator simulator = new Simulator(map);
            //PreviewMap(simulator);
            while (simulator.Simulate().Changes > 0)
            {
                //PreviewMap(simulator);
            }
            map = simulator.CurrentMap;
            int occupiedCount = 0;
            Iterate(simulator.CurrentMap, (x, y) => {
                if (simulator.CurrentMap[x, y] == '#') occupiedCount++;
                }
            );
            return occupiedCount;
        }

        private static void Iterate(char[,] map, Action<int, int> charAction, Action<int> rowAction = null)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    charAction.Invoke(x, y);
                }
                if (rowAction != null) rowAction.Invoke(y);
            }
        }

        private static void PreviewMap(Simulator simulator)
        {
            Iterate(simulator.CurrentMap,
                (x, y) => Console.Write(simulator.CurrentMap[x, y]),
                (y) => Console.WriteLine());
            Console.WriteLine();
        }

        private class Simulator
        {
            public Simulator(char[,] map)
            {
                CurrentMap = map;
                tempMap = new char[CurrentMap.GetLength(0), CurrentMap.GetLength(1)];
                Changes = 0;
            }

            char[,] tempMap;
            public int Changes { get; internal set; }
            public char[,] CurrentMap { get; internal set; }

            internal Simulator Simulate()
            {
                Changes = 0;
                Iterate(CurrentMap, (x, y) =>
                {
                    if (CurrentMap[x, y] == '.') tempMap[x, y] = '.';
                    else if (WillGetOccupied(x, y))
                    {
                        Changes++;
                        tempMap[x, y] = '#';
                    }
                    else if (WillGetEmpty(x, y))
                    {
                        Changes++;
                        tempMap[x, y] = 'L';
                    }
                    else tempMap[x, y] = CurrentMap[x, y];
                });
                CurrentMap = tempMap;
                tempMap = new char[CurrentMap.GetLength(0), CurrentMap.GetLength(1)];
                return this;
            }

            private bool WillGetEmpty(int x, int y)
            {
                return CurrentMap[x, y] == '#' && OccupiedAround(x, y) >= 5;
            }

            private bool WillGetOccupied(int x, int y)
            {
                return CurrentMap[x, y] == 'L' && OccupiedAround(x, y) == 0;
            }

            private int OccupiedAround(int x, int y)
            {
                int count = 0;
                List<Tuple<int, int>> directions = new List<Tuple<int, int>>{
                    Tuple.Create(-1, -1),
                    Tuple.Create(-1, 0),
                    Tuple.Create(-1, 1),
                    Tuple.Create(0, -1),
                    Tuple.Create(0, 1),
                    Tuple.Create(1, -1),
                    Tuple.Create(1, 0),
                    Tuple.Create(1, 1),
                };

                foreach(Tuple<int, int> direction in directions)
                {
                    int newX = x;
                    int newY = y;
                    while (true)
                    {
                        newX += direction.Item1;
                        newY += direction.Item2;
                        if (newX < 0 || newX >= CurrentMap.GetLength(0)) break;
                        if (newY < 0 || newY >= CurrentMap.GetLength(1)) break;
                        if (CurrentMap[newX, newY] == '#')
                        {
                            count++;
                            break;
                        }
                        if (CurrentMap[newX, newY] == 'L')
                        {
                            break;
                        }
                    }
                }
                return count;
            }
        }
    }
}