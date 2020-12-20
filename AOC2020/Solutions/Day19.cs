using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day19 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            Parser parser = new Parser();
            bool data = false;
            int validCount = 0;
            foreach(string line in lines.RawLines)
            {
                if(line == "")
                {
                    data = true;
                    continue;
                }

                if(data)
                {
                    if (parser.Validate(line))
                    {
                        Console.WriteLine($"[MATCH] {line}");
                        validCount++;
                    }
                }
                else
                {
                    parser.AddRule(line);
                }
            }
            return validCount;
        }

        private class Parser
        {
            private Dictionary<string, HashSet<string>> cykTable;
            private Dictionary<string, List<string>> producers;
            private Dictionary<string, HashSet<string>> terminals;

            public Parser()
            {
                terminals = new Dictionary<string, HashSet<string>>(); // terminal, rules
                producers = new Dictionary<string, List<string>>(); // production, [rule]
            }

            internal void AddRule(string line)
            {
                string[] parts = line.Split(new[] { ':' });
                string rule = parts[0];
                List<List<string>> productions = new List<List<string>>();
                parts = parts[1].Split(new[] { '|' }).Select(v => v.Trim()).ToArray();
                foreach(string part in parts)
                {
                    if (part.StartsWith("\""))
                    {
                        string symbol = part.Replace("\"", "");
                        if (!terminals.ContainsKey(symbol)) terminals.Add(symbol, new HashSet<string>());
                        terminals[symbol].Add(rule);
                        productions.Add(new List<string>() { symbol });
                    }
                    else
                    {
                        List<string> symbols = part.Split(new[] { ' ' }).ToList();
                        productions.Add(symbols);
                        if (!producers.ContainsKey(part)) producers.Add(part, new List<string>());
                        producers[part].Add(rule);
                    }
                }
            }

            internal bool Validate(string line)
            {
                cykTable = new Dictionary<string, HashSet<string>>();
                for(int i = 1; i <= line.Length; i++)
                {
                    for(int j = 1; j <= line.Length; j++)
                    {
                        cykTable.Add($"{i} {j}", new HashSet<string>());
                    }
                }
                for(int i = 1; i <= line.Length; i++)
                {
                    foreach(string rule in terminals[line[i - 1].ToString()])
                    {
                        cykTable[$"{i} 1"].Add(rule);
                    }
                }
                for(int i = 2; i <= line.Length; i++)
                {
                    for(int j = 1; j <= line.Length - i + 1; j++)
                    {
                        for(int k = 1; k <= i - 1; k++)
                        {
                            foreach(string s1 in cykTable[$"{j} {k}"])
                            {
                                foreach(string s2 in cykTable[$"{j + k} {i - k}"])
                                {
                                    if(producers.ContainsKey($"{s1} {s2}"))
                                    {
                                        foreach(string producer in producers[$"{s1} {s2}"])
                                        {
                                            cykTable[$"{j} {i}"].Add(producer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return cykTable[$"1 {line.Length}"].Contains("0");
            }
        }
    }
}