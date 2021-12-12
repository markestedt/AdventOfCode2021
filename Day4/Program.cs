using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var numbers = input[0].Split(',').Select(int.Parse);

            var boards = GetBoards(input.Skip(1).ToArray());
            Part1(boards, numbers);
            Part2(boards, numbers);


            Console.ReadLine();
        }

        public static void Part2(IEnumerable<Board> boards, IEnumerable<int> numbers)
        {
            Board winningBoard = null;
            int lastNumberCalled = 0;

            var boardList = boards.ToList();

            foreach (var number in numbers)
            {
                foreach (var board in boardList)
                {
                    board.MarkNumber(number);
                }

                if (winningBoard == null)
                {
                    if (boardList.Any(b => b.HasBingo()))
                    {
                        boardList.RemoveAll(b => b.HasBingo());
                        if (boardList.Count == 1)
                        {
                            winningBoard = boardList.First();
                        }
                    }
                }
                else
                {
                    if (winningBoard.HasBingo())
                    {
                        lastNumberCalled = number;
                        break;
                    }
                }
            }
            var sumUnMarked = winningBoard.Rows.SelectMany(r => r.Numbers.Where(x => !x.Marked).Select(x => x.Value)).Sum();
            Console.WriteLine($"Part2: {sumUnMarked * lastNumberCalled}");
        }

        public static void Part1(IEnumerable<Board> boards, IEnumerable<int> numbers)
        {
            Board winningBoard = null;
            int lastNumberCalled = 0;

            foreach (var number in numbers)
            {
                foreach (var board in boards)
                {
                    board.MarkNumber(number);
                }

                if (boards.Any(b => b.HasBingo()))
                {
                    lastNumberCalled = number;
                    winningBoard = boards.Single(b => b.HasBingo());
                    break;
                }
            }

            var sumUnMarked = winningBoard.Rows.SelectMany(r => r.Numbers.Where(x => !x.Marked).Select(x => x.Value)).Sum();
            Console.WriteLine($"Part1: {sumUnMarked * lastNumberCalled}");
        }


        public static IEnumerable<Board> GetBoards(string[] input)
        {
            var boards = new List<Board>();
            var board = new Board();

            var rowNr = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (board.Rows.Count() > 4)
                {
                    boards.Add(board);
                    board = new Board();
                    rowNr = 0;
                }

                var rowNumbers = input[i].Trim().Replace("  ", " ").Split(' ').Select(x => int.Parse(x.Trim()));
                var row = new Row
                {
                    RowNr = rowNr,
                    Numbers = rowNumbers.Select(x => new Number { Value = x }).ToList()
                };
                board.Rows.Add(row);
            }

            return boards;
        }
    }

    public class Board
    {
        public List<Row> Rows = new List<Row>();
        public void MarkNumber(int number)
        {
            foreach (var row in Rows)
            {
                row.Numbers.Where(x => x.Value == number).ToList().ForEach(x => x.Marked = true);
            }
        }
        public bool HasBingo()
        {
            if (Rows.Any(r => r.Numbers.All(n => n.Marked)))
            {
                return true;
            }

            for (int i = 0; i < 5; i++)
            {
                var numbers = Rows.Select(r => r.Numbers[i]);
                if (numbers.All(n => n.Marked))
                {
                    return true;
                }
            }


            return false;
        }
    }
    public class Row
    {
        public int RowNr { get; set; }
        public List<Number> Numbers = new List<Number>();
    }

    public class Number
    {
        public int Value { get; set; }
        public bool Marked = false;
    }
}
