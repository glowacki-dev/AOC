using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day03 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            var map = new List<InfiniteList<char>>();
            foreach(string line in lines.Lines)
            {
                map.Add(new InfiniteList<char>(line));
            }

            List<int> trees = new List<int>()
            {
                PathTreesCount(map, 1, 1),
                PathTreesCount(map, 3, 1),
                PathTreesCount(map, 5, 1),
                PathTreesCount(map, 7, 1),
                PathTreesCount(map, 1, 2)
            };

            return trees.Aggregate(1.0, (acc, val) => acc *= val);
        }

        private int PathTreesCount(List<InfiniteList<char>> map, int xStep, int yStep)
        {
            int trees = 0;
            int x = 0;
            for(int y = 0; y < map.Count; y += yStep)
            {
                if (map[y][x] == '#') trees++;
                x += xStep;
            }
            return trees;
        }

        private class InfiniteList<T>
        {
            private List<T> line;

            public InfiniteList(IEnumerable<T> line)
            {
                this.line = new List<T>(line);
            }

            public T this[int index]
            {
                get
                {
                    return line[index % line.Count];
                }
            }
        }
    }
}