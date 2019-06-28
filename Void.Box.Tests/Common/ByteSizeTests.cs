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
    }
}
