using MonteCarlo_Blazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonteCarlo_Blazor.Utilities
{
    public class BuffonsNeedlePiCalc
    {
        public IEnumerable<double> GuessPi(BuffonsNeedleSettings settings)
        {
            var mc = new MonteCarlo<double>();
            return mc.RunReplications(() => BuffonThrow(settings), settings.MonteCarlo)
                .Select(mcRes => mcRes.Result);
        }

        private bool BuffonThrow(BuffonsNeedleSettings settings)
        {
            var rnd = new Random();
            var x = rnd.NextDouble() * settings.LineGap;
            var angle = rnd.NextDouble() * 180;

            var xLen = Math.Sin(angle) * settings.NeedleLength;
            return (Math.Abs(xLen) >= settings.LineGap - x);
        }
    }
}
