using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Void
{
    public class ByteSizeTests
    {
        [Fact]
        public void LongConverting() {
            Assert.Equal(333, new ByteSize(333).Value);
        }

        [Fact]
        public void DoubleConverting() {
            Assert.Equal(333 * 1024 * 1024, new ByteSize(333, ByteUnit.Mega).Value);
        }

        [Fact]
        public void BytesParsing() {
            Assert.Equal(22, ByteSize.Parse("22 bytes").Value);
        }

        [Fact]
        public void KiloBytesDotParsing() {
            Assert.Equal((decimal)33.3 * 1024, ByteSize.Parse("33.3 KB").Value);
        }

        [Fact]
        public void MegaBytesCommaParsing() {
            Assert.Equal((decimal)0.7 * 1024 * 1024, ByteSize.Parse(".7 MB").Value);
        }

        [Fact]
        public void GigaBytesCommaParsing() {
            Assert.Equal((decimal)2 * 1024 * 1024  *1024, ByteSize.Parse("2. GB").Value);
        }

        [Fact]
        public void TeraBytesCommaParsing() {
            Assert.Equal((decimal)43.43 * 1024 * 1024 * 1024 * 1024, ByteSize.Parse("43,43TB").Value);
        }

        [Fact]
        public void FailPasingMiltiplePunctuation() {
            Assert.Throws<FormatException>(() => {
                ByteSize.Parse("123.123, bytes");
            });
        }

        [Fact]
        public void FailPasingUnit() {
            Assert.Throws<FormatException>(() => {
                ByteSize.Parse("123 PPB");
            });
        }

        [Fact]
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
