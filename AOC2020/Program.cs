using System;

namespace AOC2020
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //new Runner<int>(DataType.real, new Day01()).Run();
            new Runner<string>(DataType.real, new Day02()).Run();
        }
    }
}
