using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;

namespace Void.Json
{
    [Parallelizable]
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



        [Test]
        public void GetInt() {
            Assert.AreEqual(1, (int)config["A1"]);
            Assert.AreEqual(1, config["A1"].To<int>());
            Assert.AreEqual(1, config["A1"].Required<int>());
        }

        [Test]
        public void GetDouble() {
            Assert.AreEqual(2.0, (double)config["B1"]);
            Assert.AreEqual(2.0, config["B1"].To<double>());
            Assert.AreEqual(2.0, config["B1"].Required<double>());
        }

        [Test]
        public void GetString() {
            Assert.AreEqual("3", (string)config["C1"]);
            Assert.AreEqual("3", config["C1"].To<string>());
            Assert.AreEqual("3", config["C1"].Required<string>());
        }

        [Test]
        public void GetUri() {
            Assert.AreEqual(new Uri("http://localhost:9999"), config["G1"]["G2"].Required<Uri>());
        }

        [Test]
        public void CastStringToInt() {
            Assert.AreEqual(3, (int)config["C1"]);
            Assert.AreEqual(3, config["C1"].To<int>());
            Assert.AreEqual(3, config["C1"].Required<int>());
        }

        [Test]
        public void GetNull() {
            Assert.Null((int?)config["C1"]["A2"]);
        }

        [Test]
        public void GetByIndex() {
            Assert.AreEqual("value", (string)config["F1"][3]["subelement"]);
        }

        [Test]
        public void GetRequiredValue() {
            Assert.AreEqual(5, (int)config["D1"]["A2"].Required());
        }

        [Test]
        public void GetMissedValue() {
            Assert.Throws<InvalidDataException>(() => config["D1"]["B22"].Required<int>());
        }

        [Test]
        public void EnumerateMissedValue() {
            Assert.Throws<InvalidDataException>(() => {
                foreach (var item in config["D1"]["C2"].Required()) {
                    item.ToString();
                }
            });
        }

        [Test]
        public void EnumerateRequiredItems() {
            var values = new Queue<object>(new object[] {
                "11111111", 222222, 333333.0, null
            });
            foreach (var item in config["F1"][4].Required()) {
                Assert.AreEqual(values.Dequeue(), item.To<object>());
            }
        }

        [Test]
        public void OptionIsNull() {
            Assert.True(config["F1"][5].IsNull);
        }

        [Test]
        public void OptionIsNotNull() {
            Assert.False(config["F1"][3].IsNull);
        }

        [Test]
        public void OptionExist() {
            Assert.True(config["F1"][5].IsExist);
        }

        [Test]
        public void OptionDoesNotExist() {
            Assert.False(config["F1"][100].IsExist);
        }

        [Test]
        public void GetListItem() {
            var list = (IReadOnlyList<JOption>)config["F1"][4];
            Assert.True(list.Count == 4, "Invalid list size");
            Assert.False(list[-1].IsExist);
            Assert.AreEqual("11111111", (string)list[0]);
            Assert.AreEqual(222222, (int)list[1]);
            Assert.AreEqual(333333.0, (double)list[2]);
            Assert.True(list[3].IsNull);
            Assert.True(list[3].IsExist);
            Assert.True(list[4].IsNull);
            Assert.False(list[4].IsExist);
        }

        [Test]
        public void EnumerableContains() {
            var list = (IReadOnlyList<JOption>)config["F1"][4];
            Assert.True(list.Count == 4, "Invalid list size");
            //Assert.(list, e => 222222 == (int)e);
            Assert.That(222222, Is.AnyOf(list));
        }

        [Test]
        public void CheckEmptyList() {
            var list = (IReadOnlyList<JOption>)config["C1"];
            Assert.True(list.Count == 0, "Invalid list size");
            Assert.False(list[-1].IsExist);
            Assert.True(list[3].IsNull);
            Assert.False(list[3].IsExist);
            Assert.True(list[4].IsNull);
            Assert.False(list[4].IsExist);
        }

        [Test]
        public void GetMapItem() {
            var map = (IReadOnlyDictionary<string, JOption>)config["D1"];
            Assert.True(map.Count == 4, "Invalid map size");
            Assert.AreEqual(5, (int)map["A2"]);
            Assert.AreEqual("pewpew", (string)map["B2"]);
            Assert.True(map["C2"].IsNull);
            Assert.True(map["C2"].IsExist);
            Assert.False(map["D2"].IsNull);
            Assert.AreEqual(3, map["D2"].Count);
            Assert.True(map["C22"].IsNull);
            Assert.False(map["C22"].IsExist);
        }

        [Test]
        public void CheckEmptyMap() {
            var map = (IReadOnlyDictionary<string, JOption>)config["F1"];
            Assert.True(map.Count == 0, "Invalid map size");
            Assert.True(map["C22"].IsNull);
            Assert.False(map["C22"].IsExist);
        }

        [Test]
        public void CheckMapKeys() {
            var map = (IReadOnlyDictionary<string, JOption>)config["D1"];
            Assert.True(map.Count == 4, "Invalid map size");
            var keys = new string[] { "A2", "B2", "C2", "D2" };
            Assert.AreEqual(keys.Length, map.Keys.Count());
            foreach (var key in map.Keys) {
                //Assert.Contains(keys, e => e == key);
                Assert.That(key, Is.AnyOf(keys));
            }
            foreach (var key in keys) {
                Assert.True(map.ContainsKey(key));
            }
            foreach (var item in map) {
                //Assert.Contains(keys, e => e == item.Key);
                Assert.That(item.Key, Is.AnyOf(keys));
            }
            Assert.False(map.ContainsKey("D22"));
        }

        [Test]
        public void CheckMapValues() {
            var map = (IReadOnlyDictionary<string, JOption>)config["D1"];
            Assert.True(map.Count == 4, "Invalid map size");
            Assert.AreEqual(4, map.Values.Count());
            //Assert.Contains(map.Values, e => e.Equals(5));
            Assert.That(5, Is.AnyOf(map.Values));
            //Assert.Contains(map.Values, e => e.Equals("pewpew"));
            Assert.That("pewpew", Is.AnyOf(map.Values));
            //Assert.DoesNotContain(map.Values, e => e.Equals(6));
            Assert.False(map.Values.Any(e => e.Equals(6)));
        }

        [Test]
        public void TryGetValueFromMap() {
            var map = (IReadOnlyDictionary<string, JOption>)config["D1"];
            Assert.True(map.Count == 4, "Invalid map size");
            Assert.True(map.TryGetValue("A2", out JOption value1) && (5 == (int)value1));
            Assert.False(map.TryGetValue("A22", out JOption value2));
        }
    }
}
