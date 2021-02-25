using MonteCarlo_Blazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonteCarlo_Blazor.Utilities
{
    public class BuffonsNeedlePiCalc
    {
        public BuffonsNeedleSettings Settings { get; set; }
        public BuffonsNeedlePiCalc(BuffonsNeedleSettings settings)
        {
            Settings = settings;
        }

        public IEnumerable<MonteCarloResult<double>> GuessPi()
        {
            var mcReplicationsResults = MonteCarlo.RunReplications(MonteCarloRun, Settings.MonteCarlo);
            foreach (var result in mcReplicationsResults)
            {
                yield return result;
            }
        }

        private double MonteCarloRun(int iterations)
        {
            var rnd = new Random();
            var crossCount = 0;

            for (var i = 0; i < iterations; i++)
            {
                var x = rnd.NextDouble() * Settings.LineGap;
                var angle = rnd.NextDouble() * 180;

                var xLen = Math.Sin(angle) * Settings.NeedleLength;
                if (Math.Abs(xLen) >= Settings.LineGap - x)
                {
                    ++crossCount;
                }
            }

            return (2 * Settings.NeedleLength) / (Settings.LineGap * ((double)crossCount / iterations));
        }
    }
}
