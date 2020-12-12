using System;
using System.Collections.Generic;
using System.Numerics;

namespace AOC2020
{
    internal class Day12 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            Vector2 position = new Vector2(0, 0);
            Vector2 waypoint = new Vector2(10, 1);
            foreach(string command in lines.Lines)
            {
                char action = command[0];
                int value = int.Parse(command.Substring(1));
                switch (action)
                {
                    case 'N':
                        waypoint += new Vector2(0, value);
                        break;
                    case 'S':
                        waypoint += new Vector2(0, -value);
                        break;
                    case 'E':
                        waypoint += new Vector2(value, 0);
                        break;
                    case 'W':
                        waypoint += new Vector2(-value, 0);
                        break;
                    case 'L':
                        waypoint = RotateByDegrees(waypoint, value);
                        break;
                    case 'R':
                        waypoint = RotateByDegrees(waypoint, -value);
                        break;
                    case 'F':
                        position += (waypoint * value);
                        break;
                    default:
                        break;
                }
            }
            return Math.Abs(Math.Round(position.X)) + Math.Abs(Math.Round(position.Y));
        }

        private Vector2 RotateByDegrees(Vector2 direction, int degrees)
        {
            double radians = degrees * Math.PI / 180;
            var ca = Math.Cos(radians);
            var sa = Math.Sin(radians);
            return new Vector2((float)(ca * direction.X - sa * direction.Y), (float)(sa * direction.X + ca * direction.Y));
        }
    }
}