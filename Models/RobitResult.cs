namespace MonteCarlo_Blazor.Models
{
    public class RobitResult
    {
        public double AverageSteps { get; set; }
        public double AverageProbability { get; set; }

        public static RobitResult operator /(RobitResult rr, double c)
        {
            return new RobitResult 
            { 
                AverageProbability = rr.AverageProbability / c, 
                AverageSteps = rr.AverageSteps / c 
            };
        }
    }
}
