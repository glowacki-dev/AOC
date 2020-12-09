using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day09 : ISolution<int>
    {
        int BufferSize = 25;
        public object Run(Input<int> lines)
        {
            List<int> buffer = new List<int>();
            foreach(int value in lines.Lines)
            {
                if(buffer.Count < BufferSize)
                {
                    buffer.Add(value);
                }
                else
                {
                    if(Validate(value, buffer))
                    {
                        buffer.Add(value);
                    }
                    else
                    {
                        //return value;
                        int sum = 0;
                        Queue<int> sumBuffer = new Queue<int>();
                        foreach (int num in buffer)
                        {
                            sumBuffer.Enqueue(num);
                            sum += num;
                            while (sum >= value)
                            {
                                if (sum == value) return sumBuffer.Min() + sumBuffer.Max();
                                sum -= sumBuffer.Dequeue();
                            }
                        }
                    }
                }
            }
            return null;
        }

        private bool Validate(int value, List<int> buffer)
        {
            for(int i = buffer.Count - BufferSize; i < buffer.Count; i++)
            {
                for(int j = buffer.Count - BufferSize; j < buffer.Count; j++)
                {
                    if (buffer[i] == buffer[j]) continue;
                    if (buffer[i] + buffer[j] == value) return true;
                }
            }
            return false;
        }
    }
}