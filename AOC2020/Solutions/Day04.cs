using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AOC2020
{
    internal class Day04 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            Passport passport = new Passport();
            int validPassports = 0;
            foreach(string line in lines.RawLines)
            {
                if(line == "")
                {
                    if (passport.Validate()) validPassports++;
                    passport = new Passport();
                }
                else
                {
                    foreach(string attribute in line.Split(' '))
                    {
                        string[] arguments = attribute.Split(':');
                        passport.LoadAttribute(arguments[0], arguments[1]);
                    }
                }
            }
            if (passport.Validate()) validPassports++;
            return validPassports;
        }

        private class Passport
        {
            private Dictionary<string, IAttributeValidator> KnownAttributes;
            private HashSet<string> OptionalAttributes;
            private Dictionary<string, string> Attributes;

            public Passport()
            {
                KnownAttributes = new Dictionary<string, IAttributeValidator>() {
                    { "byr", new NumberValidator(1920, 2002) },
                    { "iyr", new NumberValidator(2010, 2020) },
                    { "eyr", new NumberValidator(2020, 2030) },
                    { "hgt", new HeightValidator() },
                    { "hcl", new RegexValidator(@"^#[0-9a-f]{6}$") },
                    { "ecl", new ListValidator(new []{"amb", "blu", "brn", "gry", "grn", "hzl", "oth" }) },
                    { "pid", new RegexValidator(@"^\d{9}$") },
                    { "cid", new WateverValidator() }
                };
                OptionalAttributes = new HashSet<string>() { "cid" };
                Attributes = new Dictionary<string, string>();
            }

            internal void LoadAttribute(string attribute, string value)
            {
                Attributes[attribute] = value;
            }

            internal bool Validate()
            {
                foreach(var attribute in KnownAttributes)
                {
                    if (OptionalAttributes.Contains(attribute.Key)) continue;
                    if (!Attributes.ContainsKey(attribute.Key)) return false;
                    if (!attribute.Value.Validate(Attributes[attribute.Key])) return false;
                }
                return true;
            }

            private interface IAttributeValidator
            {
                bool Validate(string data);
            }

            private class NumberValidator : IAttributeValidator
            {
                private int v1;
                private int v2;

                public NumberValidator(int v1, int v2)
                {
                    this.v1 = v1;
                    this.v2 = v2;
                }

                public bool Validate(string data)
                {
                    int value = int.Parse(data);
                    return value >= v1 && value <= v2;
                }
            }

            private class RegexValidator : IAttributeValidator
            {
                private string v;

                public RegexValidator(string v)
                {
                    this.v = v;
                }

                public bool Validate(string data)
                {
                    return Regex.IsMatch(data, v);
                }
            }

            private class WateverValidator : IAttributeValidator
            {
                public bool Validate(string data)
                {
                    return true;
                }
            }

            private class HeightValidator : IAttributeValidator
            {
                public bool Validate(string data)
                {
                    Match match = Regex.Match(data, @"^(?<value>\d+)(?<unit>in|cm)$");
                    if (!match.Success) return false;

                    int value = 0;
                    switch (match.Groups["unit"].Value)
                    {
                        case "in":
                            value = int.Parse(match.Groups["value"].Value);
                            return value >= 59 && value <= 76;
                        case "cm":
                            value = int.Parse(match.Groups["value"].Value);
                            return value >= 150 && value <= 193;
                        default:
                            return false;
                    }
                }
            }

            private class ListValidator : IAttributeValidator
            {
                private HashSet<string> vs;

                public ListValidator(string[] vs)
                {
                    this.vs = new HashSet<string>(vs);
                }

                public bool Validate(string data)
                {
                    return vs.Contains(data);
                }
            }
        }
    }
}