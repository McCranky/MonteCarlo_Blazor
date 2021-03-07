using MonteCarlo_Blazor.Models;
using MonteCarlo_Blazor.Utilities;
using Plotly.Blazor;
using Plotly.Blazor.LayoutLib;
using Plotly.Blazor.Traces;
using Plotly.Blazor.Traces.ScatterLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonteCarlo_Blazor.Pages
{
    public partial class Robit
    {
        private readonly RobitSettings settings = new RobitSettings();

        #region StepsChart
        private PlotlyChart chartSteps;
        private Config configSteps = new Config();
        private Layout layoutSteps = new Layout
        {
            Title = new Title { Text = "Robit steps" },
            YAxis = new List<YAxis> { new YAxis { Title = new Plotly.Blazor.LayoutLib.YAxisLib.Title { Text = "Steps count" } } },
            XAxis = new List<XAxis> { new XAxis { Title = new Plotly.Blazor.LayoutLib.XAxisLib.Title { Text = "Replication" } } },
            AutoSize = true
        };
        private IList<ITrace> dataSteps = new List<ITrace>
        {
            new Scatter
            {
                Name = "Steps",
                Mode = ModeFlag.Lines,
                X = new List<object>{},
                Y = new List<object>{}
            }
        };
        #endregion

        #region ProbabilityChart
        private PlotlyChart chartProbability;
        private Config configProbability = new Config();
        private Layout layoutProbability = new Layout
        {
            Title = new Title { Text = "K Steps Probability" },
            YAxis = new List<YAxis> { new YAxis { Title = new Plotly.Blazor.LayoutLib.YAxisLib.Title { Text = "Percentage %" } } },
            XAxis = new List<XAxis> { new XAxis { Title = new Plotly.Blazor.LayoutLib.XAxisLib.Title { Text = "Replication" } } }
        };
        private IList<ITrace> dataProbability = new List<ITrace>
        {
            new Scatter
            {
                Name = "Probability",
                Mode = ModeFlag.Lines,
                X = new List<object>{},
                Y = new List<object>{}
            }
        };
        #endregion

        private double averageSteps;
        private double averageProbability;

        private async Task Clear()
        {
            await chartSteps.Clear();
            await chartProbability.Clear();
            averageSteps = 0;
            averageProbability = 0;
        }

        private async Task InitTraces()
        {
            await chartSteps.AddTrace(new Scatter
            {
                Name = "Steps",
                Mode = ModeFlag.Lines,
                X = new List<object> { },
                Y = new List<object> { }
            });

            await chartProbability.AddTrace(new Scatter
            {
                Name = "Probability",
                Mode = ModeFlag.Lines,
                X = new List<object> { },
                Y = new List<object> { }
            });
        }

        private async Task RunReplications()
        {
            if (chartSteps.Data.Count == 0) await InitTraces();

            settings.MonteCarlo.CancellationToken = false;
            var rnd = new Random();
            var robitCarlo = new RobitCarlo(settings, rnd.Next(), rnd.Next(), rnd.Next());
            var rep = 1;
            var totalSteps = 0;
            var totalOverSteps = 0;

            foreach (var result in robitCarlo.RunReplications())
            {
                totalSteps += result;
                averageSteps = totalSteps / (double)rep;
                if (result > settings.KSteps)
                {
                    ++totalOverSteps;
                }
                averageProbability = totalOverSteps / (double)rep;

                if (rep > settings.MonteCarlo.SkipFirstXResults && rep % settings.MonteCarlo.WriteEveryXValue == 0)
                {
                    await chartSteps.ExtendTrace(rep, averageSteps, 0);
                    await chartProbability.ExtendTrace(rep, averageProbability, 0);
                }
                ++rep;
            }

            settings.MonteCarlo.CancellationToken = true;
        }
    }
}
