using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Input<T>
    {
        public string Data { get; }
        public IEnumerable<T> Lines
        {
            get
            {
                return Data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(val => (T)Convert.ChangeType(val, typeof(T)));
            }
        }

        public IEnumerable<string> RawLines
        {
            get
            {
                return Data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            }
        }

        internal static Input<T> LoadFromFile(string dataSource)
        {
            string text;
            using (var streamReader = new StreamReader(dataSource, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            return new Input<T>(text);
        }

        public Input(string data)
        {
            Data = data.Trim();
        }
    }
}