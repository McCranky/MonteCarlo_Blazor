using MonteCarlo_Blazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonteCarlo_Blazor.Utilities
{
    public static class MonteCarlo
    {
        /// <summary>
        /// Runs raplications on given monte carlo run implementation
        /// </summary>
        /// <param name="monteCarloRunImplementation">
        /// Correct implementation of single monte carlo run function that accepts number of iteration.
        /// </param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IEnumerable<MonteCarloResult<double>> RunReplications(Func<int, double> monteCarloRunImplementation, MonteCarloSettings settings)
        {
            var rep = 1;
            var results = new List<double>(settings.Replications);
            while (!settings.CancellationToken && rep <= settings.Replications)
            {
                var iterationResult = monteCarloRunImplementation(settings.Iterations);
                results.Add(iterationResult);

                yield return new MonteCarloResult<double>
                {
                    Result = results.Sum() / results.Count,
                    Replication = rep
                };
                ++rep;
            }
        }
    }
}
