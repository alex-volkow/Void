using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json
{
    public partial class JOption
    {
        public static explicit operator char(JOption option) {
            return option?.To<char>() ?? default(char);
        }

        public static explicit operator byte(JOption option) {
            return option?.To<byte>() ?? default(byte);
        }

        public static explicit operator short(JOption option) {
            return option?.To<short>() ?? default(short);
        }

        public static explicit operator int(JOption option) {
            return option?.To<int>() ?? default(int);
        }

        public static explicit operator long(JOption option) {
            return option?.To<long>() ?? default(long);
        }

        public static explicit operator float(JOption option) {
            return option?.To<float>() ?? default(float);
        }

        public static explicit operator double(JOption option) {
            return option?.To<double>() ?? default(double);
        }

        public static explicit operator decimal(JOption option) {
            return option?.To<decimal>() ?? default(decimal);
        }

        public static explicit operator string(JOption option) {
            return option?.To<string>() ?? default(string);
        }

        public static explicit operator DateTime(JOption option) {
            return option?.To<DateTime>() ?? default(DateTime);
        }

        public static explicit operator TimeSpan(JOption option) {
            return option?.To<TimeSpan>() ?? default(TimeSpan);
        }

        public static explicit operator Uri(JOption option) {
            return option?.To<Uri>() ?? default(Uri);
        }
        public static explicit operator char?(JOption option) {
            return option?.To<char?>();
        }

        public static explicit operator byte?(JOption option) {
            return option?.To<byte?>();
        }

        public static explicit operator short?(JOption option) {
            return option?.To<short?>();
        }

        public static explicit operator int?(JOption option) {
            return option?.To<int?>();
        }

        public static explicit operator long?(JOption option) {
            return option?.To<long?>();
        }

        public static explicit operator float?(JOption option) {
            return option?.To<float?>();
        }

        public static explicit operator double?(JOption option) {
            return option?.To<double?>();
        }

        public static explicit operator decimal?(JOption option) {
            return option?.To<decimal?>();
        }

        public static explicit operator DateTime?(JOption option) {
            return option?.To<DateTime?>();
        }

        public static explicit operator TimeSpan?(JOption option) {
            return option?.To<TimeSpan?>();
        }
    }
}
