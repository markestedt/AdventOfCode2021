using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var rotations = new HashSet<Func<Vector3, Vector3>>
            {
                v => v,
                v => new Vector3(v.X, -v.Z, v.Y),
                v =>  new Vector3(v.X, -v.Y, -v.Z),
                v =>  new Vector3(v.X, v.Z, -v.Y),

                v =>  new Vector3(-v.Y, v.X, v.Z),
                v =>  new Vector3(v.Z, v.X, v.Y),
                v =>  new Vector3(v.Y, v.X, -v.Z),
                v =>  new Vector3(-v.Z, v.X, -v.Y),

                v =>  new Vector3(-v.X, -v.Y, v.Z),
                v =>  new Vector3(-v.X, -v.Z, -v.Y),
                v =>  new Vector3(-v.X, v.Y, -v.Z),
                v =>  new Vector3(-v.X, v.Z, v.Y),

                v =>  new Vector3(v.Y, -v.X, v.Z),
                v =>  new Vector3(v.Z, -v.X, -v.Y),
                v =>  new Vector3(-v.Y, -v.X, -v.Z),
                v =>  new Vector3(-v.Z, -v.X, v.Y),

                v =>  new Vector3(-v.Z, v.Y, v.X),
                v =>  new Vector3(v.Y, v.Z, v.X),
                v =>  new Vector3(v.Z, -v.Y, v.X),
                v =>  new Vector3(-v.Y, -v.Z, v.X),

                v =>  new Vector3(-v.Z, -v.Y, -v.X),
                v =>  new Vector3(-v.Y, v.Z, -v.X),
                v =>  new Vector3(v.Z, v.Y, -v.X),
                v =>  new Vector3(v.Y, -v.Z, -v.X),
            };

            var input = File.ReadAllText("input.txt");

            var scans = input.Split(Environment.NewLine + Environment.NewLine).Select(x =>
                x.Split(Environment.NewLine).Skip(1).Select(GetVector).ToArray()).ToArray();

            var test = GetBeacons(scans, rotations);
            var ans2 = test.Item2.SelectMany(i => test.Item2.Select(j => ManhattanDistance(i.Position, j.Position)))
                .Max();
        }

        private static int ManhattanDistance(Vector3 a, Vector3 b)
        {
            return Convert.ToInt32(Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z));
        }

        private static (HashSet<Vector3>, List<(Vector3 Position, Func<Vector3, Vector3> Rotation)>) GetBeacons(Vector3[][] input, HashSet<Func<Vector3, Vector3>> rotations)
        {
            var system = input[0].ToHashSet();
            //scans.RemoveAt(0);

            var scannerPositions = new List<(Vector3 Position, Func<Vector3, Vector3> Rotation)> { (new Vector3(0, 0, 0), v => v) };
            var queue = new Queue<Vector3[]>();
            foreach (var san in input[1..])
            {
                queue.Enqueue(san);
            }

            while (queue.TryDequeue(out var scanner))
            {
                bool matched = false;
                foreach (var rotation in rotations)
                {
                    var transformed = scanner.Select(s => rotation(s)).ToArray();

                    //var testing = system.Intersect(transformed);

                    var offset2 = transformed.SelectMany(i => system.Select(j => new Vector3(i.X - j.X, i.Y - j.Y, i.Z - j.Z)))
                        .GroupBy(i => i).Select(i => (i.Key, Count: i.Count()));

                    var offset = transformed.SelectMany(i => system.Select(j => new Vector3(i.X - j.X, i.Y - j.Y, i.Z - j.Z)));

                    var test = offset.GroupBy(i => i).OrderByDescending(i => i.Count()).First();
                    // Check if overlap matches in 12 points
                    //if (offset.Count < 12) continue;
                    if (test.Count() < 12) continue;

                    matched = true;
                    var added = transformed.Count(i => system.Add(i - test.Key));
                    scannerPositions.Add((test.Key, rotation));
                    break;
                }
                // If no match was found re-enqueue this scanner to check others first.
                if (!matched) queue.Enqueue(scanner);
            }

            return (system, scannerPositions);
        }

        private static Vector3 GetVector(string input)
        {
            var values = input.Split(',').Select(float.Parse).ToArray();
            return new Vector3(values[0], values[1], values[2]);
        }
    }
}
