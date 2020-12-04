using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    public enum DataType
    {
        sample,
        real
    }

    internal class Runner<T>
    {
        private string DataSource;
        private ISolution<T> Solver;
        const string solutionPath = "/Users/mg/Code/Personal/AOC2020/AOC2020/";

        internal void Run()
        {
            Console.WriteLine(Solver.GetType().Name);
            Console.WriteLine(Solver.Run(Input<T>.LoadFromFile(DataSource)));
        }

        public Runner(DataType type, ISolution<T> solver)
        {
            switch (type)
            {
                case DataType.real:
                    DataSource = solutionPath + "Data/" + solver.GetType().Name + ".txt";
                    break;
                case DataType.sample:
                    DataSource = solutionPath + "Samples/" + solver.GetType().Name + ".txt";
                    break;
                default:
                    break;
            }

            Solver = solver;
        }
    }
}