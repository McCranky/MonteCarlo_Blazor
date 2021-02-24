using System;
using System.ComponentModel.DataAnnotations;

namespace MonteCarlo_Blazor.Models
{
    public class BuffonsNeedleSettings
    {
        [Required]
        public MonteCarloSettings MonteCarlo { get; set; } = new MonteCarloSettings();
        [Required]
        [Range(1, 100000000)]
        public double NeedleLength { get; set; } = 2.5;
        [Required]
        [Range(1, 100000000)]
        public double LineGap { get; set; } = 3.2;
    }
}
