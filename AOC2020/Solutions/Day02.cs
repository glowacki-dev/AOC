using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day02 : ISolution<string>
    {
        public object Run(IEnumerable<string> lines)
        {
            int validPasswords = 0;
            foreach(string line in lines)
            {
                string[] entry = line.Split(new[] { ':', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
                if(new PasswordValidator(entry).IsValid()) validPasswords++;
            }
            return validPasswords;
        }

        private class PasswordValidator
        {
            private int lowerBound;
            private int upperBound;
            private char letter;
            private string password;

            public PasswordValidator(string[] entry)
            {
                lowerBound = int.Parse(entry[0]);
                upperBound = int.Parse(entry[1]);
                letter = entry[2][0];
                password = entry[3];
            }

            internal bool IsValid()
            {
                char firstLetter = password[lowerBound - 1];
                char secondLetter = password[upperBound - 1];
                return firstLetter != secondLetter && (firstLetter == letter || secondLetter == letter);
            }
        }
    }
}