using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day20 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            List<Tile> tiles = new List<Tile>();
            Tile tempTile = new Tile();
            foreach(string line in lines.RawLines)
            {
                if(line == "")
                {
                    tempTile.Prepare();
                    tiles.Add(tempTile);
                    tempTile = new Tile();
                    continue;
                }

                if (line.StartsWith("Tile"))
                {
                    tempTile.Number = int.Parse(line.Split(new[] { ' ', ':' })[1]);
                }
                else
                {
                    tempTile.AddRow(line);
                }
            }
            tempTile.Prepare();
            tiles.Add(tempTile);
            return tiles.Where(tile => tile.PossibleNeighbours(tiles).Count == 2).Select(tile => tile.Number).Aggregate<int, decimal>(1, (acc, num) => acc * num);
        }

        private class Tile
        {
            public Tile()
            {
                Rows = new List<string>();
                Borders = new string[4];
            }

            public int Number { get; internal set; }
            private List<string> Rows { get; }
            private string[] Borders;

            internal void AddRow(string line)
            {
                Rows.Add(line);
            }

            internal void Prepare()
            {
                Borders[0] = Rows[0];
                Borders[1] = string.Join("", Rows.Select(r => r.ToCharArray().Last()));
                Borders[2] = Rows.Last();
                Borders[3] = string.Join("", Rows.Select(r => r.ToCharArray()[0]));
            }

            internal List<Tile> PossibleNeighbours(List<Tile> tiles)
            {
                List<Tile> neighbours = new List<Tile>();
                foreach(Tile tile in tiles)
                {
                    if (tile == this) continue;

                    foreach(string pattern in Borders)
                    {
                        if (tile.PossibleMatches(pattern) > 0)
                        {
                            neighbours.Add(tile);
                            break;
                        }
                    }
                }
                return neighbours;
            }

            private int PossibleMatches(string pattern)
            {
                int matches = 0;
                foreach(string border in Borders)
                {
                    if (pattern == border) matches++;
                    if (pattern == string.Join("", border.ToCharArray().Reverse())) matches++;
                }
                return matches;
            }
        }
    }
}