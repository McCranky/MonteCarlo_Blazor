using MonteCarlo_Blazor.Models;
using System;
using System.Collections.Generic;

namespace MonteCarlo_Blazor.Utilities
{
    public class MonteCarlo<TResult>
    {
        public IEnumerable<MonteCarloResult<TResult>> RunReplications(Func<TResult> functionToPerform, MonteCarloSettings settings)
        {
            var rep = 1;
            while (!settings.CancellationToken && rep <= settings.Replications)
            {
                var result = functionToPerform();
                yield return new MonteCarloResult<TResult>
                {
                    Result = result,
                    Replication = rep
                };
                ++rep;
            }
        }

        public TResult RunIterations(Func<TResult> functionToPerform, MonteCarloSettings settings)
        {
            var crossCount = 0;
            for (var i = 0; i < settings.Iterations; i++)
            {
                var x = rnd.NextDouble() * settings.LineGap;
                var angle = rnd.NextDouble() * 180;

                var xLen = Math.Sin(angle) * settings.NeedleLength;
                if (Math.Abs(xLen) >= settings.LineGap - x)
                {
                    ++crossCount;
                }
            }


        }
    }
}
