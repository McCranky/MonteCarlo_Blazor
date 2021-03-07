using MonteCarlo_Blazor.Models;
using System;
using System.Collections.Generic;

namespace MonteCarlo_Blazor.Utilities
{
    public class RobitCarlo : MonteCarlo<int>
    {
        public RobitSettings RobitSettings { get; set; }
        private bool[,] _matrix;
        private readonly Random[] _rndGens;

        public RobitCarlo(RobitSettings settings, int seed1, int seed2, int seed3)
        {
            RobitSettings = settings;
            Settings = settings.MonteCarlo;
            _rndGens = new Random[]
            {
                new Random(seed1),
                new Random(seed2),
                new Random(seed3)
            };
            Console.WriteLine($"Seed 1: {seed1}");
            Console.WriteLine($"Seed 2: {seed2}");
            Console.WriteLine($"Seed 3: {seed3}");
        }

        protected override void BeforeReplication()
        {
            _matrix = new bool[RobitSettings.YNodes, RobitSettings.XNodes];
        }

        protected override int DoReplication()
        {
            var (x, y) = (RobitSettings.StartX, RobitSettings.StartY);
            var (lastX, lastY) = (x, y);
            var steps = 0;
            var fields = RobitSettings.XNodes * RobitSettings.YNodes;
            var goTowards = x * y <= fields - 1;

            do
            {
                // skontroluj uzol
                if (_matrix[y, x]) break;

                // poznač uzol
                ++steps;
                _matrix[y, x] = true;

                // najdenie susedov
                #region GetNeighbours
                var neighbours = new List<Tuple<int, int>>(4);
                // top
                if (y + 1 < RobitSettings.YNodes &&
                    (RobitSettings.AllowBackwardMove || (x != lastX || y + 1 != lastY)))
                {
                    neighbours.Add(new Tuple<int, int>(x, y + 1));
                }
                // right
                if (x + 1 < RobitSettings.XNodes &&
                    (RobitSettings.AllowBackwardMove || (x + 1 != lastX || y != lastY)))
                {
                    neighbours.Add(new Tuple<int, int>(x + 1, y));
                }
                // bottom
                if (y - 1 >= 0 &&
                    (RobitSettings.AllowBackwardMove || (x != lastX || y - 1 != lastY)))
                {
                    neighbours.Add(new Tuple<int, int>(x, y - 1));
                }
                // left
                if (x - 1 >= 0 &&
                    (RobitSettings.AllowBackwardMove || (x - 1 != lastX || y != lastY)))
                {
                    neighbours.Add(new Tuple<int, int>(x - 1, y));
                }
                #endregion

                // vyber suseda
                if (neighbours.Count == 0) break;
                Tuple<int, int> node;

                if (RobitSettings.BetterStrategy)
                {
                    node = neighbours[0];
                    var chosenValue = goTowards ? int.MaxValue : int.MinValue;
                    // strategia hilbertovej krivky
                    foreach (var nei in neighbours)
                    {
                        var hilberValue = CalculateHilbertNumber(fields, x, y);
                        if (goTowards)
                        {
                            if (hilberValue < chosenValue)
                            {
                                chosenValue = hilberValue;
                                node = nei;
                            }
                        }
                        else
                        {
                            if (hilberValue > chosenValue)
                            {
                                chosenValue = hilberValue;
                                node = nei;
                            }
                        }
                    }

                }
                else
                {
                    // klasicka random strategia
                    var index = 0;

                    if (neighbours.Count == 2)
                        index = _rndGens[0].Next(2);
                    else if (neighbours.Count == 3)
                        index = _rndGens[1].Next(3);
                    else if (neighbours.Count == 4)
                        index = _rndGens[2].Next(4);

                    node = neighbours[index];
                }

                (lastX, lastY) = (x, y);
                (x, y) = (node.Item1, node.Item2);

            } while (true);

            return --steps;
        }


        private int CalculateHilbertNumber(int n, int x, int y)
        {
            int rx, ry, s, d = 0;
            for (s = n / 2; s > 0; s /= 2)
            {

                rx = Convert.ToInt32(((x & s) > 0));
                ry = Convert.ToInt32((y & s) > 0);
                d += s * s * ((3 * rx) ^ ry);
                RotateQuadrant(s, ref x, ref y, rx, ry);
            }

            return d;
        }

        private void RotateQuadrant(int n, ref int x, ref int y, int rx, int ry)
        {
            if (ry == 0)
            {
                if (rx == 1)
                {
                    x = n - 1 - x;
                    y = n - 1 - y;
                }

                var t = x;
                x = y;
                y = t;
            }
        }
    }
}
