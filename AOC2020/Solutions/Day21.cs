using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020
{
    internal class Day21 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            List<string> allIngredients = new List<string>();
            Dictionary<string, HashSet<string>> allergens = new Dictionary<string, HashSet<string>>();
            foreach(string line in lines.Lines)
            {
                Match match = Regex.Match(line, @"(?<ingredients>(\w* )+)\(contains (?<allergens>(\w*(, )?)+)\)");
                if (!match.Success) return "Can't parse input";
                List<string> possibleAllergens = match.Groups["allergens"].Value.Split(new[] { ',', ' ' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> currentIngredients = match.Groups["ingredients"].Value.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                allIngredients.AddRange(currentIngredients);
                foreach(string allergen in possibleAllergens)
                {
                    if (!allergens.ContainsKey(allergen)) allergens.Add(allergen, new HashSet<string>(currentIngredients));
                    else allergens[allergen].IntersectWith(currentIngredients);
                }
            }
            while(allergens.Any(allergen => allergen.Value.Count > 1))
            {
                foreach(string allergen in allergens.Keys)
                {
                    if (allergens[allergen].Count > 1) continue;
                    allIngredients.RemoveAll(ingredient => ingredient == allergens[allergen].First());
                    foreach (string otherAllergen in allergens.Keys.Except(new[] { allergen }))
                    {
                        allergens[otherAllergen].Remove(allergens[allergen].First());
                    }
                }
            }
            return string.Join(",", allergens.OrderBy(arg => arg.Key).Select(arg => arg.Value.First()));
        }
    }
}