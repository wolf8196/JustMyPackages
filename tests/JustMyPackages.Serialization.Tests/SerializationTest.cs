using JustMyPackages.Http.Rpc;
using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Xunit;

namespace JustMyPackages.Serialization.Tests
{
    public abstract class SerializationTest
    {
        protected abstract string Serialize<T>(T obj);

        protected abstract T Deserialize<T>(string str);

        [Fact]
        public void SerializesEmptyRpcApiRequest()
        {
            var expected = Act(RpcApiRequest.Empty);
            expected.Should().NotBeNull();
        }

        [Fact]
        public void SerializesSuccessfulRpcApiResponse()
        {
            ActAssert(new RpcApiResponse { IsSuccess = true });
        }

        [Fact]
        public void SerializesUnsuccessfulRpcApiResponse()
        {
            ActAssert(new RpcApiResponse { IsSuccess = false, Error = "Test message." });
        }

        [Fact]
        public void SerializesTestDto()
        {
            ActAssert(new TestDto
            {
                DefaultIntValue = default,
                NullIntValue = null,
                NormalNullableDoubleValue = 1.123123123124,
                NormalDoubleValue = 123.1231324235,
                Nan = double.NaN,
                NullableNan = double.NaN,
                Guid = Guid.NewGuid(),
                LocalTimeStamp = DateTime.Now,
                UtcTimeStamp = DateTime.UtcNow,
                LocalTimeStampOffset = DateTimeOffset.Now,
                UtcTimeStampOffset = DateTimeOffset.UtcNow,
                StringDictionary = new Dictionary<string, int>
                {
                    { "str1", 1 },
                    { "Str1", 2 },
                },
                IntDictionary = new Dictionary<int, int>
                {
                    { 100, 1 },
                    { 200, 2 },
                },
                NullCollection = null,
                EmptyCollection = [],
                NormalStream = new MemoryStream([1, 2, 3, 4, 5,]),
                NullStream = null,
                Enum = TestEnum.Value2,
                EnumDictionary = new Dictionary<TestEnum, Guid>
                {
                    { TestEnum.Value1, Guid.NewGuid() },
                    { TestEnum.Value2, Guid.NewGuid() },
                    { TestEnum.Value3, Guid.NewGuid() },
                }
            });
        }

        private T Act<T>(T obj)
        {
            var serialized = Serialize(obj);
            var newObj = Deserialize<T>(serialized);
            return newObj;
        }

        private void ActAssert<T>(T expected)
        {
            var actual = Act(expected);
            actual.Should().BeEquivalentTo(expected, opts =>
            {
                opts.Using<DateTime>(ctx =>
                {
                    ctx.Subject.Should().Be(ctx.Expectation).And.BeIn(ctx.Expectation.Kind);
                })
                .WhenTypeIs<DateTime>();

                opts.Using<Stream, StreamComparer>();

                return opts;
            });
        }

        private class TestDto
        {
            public int DefaultIntValue { get; init; }

            public int? NullIntValue { get; init; }

            public double NormalDoubleValue { get; init; }

            public double? NormalNullableDoubleValue { get; init; }

            public double Nan { get; init; }

            public double? NullableNan { get; init; }
            
            public DateTime LocalTimeStamp { get; init; }

            public DateTime UtcTimeStamp { get; init; }

            public DateTimeOffset LocalTimeStampOffset { get; init; }

            public DateTimeOffset UtcTimeStampOffset { get; init; }

            public Guid Guid { get; init; }

            public IReadOnlyDictionary<string, int>? StringDictionary { get; init; }

            public IReadOnlyDictionary<int, int>? IntDictionary { get; init; }

            public IReadOnlyCollection<int>? NullCollection { get; init; }

            public IReadOnlyCollection<int>? EmptyCollection { get; init; }

            public Stream? NullStream { get; init; }

            public Stream? NormalStream { get; init; }

            public TestEnum Enum { get; init; }

            public IReadOnlyDictionary<TestEnum, Guid>? EnumDictionary { get; init; }
        }

        private enum TestEnum
        {
            Undefined = 0,
            Value1 = 1,
            Value2 = 2,
            Value3 = 3
        }
    }
}