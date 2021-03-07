using MonteCarlo_Blazor.Models;
using System.Collections.Generic;

namespace MonteCarlo_Blazor.Utilities
{
    public abstract class MonteCarlo<TResult>
    {
        protected MonteCarloSettings Settings { get; set; }
        protected virtual void BeforeReplication() { }
        public IEnumerable<TResult> RunReplications()
        {
            for (int i = 0; i < Settings.Replications && !Settings.CancellationToken; i++)
            {
                BeforeReplication();

                var result = DoReplication();

                AfterReplication();

                yield return result;
            }
        }
        protected virtual void AfterReplication() { }
        protected abstract TResult DoReplication();
    }
}
