using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var snwer = Run(input);
        }

        public static (long, long) Run(string[] input)
        {
            IEnumerable<Snail> Parse(string line)
            {
                int level = 0;
                foreach (var c in line)
                {
                    level += c == '[' ? 1 : c == ']' ? -1 : 0;
                    if (char.IsDigit(c))
                        yield return new Snail(int.Parse(c.ToString()), level);
                }
            }

            LinkedListNode<Snail> Collapse(LinkedList<Snail> num, LinkedListNode<Snail> node, int value)
            {
                var newNode = new LinkedListNode<Snail>(new Snail(value, node.Value.Level - 1));
                num.AddBefore(node, newNode);
                num.Remove(node.Next);
                num.Remove(node);
                return newNode;
            }

            LinkedList<Snail> Reduce(LinkedList<Snail> num)
            {
                Snail Snail;
                do
                {
                    Snail = num.FirstOrDefault(n => n.Level > 4);

                    if (Snail != null)
                    {
                        var node = num.Find(Snail);
                        if (node.Previous != null)
                            node.Previous.Value.Value += node.Value.Value;

                        if (node.Next.Next != null)
                            node.Next.Next.Value.Value += node.Next.Value.Value;

                        node = Collapse(num, node, 0);
                        continue;
                    }

                    Snail = num.FirstOrDefault(n => n.Value > 9);

                    if (Snail != null)
                    {
                        var node = num.Find(Snail);
                        num.AddBefore(node, new Snail(node.Value.Value / 2, node.Value.Level + 1));
                        num.AddBefore(node, new Snail(node.Value.Value - (node.Value.Value / 2), node.Value.Level + 1));
                        num.Remove(node);
                    }
                } while (Snail != null);

                return num;
            }

            LinkedList<Snail> Add(LinkedList<Snail> num, LinkedList<Snail> next)
              => Reduce(new LinkedList<Snail>(num.Union(next).Select(s => new Snail(s.Value, s.Level + 1))));

            long Magnitude(LinkedList<Snail> num)
            {
                while (num.Count > 1)
                {
                    var level = num.Max(n => n.Level);
                    for (var node = num.First; node != null && node.Next != null; node = node.Next)
                        if (node.Value.Level == node.Next.Value.Level && node.Value.Level == level)
                            node = Collapse(num, node, node.Value.Value * 3 + node.Next.Value.Value * 2);
                }
                return num.First.Value.Value;
            }

            var nums = input.Select(line => new LinkedList<Snail>(Parse(line))).ToList();
            var sum = nums.Skip(1).Aggregate(nums.First(), (acc, next) => Add(acc, next));

            var sums = from a in nums
                       from b in nums
                       where a != b
                       select Magnitude(Add(a, b));

            return (Magnitude(sum), sums.Max());
        }
    }

    public class Snail
    {
        public Snail(int value, int level)
        {
            Value = value;
            Level = level;
        }

        public int Value { get; set; }
        public int Level { get; set; }
    }
}
