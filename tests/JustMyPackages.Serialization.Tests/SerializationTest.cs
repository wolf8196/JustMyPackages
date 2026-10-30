using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using JustMyPackages.Http.Rpc;
using MessagePack;

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
                NormalDecimalValue = decimal.MaxValue,
                GuidValue = Guid.NewGuid(),
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
                Enum = TestEnumType.Value2,
                EnumDictionary = new Dictionary<TestEnumType, Guid>
                {
                    { TestEnumType.Value1, Guid.NewGuid() },
                    { TestEnumType.Value2, Guid.NewGuid() },
                    { TestEnumType.Value3, Guid.NewGuid() },
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

        [MessagePackObject]
        public class TestDto
        {
            [Key(0)]
            public int DefaultIntValue { get; init; }

            [Key(1)]
            public int? NullIntValue { get; init; }

            [Key(2)]
            public double NormalDoubleValue { get; init; }

            [Key(3)]
            public double? NormalNullableDoubleValue { get; init; }

            [Key(4)]
            public double Nan { get; init; }

            [Key(5)]
            public double? NullableNan { get; init; }

            [Key(6)]
            public decimal NormalDecimalValue { get; set; }

            [Key(7)]
            public DateTime LocalTimeStamp { get; init; }

            [Key(8)]
            public DateTime UtcTimeStamp { get; init; }

            [Key(9)]
            public DateTimeOffset LocalTimeStampOffset { get; init; }

            [Key(10)]
            public DateTimeOffset UtcTimeStampOffset { get; init; }

            [Key(11)]
            public Guid GuidValue { get; init; }

            [Key(12)]
            public IReadOnlyDictionary<string, int>? StringDictionary { get; init; }

            [Key(13)]
            public IReadOnlyDictionary<int, int>? IntDictionary { get; init; }

            [Key(14)]
            public IReadOnlyCollection<int>? NullCollection { get; init; }

            [Key(15)]
            public IReadOnlyCollection<int>? EmptyCollection { get; init; }

            [Key(16)]
            public TestEnumType Enum { get; init; }

            [Key(17)]
            public IReadOnlyDictionary<TestEnumType, Guid>? EnumDictionary { get; init; }

            [Key(18)]
            public Stream? NullStream { get; init; }

            [Key(19)]
            public Stream? NormalStream { get; init; }
        }

        public enum TestEnumType
        {
            Undefined = 0,
            Value1 = 1,
            Value2 = 2,
            Value3 = 3
        }
    }
}