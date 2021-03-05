using MonteCarlo_Blazor.Models;
using System.Collections.Generic;

namespace MonteCarlo_Blazor.Utilities
{
    public abstract class MonteCarlo<TResult>
    {
        protected MonteCarloSettings Settings { get; set; }
        protected virtual void BeforeSimulation() { }
        public IEnumerable<TResult> RunReplications()
        {
            for (int i = 0; i < Settings.Replications && !Settings.CancellationToken; i++)
            {
                BeforeSimulation();

                var result = DoReplication();

                AfterSimulation();

                yield return result;
            }
        }
        protected virtual void AfterSimulation() { }
        protected abstract TResult DoReplication();
    }
}
