using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Dalion.Ringor.Serialization {
    public class StringNullableEnumArrayConverterTests {
        private readonly StringNullableEnumArrayConverter _sut;

        protected StringNullableEnumArrayConverterTests() {
            _sut = new StringNullableEnumArrayConverter();
        }

        private class EnumContainer<T> where T : struct {
            [JsonConverter(typeof(StringNullableEnumArrayConverter))]
            public T?[] Enums { get; set; }
        }

        private enum NamedEnum {
            First,
            Second,
        }

        private enum ValuedEnum {
            First = 1,
            Second = 2,
        }

        public class ReadJson : StringNullableEnumArrayConverterTests {
            private readonly JsonSerializerSettings _settings;

            public ReadJson() {
                _settings = new JsonSerializerSettings {
                    Converters = {
                        _sut,
                        new StringNullableEnumConverter()
                    }
                };
            }

            [Fact]
            public void GivenNullToDeserialize_ReadsNull() {
                var expected = new EnumContainer<NamedEnum> {
                    Enums = null
                };

                var actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>("{\r\n  \"Enums\": null\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenEmptyArrayToDeserialize_ReadsEmptyArray() {
                var expected = new EnumContainer<NamedEnum> {
                    Enums = new NamedEnum?[0]
                };

                var actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>("{\r\n  \"Enums\": []\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenValidNamesToDeserialize_ReadsCorrectEnumValue() {
                var expected = new EnumContainer<NamedEnum> {
                    Enums = new NamedEnum?[] {NamedEnum.First}
                };

                var actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>("{\r\n  \"Enums\": [\"First\"]\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);

                expected = new EnumContainer<NamedEnum> {
                    Enums = new NamedEnum?[] {NamedEnum.First, NamedEnum.Second}
                };

                actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>("{\r\n  \"Enums\": [\"First\",\"Second\"]\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenInvalidName_ReadsNull() {
                var expected = new EnumContainer<NamedEnum> {
                    Enums = new NamedEnum?[] {null, NamedEnum.Second}
                };

                // ReSharper disable once RedundantStringInterpolation
                var actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>($"{{\r\n  \"Enums\": [\"banana\", \"Second\"]\r\n}}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenValidValueToDeserialize_ReadsCorrectEnumValue() {
                var expected = new EnumContainer<ValuedEnum> {
                    Enums = new ValuedEnum?[] {ValuedEnum.First}
                };

                var actual = JsonConvert.DeserializeObject<EnumContainer<ValuedEnum>>("{\r\n  \"Enums\": [1]\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);

                expected = new EnumContainer<ValuedEnum> {
                    Enums = new ValuedEnum?[] {ValuedEnum.Second}
                };

                actual = JsonConvert.DeserializeObject<EnumContainer<ValuedEnum>>("{\r\n  \"Enums\": [2]\r\n}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenInvalidValue_ReadsNull() {
                var expected = new EnumContainer<ValuedEnum> {
                    Enums = new ValuedEnum?[] {null}
                };

                // ReSharper disable once RedundantStringInterpolation
                var actual = JsonConvert.DeserializeObject<EnumContainer<ValuedEnum>>($"{{\r\n  \"Enums\": [564]\r\n}}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }

            [Theory]
            [InlineData("[\"First\",\"Second\"]")]
            [InlineData("[\"FIRST\",\"second\"]")]
            [InlineData("[\"FirsT\",\"SeconD\"]")]
            public void GivenValidNameWithDifferentCasing_ReadsCorrectEnumValue(string valueToDeserialize) {
                var expected = new EnumContainer<NamedEnum> {
                    Enums = new NamedEnum?[] {NamedEnum.First, NamedEnum.Second}
                };

                var actual = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>($"{{\r\n  \"Enums\": {valueToDeserialize}\r\n}}", _settings);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        public class WriteJson : StringNullableEnumArrayConverterTests {
            [Fact]
            public void GivenNullToSerialize_WritesNull() {
                var c = new EnumContainer<NamedEnum> {
                    Enums = null
                };

                var json = JsonConvert.SerializeObject(c, Formatting.None, _sut);
                json.Should().Be("{\"Enums\":null}");
            }

            [Fact]
            public void GivenEmptyArrayToSerialize_WritesEmptyArray() {
                var c = new EnumContainer<NamedEnum> {
                    Enums = new NamedEnum?[0]
                };

                var json = JsonConvert.SerializeObject(c, Formatting.None, _sut);
                json.Should().Be("{\"Enums\":[]}");
            }

            [Fact]
            public void GivenNamedEnumToSerialize_WritesTheNames() {
                var c = new EnumContainer<NamedEnum> {
                    Enums = new NamedEnum?[] {NamedEnum.First}
                };

                var json = JsonConvert.SerializeObject(c, Formatting.None, _sut);
                json.Should().Be("{\"Enums\":[\"First\"]}");

                c = new EnumContainer<NamedEnum> {
                    Enums = new NamedEnum?[] {NamedEnum.First, NamedEnum.Second}
                };

                json = JsonConvert.SerializeObject(c, Formatting.None, _sut);
                json.Should().Be("{\"Enums\":[\"First\",\"Second\"]}", json);
            }

            [Fact]
            public void GivenValuedEnumToSerialize_WritesTheName() {
                var c = new EnumContainer<ValuedEnum> {
                    Enums = new ValuedEnum?[] {ValuedEnum.First}
                };

                var json = JsonConvert.SerializeObject(c, Formatting.None, _sut);
                json.Should().Be("{\"Enums\":[\"First\"]}");

                c = new EnumContainer<ValuedEnum> {
                    Enums = new ValuedEnum?[] {ValuedEnum.First, ValuedEnum.Second}
                };

                json = JsonConvert.SerializeObject(c, Formatting.None, _sut);
                json.Should().Be("{\"Enums\":[\"First\",\"Second\"]}", json);
            }
        }
    }
}