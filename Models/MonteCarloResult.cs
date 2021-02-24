namespace MonteCarlo_Blazor.Models
{
    public class MonteCarloResult<TResult>
    {
        public int Replication { get; set; }
        public TResult Result { get; set; }
    }
}
