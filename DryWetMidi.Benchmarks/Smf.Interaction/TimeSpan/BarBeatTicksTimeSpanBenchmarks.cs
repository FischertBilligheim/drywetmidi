﻿using System;
using BenchmarkDotNet.Engines;
using Melanchall.DryWetMidi.Smf.Interaction;
using NUnit.Framework;

namespace Melanchall.DryWetMidi.Benchmarks.Smf.Interaction
{
    [TestFixture]
    public sealed class BarBeatTicksTimeSpanBenchmarks : BenchmarkTest
    {
        #region Nested classes

        [InProcessSimpleJob(RunStrategy.Monitoring, launchCount: 5, warmupCount: 5, targetCount: 5, invocationCount: 5)]
        public class Benchmarks_BarBeatTicks : TimeSpanBenchmarks<BarBeatTicksTimeSpan>
        {
            #region Constants

            private static readonly TimeSignature FirstTimeSignature = new TimeSignature(3, 4);
            private static readonly TimeSignature SecondTimeSignature = new TimeSignature(5, 8);

            #endregion

            #region Overrides

            public override TempoMap TempoMap { get; } = GetTempoMap();

            #endregion

            #region Methods

            private static TempoMap GetTempoMap()
            {
                var timeSignatureChangeOffset = 5 * TimeOffset - 1;
                var maxTime = Math.Max((TimesCount - 1) * TimeOffset, (TimesCount - 1) * TimeOffset + Length);

                bool firstTimeSignature = true;

                using (var tempoMapManager = new TempoMapManager())
                {
                    var time = 0L;

                    while (time < maxTime)
                    {
                        tempoMapManager.SetTimeSignature(time, firstTimeSignature ? FirstTimeSignature : SecondTimeSignature);

                        firstTimeSignature = !firstTimeSignature;
                        time += timeSignatureChangeOffset;
                    }

                    return tempoMapManager.TempoMap;
                }
            }

            #endregion
        }

        #endregion

        #region Test methods

        [Test]
        [Description("Benchmark bar.beat.ticks time/length conversion.")]
        public void ConvertBarBeatTicksTimeSpan()
        {
            RunBenchmarks<Benchmarks_BarBeatTicks>();
        }

        #endregion
    }
}
