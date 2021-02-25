using MonteCarlo_Blazor.Models;
using System;
using System.Collections.Generic;

namespace MonteCarlo_Blazor.Utilities
{
    public class RobitStepsCalc
    {
        public RobitSettings Settings { get; set; }
        private bool[,] matrix;
        private Random _rnd;

        public RobitStepsCalc(RobitSettings settings)
        {
            Settings = settings;
            _rnd = new Random();
        }

        private void InitAxis()
        {
            matrix = new bool[Settings.YNodes, Settings.XNodes];
        }

        public IEnumerable<MonteCarloResult<double>> CalculateSteps()
        {
            var mcReplicationsResults = MonteCarlo.RunReplications(MonteCarloRun, Settings.MonteCarlo);
            foreach (var result in mcReplicationsResults)
            {
                yield return result;
            }
        }

        private double MonteCarloRun(int iterations)
        {
            var stepsCount = 0;
            var biggerAsKCount = 0;

            for (var i = 0; i < iterations; i++)
            {
                var steps = RunRobit();
                stepsCount += steps;
                if (steps > Settings.KSteps) ++biggerAsKCount;
            }


            return stepsCount / (double)iterations;
        }

        private int RunRobit()
        {
            InitAxis();
            var (x,y) = (Settings.StartX, Settings.StartY);
            var (lastX, lastY) = (x, y);
            var steps = 0;

            do
            {
                // skontroluj uzol
                if (matrix[y, x]) break;

                ++steps;
                matrix[y, x] = true;

                #region GetNeighbours
                var neighbours = new List<Tuple<int, int>>(4);
                // top
                if (y + 1 < Settings.YNodes - 1 && 
                    (x != lastX || y + 1 != lastY))
                {
                    neighbours.Add(new Tuple<int, int>(x, y + 1));
                }
                // right
                if (x + 1 < Settings.XNodes - 1 && 
                    (x + 1 != lastX || y != lastY))
                {
                    neighbours.Add(new Tuple<int, int>(x + 1, y));
                }
                // bottom
                if (y - 1 >= 0 && 
                    (x != lastX || y - 1 != lastY))
                {
                    neighbours.Add(new Tuple<int, int>(x, y - 1));
                }
                // left
                if (x - 1 >= 0 && 
                    (x - 1 != lastX || y != lastY))
                {
                    neighbours.Add(new Tuple<int, int>(x - 1, y));
                }
                #endregion

                if (neighbours.Count == 0) break;
                var node = neighbours[_rnd.Next(neighbours.Count)];
                (lastX, lastY) = (x, y);
                (x, y) = (node.Item1, node.Item2);
            } while (true);

            return steps;
        }
    }
}
