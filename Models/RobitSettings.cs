using System;
using System.ComponentModel.DataAnnotations;

namespace MonteCarlo_Blazor.Models
{
    public class RobitSettings
    {
        [Required]
        public MonteCarloSettings MonteCarlo { get; set; } = new MonteCarloSettings();
        [Required]
        [Range(1, int.MaxValue)]
        public int XNodes { get; set; } = 4;
        [Required]
        [Range(1, int.MaxValue)]
        public int YNodes { get; set; } = 4;
        [Required]
        [Range(1, int.MaxValue)]
        public int KSteps { get; set; } = 4;
        [Required]
        [Range(1, int.MaxValue)]
        public int StartX { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int StartY { get; set; }
        public bool BetterStrategy { get; set; }
        public bool AllowBackwardMove { get; set; }
    }
}
