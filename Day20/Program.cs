using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt").Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var algorithm = input[0].Select(x => x == '#').ToArray();

            var image = input.Skip(1).Select(line => line.Select(c => c == '#').ToArray()).ToArray();
            var defaultValue = false;

            var part1 = 0;

            for (var i = 1; i <= 50; i++)
            {
                (image, defaultValue) = RunImageEnhancement(image, algorithm, defaultValue, i);
                if (i == 2)
                {
                    part1 = image.SelectMany(x => x).Count(x => x);
                }
            }

            var part2 = image.SelectMany(x => x).Count(x => x);

            Console.WriteLine("Part1: " + part1);
            Console.WriteLine("Part2: " + part2);
            Console.ReadLine();
        }


        private static (bool[][], bool) RunImageEnhancement(IReadOnlyList<bool[]> image,
            IReadOnlyList<bool> algorithm, bool defaultValue, int run)
        {
            const int padding = 1;

            var newSize = image.Count + padding * 2;
            var enhanced = new bool[newSize][];

            foreach (var y in Enumerable.Range(-padding, newSize))
            {
                enhanced[y + padding] = new bool[newSize];

                foreach (var x in Enumerable.Range(-padding, newSize))
                {
                    enhanced[y + padding][x + padding] = GetValue(image, (x, y), defaultValue, algorithm);
                }
            }

            if ((!defaultValue && algorithm[0]) ||
                (defaultValue && !algorithm.Last()))
            {
                defaultValue = !defaultValue;
            }

            return (enhanced, defaultValue);
        }

        private static bool GetValue(IReadOnlyList<bool[]> image, (int, int) point, bool defaultValue, IReadOnlyList<bool> algorithm)
        {
            var sb = new StringBuilder();

            foreach (var (x, y) in GetSubImage(point).OrderBy(x => x.Key).Select(x => x.Value))
            {
                // is within current image, take value from current
                if (y >= 0 &&
                    y < image.Count &&
                    x >= 0 &&
                    x < image[0].Length)
                {
                    sb.Append(image[y][x] ? 1 : 0);
                }
                else // append default value
                {
                    sb.Append(defaultValue ? 1 : 0);
                }
            }

            var index = Convert.ToInt32(sb.ToString(), 2);
            return algorithm[index];
        }

        private static Dictionary<int, (int X, int Y)> GetSubImage((int X, int Y) centerPoint)
        {
            var adjacent = new Dictionary<int, (int, int)>
            {
                {0, (centerPoint.X - 1, centerPoint.Y - 1)},
                {1, (centerPoint.X, centerPoint.Y - 1)},
                {2, (centerPoint.X + 1, centerPoint.Y - 1)},
                {3, (centerPoint.X - 1, centerPoint.Y)},
                {4, centerPoint},
                {5, (centerPoint.X + 1, centerPoint.Y)},
                {6, (centerPoint.X - 1, centerPoint.Y + 1)},
                {7, (centerPoint.X, centerPoint.Y + 1)},
                {8, (centerPoint.X + 1, centerPoint.Y + 1)},
            };

            return adjacent;
        }
    }

    class ImageEnhancer
    {
        private static readonly IEnumerable<Coor> s_arounds = new Coor[]
        {
            new(-1, -1),
            new(-1, 0),
            new(-1, 1),

            new(0, -1),
            new(0, 0),
            new(0, 1),

            new(1, -1),
            new(1, 0),
            new(1, 1),
        };

        private readonly ReadOnlyCollection<bool> _algorithm;

        // Represents what colour is the inifinite surrounding
        private bool _surroundingColor = false;

        public ImageEnhancer(ReadOnlyCollection<bool> algorithm)
        {
            _algorithm = algorithm;
        }

        public bool[][] Enhance(bool[][] image)
        {
            var height = image.Length;
            var width = image[0].Length;
            int boundary = 2;

            var result = new bool[height + boundary * 2][];

            for (int y = -boundary; y <= height + boundary - 1; y++)
            {
                result[y + boundary] = new bool[width + boundary * 2];

                for (int x = -boundary; x <= width + boundary - 1; x++)
                {
                    result[y + boundary][x + boundary] = ResolvePixel(image, y, x);
                }
            }

            // Check the infinite surroundings
            // If it is dark and 9 darks turn into a light => flip the background
            // If it is lit and 9 lights turn into dark => also flip
            if ((!_surroundingColor && _algorithm[0]) || (_surroundingColor && !_algorithm.Last()))
            {
                _surroundingColor = !_surroundingColor;
            }

            return result;
        }

        public static void Display(bool[][] image)
        {
            for (int y = 0; y < image.Length; y++)
            {
                for (int x = 0; x < image[y].Length; x++)
                {
                    Console.Write(image[y][x] ? '#' : '.');
                }

                Console.WriteLine();
            }
        }

        private bool ResolvePixel(bool[][] image, int y, int x)
        {
            var value = 0;
            var position = new Coor(Y: y, X: x);

            foreach (var c in s_arounds)
            {
                value <<= 1;

                var coor = position + c;
                if (coor.Y >= 0 && coor.Y < image.Length && coor.X >= 0 && coor.X < image[0].Length)
                {
                    // Within bounds, we look at the input image
                    if (image[coor.Y][coor.X])
                    {
                        value |= 1;
                    }
                }
                else
                {
                    // Outside of bounds, we check the fake infinite surroundings
                    if (_surroundingColor)
                    {
                        value |= 1;
                    }
                }
            }

            return _algorithm[value];
        }
    }

    public record Coor(int Y, int X)
    {
        public static Coor operator -(Coor me, Coor other) => new(Y: me.Y - other.Y, X: me.X - other.X);
        public static Coor operator +(Coor me, Coor other) => new(Y: me.Y + other.Y, X: me.X + other.X);
        public int Y { get; } = Y;
        public int X { get; } = X;
    }
}
