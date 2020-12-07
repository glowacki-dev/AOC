using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day06 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            int groupAnswersCount = 0;
            string groupAnswers = "";
            int totalPositive = 0;
            foreach(string answers in lines.RawLines)
            {
                if(answers == "")
                {
                    totalPositive += groupAnswers.GroupBy(answer => answer).Where(group => group.Count() == groupAnswersCount).Count();
                    groupAnswers = "";
                    groupAnswersCount = 0;
                }
                else
                {
                    groupAnswersCount++;
                    groupAnswers += answers;
                }
            }
            totalPositive += groupAnswers.GroupBy(answer => answer).Where(group => group.Count() == groupAnswersCount).Count();
            return totalPositive;
        }
    }
}