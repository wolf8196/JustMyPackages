using System;
using System.Collections.Generic;
using System.IO;
using NeoSmart.StreamCompare;

namespace JustMyPackages.Serialization.Tests
{
    internal sealed class StreamComparer : IEqualityComparer<Stream>
    {
        private readonly StreamCompare comparer;

        public StreamComparer()
        {
            comparer = new StreamCompare();
        }

        public bool Equals(Stream? x, Stream? y)
        {
            if (x != null)
            {
                x.Position = 0;
            }

            if (y != null)
            {
                y.Position = 0;
            }

            return comparer.AreEqualAsync(x, y, true).Result;
        }

        public int GetHashCode(Stream obj)
        {
            throw new NotImplementedException();
        }
    }
}