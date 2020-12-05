using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day05 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            Dictionary<int, HashSet<int>> seatsMap = new Dictionary<int, HashSet<int>>();
            int lowestRow = int.MaxValue;
            int highestRow = int.MinValue;
            foreach(Ticket ticket in lines.Lines.OrderBy(ticketName => ticketName).Select(ticketNumber => new Ticket(ticketNumber)))
            {
                if (!seatsMap.ContainsKey(ticket.Row))
                {
                    if (ticket.Row > highestRow) highestRow = ticket.Row;
                    if (ticket.Row < lowestRow) lowestRow = ticket.Row;
                    seatsMap[ticket.Row] = new HashSet<int>(new[] { 0, 1, 2, 3, 4, 5, 6, 7 });
                }
                seatsMap[ticket.Row].Remove(ticket.Seat);
            }
            seatsMap.Remove(highestRow);
            seatsMap.Remove(lowestRow);
            var location = seatsMap.First(entry => entry.Value.Count > 0);
            return new Ticket(location.Key, location.Value.First()).GetChecksum();
        }

        private class Ticket
        {
            private string seatName;

            public Ticket(string seatName)
            {
                this.seatName = seatName;
                Process();
            }

            public Ticket(int currentRow, int freeSeat)
            {
                Row = currentRow;
                Seat = freeSeat;
            }

            public int Row { get; private set; }
            public int Seat { get; private set; }

            internal int GetChecksum()
            {
                return Row * 8 + Seat;
            }

            private void Process()
            {
                int rowLow = 0;
                int rowHigh = 127;
                int seatLow = 0;
                int seatHigh = 7;
                foreach(char component in seatName)
                {
                    switch (component)
                    {
                        case 'F':
                            rowHigh -= (int)Math.Ceiling((rowHigh - rowLow) / 2.0d);
                            break;
                        case 'B':
                            rowLow += (int)Math.Ceiling((rowHigh - rowLow) / 2.0d);
                            break;
                        case 'L':
                            seatHigh -= (int)Math.Ceiling((seatHigh - seatLow) / 2.0d);
                            break;
                        case 'R':
                            seatLow += (int)Math.Ceiling((seatHigh - seatLow) / 2.0d);
                            break;
                        default:
                            break;
                    }
                }
                Row = rowLow;
                Seat = seatLow;
            }
        }
    }
}