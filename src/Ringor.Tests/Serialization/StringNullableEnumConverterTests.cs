using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Dalion.Ringor.Serialization {
    public class StringNullableEnumConverterTests {
        private readonly StringNullableEnumConverter _sut;

        protected StringNullableEnumConverterTests() {
            _sut = new StringNullableEnumConverter();
        }

        private class EnumContainer<T> where T : struct {
            [JsonConverter(typeof(StringNullableEnumConverter))]
            public T? Enum { get; set; }
        }

        private enum NamedEnum {
            First,
            Second,
        }

        private enum ValuedEnum {
            First = 1,
            Second = 2,
        }

        public class ReadJson : StringNullableEnumConverterTests {
            private readonly JsonSerializerSettings _settings;

            public ReadJson() {
                _settings = new JsonSerializerSettings() {
                    Converters = {
                        _sut
                    }
                };
            }

            [Fact]
            public void GivenNullToDeserialize_ReadsNull() {
                var expected = new EnumContainer<NamedEnum> {
                    Enum = null
                };

                var actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>("{\r\n  \"Enum\": null\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenValidNameToDeserialize_ReadsCorrectEnumValue() {
                var expected = new EnumContainer<NamedEnum> {
                    Enum = NamedEnum.First
                };

                var actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>("{\r\n  \"Enum\": \"First\"\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);

                expected = new EnumContainer<NamedEnum> {
                    Enum = NamedEnum.Second
                };

                actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>("{\r\n  \"Enum\": \"Second\"\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenInvalidName_ReadsNull() {
                var expected = new EnumContainer<NamedEnum> {
                    Enum = null
                };

                // ReSharper disable once RedundantStringInterpolation
                var actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>($"{{\r\n  \"Enum\": \"banana\"\r\n}}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenValidValueToDeserialize_ReadsCorrectEnumValue() {
                var expected = new EnumContainer<ValuedEnum> {
                    Enum = ValuedEnum.First
                };

                var actual = JsonConvert.DeserializeObject<EnumContainer<ValuedEnum>>("{\r\n  \"Enum\": 1\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);

                expected = new EnumContainer<ValuedEnum> {
                    Enum = ValuedEnum.Second
                };

                actual = JsonConvert.DeserializeObject<EnumContainer<ValuedEnum>>("{\r\n  \"Enum\": 2\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenInvalidValue_ReadsNull() {
                var expected = new EnumContainer<ValuedEnum> {
                    Enum = null
                };

                // ReSharper disable once RedundantStringInterpolation
                var actual = JsonConvert.DeserializeObject<EnumContainer<ValuedEnum>>($"{{\r\n  \"Enum\": 1564\r\n}}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Theory]
            [InlineData("Second")]
            [InlineData("second")]
            [InlineData("sEcOnd")]
            [InlineData("SECOND")]
            public void GivenValidNameWithDifferentCasing_ReadsCorrectEnumValue(string valueToDeserialize) {
                var expected = new EnumContainer<NamedEnum> {
                    Enum = NamedEnum.Second
                };

                var actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>($"{{\r\n  \"Enum\": \"{valueToDeserialize}\"\r\n}}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        public class WriteJson : StringNullableEnumConverterTests {
            [Fact]
            public void GivenNullToSerialize_WritesNull() {
                var c = new EnumContainer<NamedEnum> {
                    Enum = null
                };

                var json = JsonConvert.SerializeObject(c, Formatting.Indented, _sut);
                json.Should().Be("{\r\n  \"Enum\": null\r\n}");
            }

            [Fact]
            public void GivenNamedEnumToSerialize_WritesTheName() {
                var c = new EnumContainer<NamedEnum> {
                    Enum = NamedEnum.First
                };

                var json = JsonConvert.SerializeObject(c, Formatting.Indented, _sut);
                json.Should().Be("{\r\n  \"Enum\": \"First\"\r\n}");

                c = new EnumContainer<NamedEnum> {
                    Enum = NamedEnum.Second
                };

                json = JsonConvert.SerializeObject(c, Formatting.Indented, _sut);
                json.Should().Be("{\r\n  \"Enum\": \"Second\"\r\n}", json);
            }

            [Fact]
            public void GivenValuedEnumToSerialize_WritesTheName() {
                var c = new EnumContainer<ValuedEnum> {
                    Enum = ValuedEnum.First
                };

                var json = JsonConvert.SerializeObject(c, Formatting.Indented, _sut);
                json.Should().Be("{\r\n  \"Enum\": \"First\"\r\n}");

                c = new EnumContainer<ValuedEnum> {
                    Enum = ValuedEnum.Second
                };

                json = JsonConvert.SerializeObject(c, Formatting.Indented, _sut);
                json.Should().Be("{\r\n  \"Enum\": \"Second\"\r\n}", json);
            }
        }
    }
}