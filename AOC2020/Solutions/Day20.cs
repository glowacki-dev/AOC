using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            int allTilesCount = tiles.Count;
            int mapSize = (int)Math.Sqrt(allTilesCount);
            Tile[,] map = new Tile[mapSize, mapSize];
            List<Tile> firstTile = tiles.Where(tile => tile.PossibleNeighbours(tiles).Count == 2).Where(tile => tile.PossibleNeighbours(tiles, 1).Count == 1).Where(tile => tile.PossibleNeighbours(tiles, 2).Count == 1).ToList();
            if (firstTile.Count != 1) return "Can't find first tile";
            map[0, 0] = firstTile[0];
            tiles.Remove(firstTile[0]);
            while (tiles.Count > 0)
            {
                for(int x = 0; x < mapSize; x++)
                {
                    for(int y = 0; y < mapSize; y++)
                    {
                        if (map[x, y] != null) continue;
                        List<Tile> fromLeft = tiles;
                        List<Tile> fromRight = tiles;
                        List<Tile> fromDown = tiles;
                        List<Tile> fromUp = tiles; // all are possible until overriden below - this makes intersection easier
                        string up = null, right = null, down = null, left = null;
                        if (x > 0 && map[x - 1, y] != null)
                        {
                            left = map[x - 1, y].Right();
                            fromLeft = map[x - 1, y].PossibleNeighbours(tiles, 1);
                        }
                        if (y > 0 && map[x, y - 1] != null)
                        {
                            up = map[x, y - 1].Down();
                            fromUp = map[x, y - 1].PossibleNeighbours(tiles, 2);
                        }
                        if(x < mapSize - 1 && map[x + 1, y] != null)
                        {
                            right = map[x + 1, y].Left();
                            fromRight = map[x + 1, y].PossibleNeighbours(tiles, 3);
                        }
                        if (y < mapSize - 1 && map[x, y + 1] != null)
                        {
                            down = map[x, y + 1].Up();
                            fromDown = map[x, y + 1].PossibleNeighbours(tiles, 0);
                        }
                        List<Tile> possibleTiles = fromLeft.Intersect(fromUp).Intersect(fromRight).Intersect(fromDown).ToList();
                        if (possibleTiles.Count != 1) continue;

                        Tile possibleTile = possibleTiles[0];

                        if (!possibleTile.RotateToMatch(up, right, left, down)) return "Couldn't rotate.";

                        map[x, y] = possibleTile;
                        tiles.Remove(possibleTile);
                    }
                }
            }
            int megaMapSize = mapSize * (map[0, 0].Rows.Count - 2);
            char[,] megaMap = new char[megaMapSize, megaMapSize];
            for(int x = 0; x < megaMapSize; x++)
            {
                for(int y = 0; y < megaMapSize; y++)
                {
                    int tileX = x / 8;
                    int tileY = y / 8;
                    megaMap[x, y] = map[tileX, tileY].GetPixel(x % 8, y % 8);
                }
            }
            MonsterFinder finder = new MonsterFinder(megaMap, megaMapSize);
            finder.Locate();
            return finder.Score();
        }

        private class Tile
        {
            public Tile()
            {
                Rows = new List<string>();
            }

            public int Number { get; internal set; }
            public List<string> Rows { get; set; }
            public char[,] Pixels { get; set; }

            internal void AddRow(string line)
            {
                Rows.Add(line);
            }

            internal void Prepare()
            {
                int size = Rows.Count;
                Pixels = new char[size, size];
                for(int x = 0; x < size; x++)
                {
                    for(int y = 0; y < size; y++)
                    {
                        Pixels[x, y] = Rows[y][x];
                    }
                }
            }

            public string Up()
            {
                string border = "";
                for(int i = 0; i < Rows.Count; i++) border += Pixels[i, 0];
                return border;
            }

            public string Down()
            {
                string border = "";
                for (int i = 0; i < Rows.Count; i++) border += Pixels[i, Rows.Count - 1];
                return border;
            }

            public string Left()
            {
                string border = "";
                for (int i = 0; i < Rows.Count; i++) border += Pixels[0, i];
                return border;
            }

            public string Right()
            {
                string border = "";
                for (int i = 0; i < Rows.Count; i++) border += Pixels[Rows.Count - 1, i];
                return border;
            }

            internal List<Tile> PossibleNeighbours(List<Tile> tiles)
            {
                List<Tile> neighbours = new List<Tile>();
                for (int i = 0; i < 4; i++) neighbours.AddRange(PossibleNeighbours(tiles, i));
                return neighbours.Distinct().ToList();
            }

            private int PossibleMatches(string pattern)
            {
                int matches = 0;
                foreach(string border in new[] { Up(), Down(), Left(), Right() })
                {
                    if (pattern == border) matches++;
                    if (pattern == string.Join("", border.ToCharArray().Reverse())) matches++;
                }
                return matches;
            }

            internal List<Tile> PossibleNeighbours(List<Tile> tiles, int direction)
            {
                List<Tile> neighbours = new List<Tile>();
                string pattern = "";
                switch (direction)
                {
                    case 0:
                        pattern = Up();
                        break;
                    case 1:
                        pattern = Right();
                        break;
                    case 2:
                        pattern = Down();
                        break;
                    case 3:
                        pattern = Left();
                        break;
                }
                foreach (Tile tile in tiles)
                {
                    if (tile == this) continue;

                    if (tile.PossibleMatches(pattern) > 0)
                    {
                        neighbours.Add(tile);
                    }
                }
                return neighbours;
            }

            internal bool RotateToMatch(string up, string right, string left, string down)
            {
                for(int i = 0; i < 4; i++) // try every rotation
                {
                    if (Matches(up, right, left, down)) return true;
                    Rotate();
                }
                // flip
                Flip();
                for (int i = 0; i < 4; i++) // try every rotation again
                {
                    if (Matches(up, right, left, down)) return true;
                    Rotate();
                }
                return false;
            }

            private bool Matches(string up, string right, string left, string down)
            {
                return (up == null || Up() == up) &&
                    (right == null || Right() == right) &&
                    (down == null || Down() == down) &&
                    (left == null || Left() == left);
            }

            private void Flip()
            {
                char[,] newMap = new char[Rows.Count, Rows.Count];
                for(int x = 0; x < Rows.Count; x++)
                {
                    for (int y = 0; y < Rows.Count; y++)
                    {
                        newMap[x, y] = Pixels[x, Rows.Count - 1 - y];
                    }
                }
                Pixels = newMap;
            }

            private void Rotate()
            {
                char[,] newMap = new char[Rows.Count, Rows.Count];
                for (int x = 0; x < Rows.Count; x++)
                {
                    for (int y = 0; y < Rows.Count; y++)
                    {
                        newMap[x, y] = Pixels[y, Rows.Count - 1 - x];
                    }
                }
                Pixels = newMap;
            }

            // Return a single pixel value, without borders
            internal char GetPixel(int x, int y)
            {
                return Pixels[x + 1, y + 1];
            }
        }

        private class MonsterFinder
        {
            private char[,] megaMap;
            private int size;

            public MonsterFinder(char[,] megaMap, int size)
            {
                this.megaMap = megaMap;
                this.size = size;
            }

            private void Flip()
            {
                char[,] newMap = new char[size, size];
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        newMap[x, y] = megaMap[x, size - 1 - y];
                    }
                }
                megaMap = newMap;
            }

            private void Rotate()
            {
                char[,] newMap = new char[size, size];
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        newMap[x, y] = megaMap[y, size - 1 - x];
                    }
                }
                megaMap = newMap;
            }

            internal int Score()
            {
                string mapText = "";
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        mapText += megaMap[x, y];
                    }
                    mapText += "\n";
                }
                int count = 0;
                foreach (Match middle in Regex.Matches(mapText, "(?<pre>.*)(?<middle>#....##....##....###)(?<post>.*)"))
                {
                    int offset = middle.Groups["middle"].Index % (size + 1); //remember aobut newlines
                    Match monsterMatch = Regex.Match(mapText, "(?<pre>.{" + offset + "})..................#.(?<post>.*)\n(?<pre2>.{" + offset + "})#....##....##....###(?<post2>.*)\n(?<pre3>.{" + offset + "}).#..#..#..#..#..#...(?<post3>.*)", RegexOptions.Multiline);
                    if (monsterMatch.Success)
                        count++;
                }
                return mapText.Count(c => c =='#') - (count * 15);
            }

            internal void Locate()
            {
                for (int i = 0; i < 4; i++) // try every rotation
                {
                    if (GotMonster()) return;
                    Rotate();
                }
                // flip
                Flip();
                for (int i = 0; i < 4; i++) // try every rotation again
                {
                    if (GotMonster()) return;
                    Rotate();
                }
            }

            private bool GotMonster()
            {
                string mapText = "";
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        mapText += megaMap[x, y];
                    }
                    mapText += "\n";
                }
                foreach(Match middle in Regex.Matches(mapText, "(?<pre>.*)(?<middle>#....##....##....###)(?<post>.*)"))
                {
                    int offset = middle.Groups["middle"].Index % (size + 1); //remember aobut newlines
                    Match monsterMatch = Regex.Match(mapText, "(?<pre>.{" + offset + "})..................#.(?<post>.*)\n(?<pre2>.{" + offset + "})#....##....##....###(?<post2>.*)\n(?<pre3>.{" + offset + "}).#..#..#..#..#..#...(?<post3>.*)", RegexOptions.Multiline);
                    if (monsterMatch.Success)
                        return true;
                }
                return false;
            }
        }
    }
}