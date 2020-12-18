using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    internal class Day18 : ISolution<string>
    {
        public object Run(Input<string> lines)
        {
            return lines.Lines.Sum(line => new AltCalculator(line).Compute());
        }

        private class AltCalculator
        {
            readonly List<IOperand> query = new List<IOperand>();

            public AltCalculator(string line)
            {
                Stack<Operation> operators = new Stack<Operation>();
                query.Clear();
                foreach (char symbol in line)
                {
                    if (symbol == ' ') continue;
                    IOperand operand = ParseInstruction(symbol);
                    if (operand is Operation operation)
                    {
                        while (operators.Any() && operation.ShouldPop(operators.Peek())) query.Add(operators.Pop()); // Pop all operations with lower priority
                        if (operation is Parantheses parantheses && !parantheses.Opening) operators.Pop(); // Pop matching paranteheses (other operations already popped)
                        if (operation.Pushable) operators.Push(operation); // Don't push closing parantheses
                    }
                    else
                    {
                        query.Add(operand);
                    }
                }
                query.AddRange(operators);
            }

            internal decimal Compute()
            {
                Stack<decimal> buffer = new Stack<decimal>();
                foreach (IOperand operand in query)
                {
                    switch (operand)
                    {
                        case Number number:
                            buffer.Push(number.Value);
                            break;
                        case Operation operation:
                            buffer.Push(operation.Calculate(buffer.Pop(), buffer.Pop()));
                            break;
                    }
                }
                return buffer.Peek();
            }

            private IOperand ParseInstruction(char symbol)
            {
                switch (symbol)
                {
                    case '+':
                        return new Addition();
                    case '*':
                        return new Multiplication();
                    case '(':
                        return new Parantheses() { Opening = true };
                    case ')':
                        return new Parantheses() { Opening = false };
                    default:
                        return new Number(symbol);
                }
            }

            internal interface IOperand
            {
                int Value { get; }
            }

            internal class Number : IOperand
            {
                public int Value { get; private set; }

                public Number(char symbol)
                {
                    Value = int.Parse(symbol.ToString());
                }
            }

            internal class Operation : IOperand
            {
                public int Value { get { return Priority; } }
                public virtual int Priority { get; }
                public virtual bool Pushable { get { return true; } }

                internal virtual decimal Calculate(decimal v1, decimal v2)
                {
                    throw new NotImplementedException();
                }

                internal virtual bool ShouldPop(Operation stackOperation)
                {
                    return stackOperation.Priority >= Priority;
                }
            }

            internal class Addition : Operation
            {
                public override int Priority { get { return 1; } }
                internal override decimal Calculate(decimal v1, decimal v2)
                {
                    return v1 + v2;
                }
            }

            internal class Multiplication : Operation
            {
                public override int Priority { get { return 0; } }
                internal override decimal Calculate(decimal v1, decimal v2)
                {
                    return v1 * v2;
                }
            }

            internal class Parantheses : Operation
            {
                public override int Priority { get { return -100; } }
                public bool Opening { get; set; }
                public override bool Pushable { get { return Opening; } }

                internal override bool ShouldPop(Operation operation)
                {
                    if (Opening) return false;

                    return !(operation is Parantheses);
                }
            }
        }
    }
}