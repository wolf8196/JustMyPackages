using System;
using System.Collections.Generic;
using System.IO;
using NeoSmart.StreamCompare;

namespace JustMyPackages.Serialization.Tests
{
    internal sealed class StreamComparer : IEqualityComparer<Stream>
    {
        private readonly StreamCompare _comparer;

        public StreamComparer()
        {
            _comparer = new StreamCompare();
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

            return _comparer.AreEqualAsync(x, y, true).Result;
        }

        public int GetHashCode(Stream obj)
        {
            throw new NotImplementedException();
        }
    }
}