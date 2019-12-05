using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    [Parallelizable]
    public class ByteSizeTests
    {
        [Test]
        public void LongConverting() {
            Assert.AreEqual(333, new ByteSize(333).Value);
        }

        [Test]
        public void DoubleConverting() {
            Assert.AreEqual(333 * 1024 * 1024, new ByteSize(333, ByteUnit.Mega).Value);
        }

        [Test]
        public void BytesParsing() {
            Assert.AreEqual(22, ByteSize.Parse("22 bytes").Value);
        }

        [Test]
        public void KiloBytesDotParsing() {
            Assert.AreEqual((decimal)33.3 * 1024, ByteSize.Parse("33.3 KB").Value);
        }

        [Test]
        public void MegaBytesCommaParsing() {
            Assert.AreEqual((decimal)0.7 * 1024 * 1024, ByteSize.Parse(".7 MB").Value);
        }

        [Test]
        public void GigaBytesCommaParsing() {
            Assert.AreEqual((decimal)2 * 1024 * 1024  *1024, ByteSize.Parse("2. GB").Value);
        }

        [Test]
        public void TeraBytesCommaParsing() {
            Assert.AreEqual((decimal)43.43 * 1024 * 1024 * 1024 * 1024, ByteSize.Parse("43,43TB").Value);
        }

        [Test]
        public void FailPasingMiltiplePunctuation() {
            Assert.Throws<FormatException>(() => {
                ByteSize.Parse("123.123, bytes");
            });
        }

        [Test]
        public void FailPasingUnit() {
            Assert.Throws<FormatException>(() => {
                ByteSize.Parse("123 PPB");
            });
        }

        [Test]
        public void ToLongString() {
            var pool = new int[] {
                146, 47, 107520, 30100, 262566, 69120, 10228, 224, 64512, 4500, 20480, 4520, 16248,
                671608, 135032, 24024, 20344, 243784, 1496440, 788856, 125512, 62944, 669608, 760320,
                40960, 38400, 7680, 1880, 10752, 2108, 72704, 5252, 176640, 424448, 5120, 5120, 38400,
                38912, 34816, 40080, 251832, 37000, 27784, 118520, 89600, 25088, 559, 1079856, 730432,
                1009000, 1033952, 1079856, 1102088, 1141796, 3139712, 73352, 62088, 33936, 1307648,
                1006080, 868864, 32038, 1163, 230, 1153, 68266, 151749, 48494, 108539, 5227, 76483,
                4028, 32461, 202401, 492048, 155764, 625953, 229924, 402249, 78641, 311949, 136072,
                250568, 58078, 190253, 1641, 282115, 86929, 132370, 1117, 43184, 18467, 50276, 23264,
                19798, 5871, 587
            };
            for (var i = 0; i < pool.Length; i++) {
                try {
                    var size = new ByteSize(pool[i]);
                    size.ToLongString();
                }
                catch(Exception ex) {
                    throw new InvalidOperationException(
                        $"Failed to convert to long string '{pool[i]}'", ex
                        );
                }
            }
        }
    }
}
