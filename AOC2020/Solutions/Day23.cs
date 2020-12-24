using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day23 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            LinkedList<int> numbers = new LinkedList<int>(lines.Lines.First().ToCharArray().Select(c => int.Parse(c.ToString())));
            List<LinkedListNode<int>> hand = new List<LinkedListNode<int>>();
            for(int i = 10; i <= 1000000; i++) numbers.AddLast(i);
            Dictionary<int, LinkedListNode<int>> lookup = new Dictionary<int, LinkedListNode<int>>();
            for (LinkedListNode<int> cup = numbers.First; cup is LinkedListNode<int>; cup = cup.Next) lookup.Add(cup.Value, cup);
            LinkedListNode<int> current = numbers.First;
            for(int turn = 1; turn <= 10000000; turn++)
            {
                for (int i = 0; i < 3; i++)
                {
                    LinkedListNode<int> target = current.Next;
                    if (target == null) target = numbers.First;
                    hand.Add(target);
                    numbers.Remove(target);
                }

                int insertAfter = current.Value - 1;
                while (true)
                {
                    if (insertAfter < 1) insertAfter = 1000000;
                    if (!hand.Contains(lookup[insertAfter])) break;
                    insertAfter--;
                }

                LinkedListNode<int> preceeding = lookup[insertAfter];
                for(int i = 0; i < 3; i++)
                {
                    numbers.AddAfter(preceeding, hand[i]);
                    preceeding = hand[i];
                }
                hand.Clear();
                current = current.Next;
                if(current == null) current = numbers.First;
            }
            LinkedListNode<int> first = lookup[1];
            return (decimal)first.Next.Value * first.Next.Next.Value;
        }
    }
}