using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            long part1Answer = 0;
            var part2Scores = new List<long>();

            foreach (var line in input)
            {
                var (invalidChars, unclosedChars) = ValidateLine(line);

                if (invalidChars.Length > 0)
                {
                    part1Answer += GetInvalidCharScore(invalidChars[0]);
                }
                else if (unclosedChars.Length > 0)
                {
                    long score = 0;

                    foreach (var c in unclosedChars)
                    {
                        score *= 5;
                        score += GetMissingCharScore(GetMatchingChar(c));
                    }

                    part2Scores.Add(score);
                }
            }

            Console.WriteLine($"Part 1: {part1Answer}");
            Console.WriteLine($"Part 2: {GetMedian(part2Scores)}");
            Console.Read();
        }

        private static long GetMedian(IEnumerable<long> scores)
        {
            var count = scores.Count();

            scores = scores.OrderBy(x => x);
            var middle = count / 2;

            return scores.ElementAt(middle);
        }

        private static int GetMissingCharScore(char c)
        {
            switch (c)
            {
                case ')':
                    return 1;
                case ']':
                    return 2;
                case '}':
                    return 3;
                case '>':
                    return 4;
                default:
                    throw new ArgumentException();
            }
        }

        private static int GetInvalidCharScore(char c)
        {
            switch (c)
            {
                case ')':
                    return 3;
                case ']':
                    return 57;
                case '}':
                    return 1197;
                case '>':
                    return 25137;
                default:
                    throw new ArgumentException();
            }
        }


        private static (char[] invalidChars, char[] unclosedChars) ValidateLine(string line)
        {
            var invalidChars = new List<char>();
            var openingChars = new Stack<char>();

            foreach (var c in line)
            {
                if (c.IsOpeningChar())
                {
                    openingChars.Push(c);
                }
                else
                {
                    var openingChar = openingChars.Pop();

                    if (openingChar != GetMatchingChar(c))
                    {
                        invalidChars.Add(c);
                    }
                }
            }

            return (invalidChars.ToArray(), openingChars.ToArray());
        }

        private static char GetMatchingChar(char c)
        {
            switch (c)
            {
                case '(':
                    return ')';
                case '[':
                    return ']';
                case '{':
                    return '}';
                case '<':
                    return '>';
                case ')':
                    return '(';
                case ']':
                    return '[';
                case '}':
                    return '{';
                case '>':
                    return '<';
                default:
                    throw new ArgumentException();
            }
        }
    }

    public static class Extensions
    {
        public static bool IsOpeningChar(this char c)
        {
            var openingChars = new List<char> { '(', '[', '{', '<' };
            return openingChars.Contains(c);
        }
    }
}
