using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var includeDiagonal = true;

            var lines = new List<Line>();

            foreach (var val in input)
            {
                var values = val.Split("->");
                var startValues = values[0].Split(',');
                var endValues = values[1].Split(',');

                var line = new Line
                {
                    xStart = int.Parse(startValues[0].Trim()),
                    yStart = int.Parse(startValues[1].Trim()),
                    xEnd = int.Parse(endValues[0].Trim()),
                    yEnd = int.Parse(endValues[1].Trim())
                };

                lines.Add(line);
            }

            var points = new List<Point>();

            foreach (var line in lines)
            {
                if (line.xStart == line.xEnd)
                {
                    var start = line.yStart < line.yEnd ? line.yStart : line.yEnd;

                    var yValues = Enumerable.Range(start, Math.Abs(line.yStart - line.yEnd) + 1);
                    foreach (var value in yValues)
                    {
                        points.Add(new Point { X = line.xStart, Y = value });
                    }
                }
                else if (line.yStart == line.yEnd)
                {
                    var start = line.xStart < line.xEnd ? line.xStart : line.xEnd;
                    var xValues = Enumerable.Range(start, Math.Abs(line.xStart - line.xEnd) + 1);
                    foreach (var value in xValues)
                    {
                        points.Add(new Point { X = value, Y = line.yStart });
                    }
                }
                else if (includeDiagonal)
                {
                    var xDirection = line.xStart == line.xEnd ? 0 : line.xEnd > line.xStart ? 1 : -1;
                    var yDirection = line.yStart == line.yEnd ? 0 : line.yEnd > line.yStart ? 1 : -1;
                    var distance = Math.Max(Math.Abs(line.xStart - line.xEnd), Math.Abs(line.yStart - line.yEnd)) + 1;

                    points.AddRange(Enumerable.Range(0, distance).Select(e => new Point(line.xStart + (e * xDirection), line.yStart + (e * yDirection))));
                }
            }

            Console.WriteLine($"Part1: {points.GroupBy(p => p).Count(x => x.Count() > 1)}");
            Console.ReadLine();
        }
    }

    public class Line
    {
        public int xStart { get; set; }
        public int xEnd { get; set; }
        public int yStart { get; set; }
        public int yEnd { get; set; }
    }
}
