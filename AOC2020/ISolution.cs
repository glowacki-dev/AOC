using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public interface ISolution<T>
    {
        object Run(IEnumerable<T> lines);
    }
}