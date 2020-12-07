using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020
{
    internal class Day07 : ISolution<string>
    {
        Dictionary<string, List<string>> containedIn;
        Dictionary<string, List<Tuple<int, string>>> contains;
        Dictionary<string, int> childrenCountCache = new Dictionary<string, int>();

        public object Run(Input<string> lines)
        {
            containedIn = new Dictionary<string, List<string>>();
            contains = new Dictionary<string, List<Tuple<int, string>>>();
            foreach (string line in lines.Lines)
            {
                Match match = Regex.Match(line.Replace(" ", "").Replace(".", ""), @"(?<outer>\w+\w+)bagscontain(?<inner>.*)");
                if (!match.Success) return "Can't parse " + line;

                string outer = match.Groups["outer"].Value;
                contains.Add(outer, new List<Tuple<int, string>>());

                foreach (string inner in match.Groups["inner"].Value.Split(','))
                {
                    if (inner == "nootherbags") continue;
                    match = Regex.Match(inner, @"(?<count>\d+)(?<color>\w+)bag(s?)");

                    if (!containedIn.ContainsKey(match.Groups["color"].Value)) containedIn.Add(match.Groups["color"].Value, new List<string>());
                    containedIn[match.Groups["color"].Value].Add(outer);

                    if (!contains.ContainsKey(outer)) contains.Add(outer, new List<Tuple<int, string>>());
                    contains[outer].Add(new Tuple<int, string>(int.Parse(match.Groups["count"].Value), match.Groups["color"].Value));
                }
            }
            
            return ChildrenCount("shinygold") - 1; //exclude outermost
        }

        private int ChildrenCount(string v)
        {
            if (childrenCountCache.ContainsKey(v)) return childrenCountCache[v];

            int childrenCount = contains[v].Sum(child => child.Item1 * ChildrenCount(child.Item2)) + 1; // include itself
            childrenCountCache[v] = childrenCount;
            return childrenCount;
        }

        private int ParentsCount(string v)
        {
            HashSet<string> processedColors = new HashSet<string>();
            Queue<string> tempColors = new Queue<string>(new []{ v });
            while(tempColors.Count > 0)
            {
                string color = tempColors.Dequeue();
                if (processedColors.Contains(color)) continue;
                processedColors.Add(color);

                if (!containedIn.ContainsKey(color)) continue; // Outermost color
                foreach(string parent in containedIn[color])
                {
                    if (processedColors.Contains(parent)) continue;
                    tempColors.Enqueue(parent);
                }
            }
            processedColors.Remove(v);
            return processedColors.Count;
        }
    }
}