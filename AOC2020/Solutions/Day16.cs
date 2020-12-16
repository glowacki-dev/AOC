using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020
{
    internal class Day16 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            int section = 0;
            Validator validator = new Validator();
            int errorRate = 0;
            List<Ticket> tickets = new List<Ticket>();
            foreach(string line in lines.RawLines)
            {
                if(line == "")
                {
                    section++;
                    continue;
                }
                switch(section)
                {
                    case 0:
                        validator.ParseRule(line);
                        //definitions
                        break;
                    case 1:
                        if (line.StartsWith("your ticket")) continue;
                        tickets.Add(Ticket.Parse(line));
                        break;
                    case 2:
                        if (line.StartsWith("nearby tickets")) continue;
                        Ticket ticket = Ticket.Parse(line);
                        if (validator.Validate(ticket)) tickets.Add(ticket);
                        break;
                }
            }
            validator.ResetMatchingEngine();
            foreach(Ticket ticket in tickets)
            {
                validator.MatchRules(ticket);
            }
            return validator.Checksum(tickets[0]);
        }

        private class Validator
        {
            private List<Rule> Rules;
            private List<List<Rule>> PossibleRules;

            public Validator()
            {
                Rules = new List<Rule>();
            }

            internal decimal Checksum(Ticket ticket)
            {
                Cleanup();
                decimal checksum = 1;
                for(int i = 0; i < PossibleRules.Count; i++)
                {
                    Console.WriteLine($"Field {ticket.Values[i]} can be {string.Join(",", PossibleRules[i].Select(r => r.Name))}");
                    if (!PossibleRules[i][0].Name.StartsWith("departure")) continue;
                    checksum *= ticket.Values[i];
                }
                return checksum;
            }

            private void Cleanup()
            {
                // If there is only one rule left for a field it can't be used anywhere else
                bool changes = true;
                while (changes)
                {
                    changes = false;
                    for (int j = 0; j < PossibleRules.Count; j++)
                    {
                        if (PossibleRules[j].Count > 1) continue;

                        for (int i = 0; i < PossibleRules.Count; i++)
                        {
                            if (i == j) continue;
                            if (PossibleRules[i].Remove(PossibleRules[j][0])) changes = true;
                        }
                    }
                }
            }

            internal void MatchRules(Ticket ticket)
            {
                int fieldNumber = 0;
                foreach (int value in ticket.Values)
                {
                    List<Rule> badRules = new List<Rule>();
                    // Try to mach every rule for this field
                    foreach(Rule rule in PossibleRules[fieldNumber])
                    {
                        if (!rule.Covers(value)) badRules.Add(rule);
                    }
                    // Remove all that didn't match (they can't be for this field)
                    foreach(Rule rule in badRules)
                    {
                        PossibleRules[fieldNumber].Remove(rule);
                    }
                    fieldNumber++;
                }
                Cleanup();
            }

            internal void ResetMatchingEngine()
            {
                PossibleRules = new List<List<Rule>>();
                // Every rule is posible for every field
                foreach(Rule rule in Rules)
                {
                    PossibleRules.Add(new List<Rule>(Rules));
                }
            }

            internal void ParseRule(string line)
            {
                string ruleName = line.Split(new[] { ':' }).First();
                Rule rule = new Rule(ruleName);
                foreach (Match match in Regex.Matches(line, @"(\d+-\d+)"))
                {
                    int[] values = match.Value.Split(new[] { '-' }).Select(val => int.Parse(val)).ToArray();
                    rule.AddRange(values[0], values[1]);
                }
                Rules.Add(rule);
            }

            internal bool Validate(Ticket ticket)
            {
                foreach(int value in ticket.Values)
                {
                    if (!Rules.Any(rule => rule.Covers(value))) return false;
                }
                return true;
            }

            private class Rule
            {
                public string Name { get; }

                private List<Tuple<int, int>> Ranges;

                public Rule(string name)
                {
                    Name = name;
                    Ranges = new List<Tuple<int, int>>();
                }

                internal void AddRange(int v1, int v2)
                {
                    Ranges.Add(new Tuple<int, int>(v1, v2));
                }

                internal bool Covers(int value)
                {
                    foreach(Tuple<int, int> range in Ranges)
                    {
                        if (value >= range.Item1 && value <= range.Item2) return true;
                    }
                    return false;
                }
            }
        }

        private class Ticket
        {
            public Ticket(IEnumerable<int> values)
            {
                Values = values.ToList();
            }

            public List<int> Values { get; }

            internal static Ticket Parse(string line)
            {
                return new Ticket(line.Split(new[] { ',' }).Select(val => int.Parse(val)));
            }
        }
    }
}