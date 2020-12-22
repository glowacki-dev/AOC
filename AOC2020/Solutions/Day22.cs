using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day22 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            List<int> p1 = new List<int>();
            List<int> p2 = new List<int>();
            bool next = false;
            foreach (string line in lines.RawLines)
            {
                if (line.StartsWith("Player")) continue;
                if (line == "")
                {
                    next = true;
                    continue;
                }
                int card = int.Parse(line);
                if (next)
                {
                    p2.Add(card);
                }
                else
                {
                    p1.Add(card);
                }
            }
            Game game = new Game(p1, p2);
            game.Play();
            return game.Score();
        }

        private class Game
        {
            private List<int> p1;
            private List<int> p2;
            private HashSet<string> history;

            public Game(List<int> p1, List<int> p2)
            {
                this.p1 = p1;
                this.p2 = p2;
                this.history = new HashSet<string>();
            }

            internal bool Play()
            {
                while (p1.Count > 0 && p2.Count > 0) PlayRound();
                return p2.Count == 0; // true if p1 is a winner
            }

            internal int Score()
            {
                List<int> winner = p1.Count == 0 ? p2 : p1;
                return winner.Select((card, index) => card * (winner.Count - index)).Sum();
            }

            private void PlayRound()
            {
                // Check for exact same configuration -> p1 wins
                string current = string.Join(" ", p1) + "|" + string.Join(" ", p2);
                if (history.Contains(current))
                {
                    p1.AddRange(p2);
                    p2.Clear();
                    return;
                }
                history.Add(current);
                // Regular rules
                int p1Card = p1[0];
                int p2Card = p2[0];
                p1.RemoveAt(0);
                p2.RemoveAt(0);
                if (p1.Count >= p1Card && p2.Count >= p2Card)
                {
                    // Play new recursive game
                    Game subGame = new Game(p1.GetRange(0, p1Card), p2.GetRange(0, p2Card));
                    if(subGame.Play())
                    {
                        p1.Add(p1Card);
                        p1.Add(p2Card);
                    }
                    else
                    {
                        p2.Add(p2Card);
                        p2.Add(p1Card);
                    }
                }
                else
                {
                    if (p1Card > p2Card)
                    {
                        p1.Add(p1Card);
                        p1.Add(p2Card);
                    }
                    else
                    {
                        p2.Add(p2Card);
                        p2.Add(p1Card);
                    }
                }
            }
        }
    }
}