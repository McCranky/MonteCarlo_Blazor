using System;
using System.ComponentModel.DataAnnotations;

namespace MonteCarlo_Blazor.Models
{
    public class MonteCarloSettings
    {
        [Required]
        [Range(1, 100000000)]
        public int Replications { get; set; } = 1000000;
        public bool CancellationToken { get; set; }
        public int SkipFirstXResults { get; set; } = 100000;
        public int WriteEveryXValue { get; set; } = 10000;
    }
}
