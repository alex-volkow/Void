using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Void.Json
{
    public class JConfigTests
    {
        private static readonly JConfig config = new JConfig(new JObject {
            ["A1"] = 1,
            ["B1"] = 2.0,
            ["C1"] = "3",
            ["D1"] = new JObject {
                ["A2"] = 5,
                ["B2"] = "pewpew",
                ["C2"] = null,
                ["D2"] = new JArray(1.1, 2.2, 3.3)
            },
            ["F1"] = new JArray {
                    11,
                    22.0,
                    "33",
                    new JObject { ["subelement"] = "value" },
                    new JArray("11111111", 222222, 333333.0, null),
                    null
                },
            ["G1"] = new JObject {
                ["G2"] = new Uri("http://localhost:9999")
            }
        });



        [Fact]
        public void GetInt() {
            Assert.Equal(1, (int)config["A1"]);
            Assert.Equal(1, config["A1"].To<int>());
            Assert.Equal(1, config["A1"].Required<int>());
        }

        [Fact]
        public void GetDouble() {
            Assert.Equal(2.0, (double)config["B1"]);
            Assert.Equal(2.0, config["B1"].To<double>());
            Assert.Equal(2.0, config["B1"].Required<double>());
        }

        [Fact]
        public void GetString() {
            Assert.Equal("3", (string)config["C1"]);
            Assert.Equal("3", config["C1"].To<string>());
            Assert.Equal("3", config["C1"].Required<string>());
        }

        [Fact]
        public void GetUri() {
            Assert.Equal(new Uri("http://localhost:9999"), config["G1"]["G2"].Required<Uri>());
        }

        [Fact]
        public void CastStringToInt() {
            Assert.Equal(3, (int)config["C1"]);
            Assert.Equal(3, config["C1"].To<int>());
            Assert.Equal(3, config["C1"].Required<int>());
        }

        [Fact]
        public void GetNull() {
            Assert.Null((int?)config["C1"]["A2"]);
        }

        [Fact]
        public void GetByIndex() {
            Assert.Equal("value", (string)config["F1"][3]["subelement"]);
        }

        [Fact]
        public void GetRequiredValue() {
            Assert.Equal(5, (int)config["D1"]["A2"].Required());
        }

        [Fact]
        public void GetMissedValue() {
            Assert.Throws<ArgumentNullException>(() => (int)config["D1"]["B22"].Required());
        }

        [Fact]
        public void EnumerateMissedValue() {
            Assert.Throws<ArgumentNullException>(() => {
                foreach (var item in config["D1"]["C2"].Required()) {
                    item.ToString();
                }
            });
        }

        [Fact]
        public void EnumerateRequiredItems() {
            var values = new Queue<object>(new object[] {
                "11111111", 222222, 333333.0, null
            });
            foreach (var item in config["F1"][4].Required()) {
                Assert.Equal(values.Dequeue(), item.To<object>());
            }
        }

        [Fact]
        public void OptionIsNull() {
            Assert.True(config["F1"][5].IsNull);
        }

        [Fact]
        public void OptionIsNotNull() {
            Assert.False(config["F1"][3].IsNull);
        }

        [Fact]
        public void OptionExist() {
            Assert.True(config["F1"][5].IsExist);
        }

        [Fact]
        public void OptionDoesNotExist() {
            Assert.False(config["F1"][100].IsExist);
        }

        [Fact]
        public void GetListItem() {
            var list = (IReadOnlyList<JOption>)config["F1"][4];
            Assert.True(list.Count == 4, "Invalid list size");
            Assert.False(list[-1].IsExist);
            Assert.Equal("11111111", (string)list[0]);
            Assert.Equal(222222, (int)list[1]);
            Assert.Equal(333333.0, (double)list[2]);
            Assert.True(list[3].IsNull);
            Assert.True(list[3].IsExist);
            Assert.True(list[4].IsNull);
            Assert.False(list[4].IsExist);
        }

        [Fact]
        public void EnumerableContains() {
            var list = (IReadOnlyList<JOption>)config["F1"][4];
            Assert.True(list.Count == 4, "Invalid list size");
            Assert.Contains(list, e => 222222 == (int)e);
        }

        [Fact]
        public void CheckEmptyList() {
            var list = (IReadOnlyList<JOption>)config["C1"];
            Assert.True(list.Count == 0, "Invalid list size");
            Assert.False(list[-1].IsExist);
            Assert.True(list[3].IsNull);
            Assert.False(list[3].IsExist);
            Assert.True(list[4].IsNull);
            Assert.False(list[4].IsExist);
        }

        [Fact]
        public void GetMapItem() {
            var map = (IReadOnlyDictionary<string, JOption>)config["D1"];
            Assert.True(map.Count == 4, "Invalid map size");
            Assert.Equal(5, (int)map["A2"]);
            Assert.Equal("pewpew", (string)map["B2"]);
            Assert.True(map["C2"].IsNull);
            Assert.True(map["C2"].IsExist);
            Assert.False(map["D2"].IsNull);
            Assert.Equal(3, map["D2"].Count);
            Assert.True(map["C22"].IsNull);
            Assert.False(map["C22"].IsExist);
        }

        [Fact]
        public void CheckEmptyMap() {
            var map = (IReadOnlyDictionary<string, JOption>)config["F1"];
            Assert.True(map.Count == 0, "Invalid map size");
            Assert.True(map["C22"].IsNull);
            Assert.False(map["C22"].IsExist);
        }

        [Fact]
        public void CheckMapKeys() {
            var map = (IReadOnlyDictionary<string, JOption>)config["D1"];
            Assert.True(map.Count == 4, "Invalid map size");
            var keys = new string[] { "A2", "B2", "C2", "D2" };
            Assert.Equal(keys.Length, map.Keys.Count());
            foreach (var key in map.Keys) {
                Assert.Contains(keys, e => e == key);
            }
            foreach (var key in keys) {
                Assert.True(map.ContainsKey(key));
            }
            foreach (var item in map) {
                Assert.Contains(keys, e => e == item.Key);
            }
            Assert.False(map.ContainsKey("D22"));
        }

        [Fact]
        public void CheckMapValues() {
            var map = (IReadOnlyDictionary<string, JOption>)config["D1"];
            Assert.True(map.Count == 4, "Invalid map size");
            Assert.Equal(4, map.Values.Count());
            Assert.Contains(map.Values, e => e.Equals(5));
            Assert.Contains(map.Values, e => e.Equals("pewpew"));
            Assert.DoesNotContain(map.Values, e => e.Equals(6));
        }

        [Fact]
        public void TryGetValueFromMap() {
            var map = (IReadOnlyDictionary<string, JOption>)config["D1"];
            Assert.True(map.Count == 4, "Invalid map size");
            Assert.True(map.TryGetValue("A2", out JOption value1) && (5 == (int)value1));
            Assert.False(map.TryGetValue("A22", out JOption value2));
        }
    }
}
