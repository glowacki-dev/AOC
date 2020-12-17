using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AOC2020
{
    internal class Day17 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            string[] input = lines.Lines.ToArray();
            Dictionary<Vector4, bool> map = new Dictionary<Vector4, bool>();
            for(int x = 0; x < input.Length; x++)
            {
                for (int y = 0; y < input.Length; y++)
                {
                    map[new Vector4(x, y, 0, 0)] = input[y][x] == '#';
                }
            }
            Simulator simulator = new Simulator(map);
            //PreviewMap(simulator);
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine(i + 1);
                simulator.Simulate();
                //PreviewMap(simulator);
            }
            int activeCubes = 0;
            Iterate(simulator.CurrentMap, (position) =>
            {
                if (simulator.CurrentMap[position] == true) activeCubes++;
            });
            return activeCubes;
        }

        private static void Iterate(Dictionary<Vector4, bool> map, Action<Vector4> cubeAction, Action<int> rowAction = null)
        {
            foreach(Vector4 position in map.Keys)
            {
                cubeAction.Invoke(position);
            }
        }

        private static void PreviewMap(Simulator simulator)
        {
            List<Vector4> positions = simulator.CurrentMap.Keys.OrderBy(v => v.X).OrderBy(v => v.Y).OrderBy(v => v.Z).OrderBy(v => v.W).ToList();
            float currentW = positions[0].W;
            float currentZ = positions[0].Z;
            float currentY = positions[0].Y;
            foreach(Vector4 position in positions)
            {
                if (position.W != currentW) Console.WriteLine();
                if (position.Z != currentZ) Console.WriteLine();
                if (position.Y != currentY) Console.WriteLine();
                Console.Write(simulator.CurrentMap[position] ? "#" : ".");
                currentY = position.Y;
                currentZ = position.Z;
                currentW = position.W;
            }
            Console.WriteLine();
        }

        private class Simulator
        {
            public Simulator(Dictionary<Vector4, bool> map)
            {
                CurrentMap = map;
                tempMap = new Dictionary<Vector4, bool>();
                Changes = 0;
                directions = new List<Vector4>();
                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                        for (int z = -1; z <= 1; z++)
                            for (int w = -1; w <= 1; w++)
                                directions.Add(new Vector4(x, y, z, w));
                directions.Remove(new Vector4(0, 0, 0, 0));
            }

            Dictionary<Vector4, bool> tempMap;
            public int Changes { get; internal set; }
            public Dictionary<Vector4, bool> CurrentMap { get; internal set; }
            List<Vector4> directions;

            internal Simulator Simulate()
            {
                Changes = 0;
                Iterate(CurrentMap, (position) =>
                {
                    //ExpandAround(tempMap, position);
                    if (CurrentMap[position]) ExpandAround(tempMap, position);
                });
                CurrentMap = tempMap;
                tempMap = new Dictionary<Vector4, bool>();
                Iterate(CurrentMap, (position) =>
                {
                    if (CurrentMap[position])
                    {
                        if (WillStayActive(position))
                        {
                            tempMap[position] = true;
                        }
                        else
                        {
                            tempMap[position] = false;
                            Changes++;
                        }
                    }
                    else
                    {
                        if (WillGetActive(position))
                        {
                            tempMap[position] = true;
                            Changes++;
                        }
                        else
                        {
                            tempMap[position] = false;
                        }
                    }
                });
                CurrentMap = tempMap;
                tempMap = new Dictionary<Vector4, bool>();
                return this;
            }

            private bool WillGetActive(Vector4 position)
            {
                int neighbours = ActiveAround(position);
                return neighbours == 3;
            }

            private bool WillStayActive(Vector4 position)
            {
                int neighbours = ActiveAround(position);
                return neighbours == 2 || neighbours == 3;
            }

            private void ExpandAround(Dictionary<Vector4, bool> map, Vector4 position)
            {
                map[position] = CurrentMap[position];
                foreach(Vector4 direction in directions)
                {
                    Vector4 newPosition = direction + position;
                    if (map.ContainsKey(newPosition)) continue;
                    map[newPosition] = false;
                }
            }

            private int ActiveAround(Vector4 position)
            {
                int count = 0;

                foreach (Vector4 direction in directions)
                {
                    Vector4 newPosition = direction + position;
                    if (CurrentMap.ContainsKey(newPosition) && CurrentMap[newPosition]) count++;
                }
                return count;
            }
        }
    }
}