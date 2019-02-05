// -----------------------------------------------------------------------
// <copyright file="ThreadLocalRandom.cs" company="Petabridge, LLC">
//      Copyright (C) 2015 - 2019 Petabridge, LLC <https://petabridge.com>
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;

namespace Petabridge.Collections.Tests
{
    /// <summary>
    ///     Create random numbers with Thread-specific seeds.
    ///     Borrowed form Jon Skeet's brilliant C# in Depth: http://csharpindepth.com/Articles/Chapter12/Random.aspx
    /// </summary>
    public static class ThreadLocalRandom
    {
        private static int _seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> Rng =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));

        /// <summary>
        ///     The current random number seed available to this thread
        /// </summary>
        public static Random Current => Rng.Value;
    }
}