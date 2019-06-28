using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Void
{
    public static class Strings
    {
        private const int PROTECTION_KEY_SIZE = 256;
        private const int DERIVATION_ITERATIONS = 1000;



        public static Encoding DefaultEncoding { get; } = Encoding.UTF8;



        public static string Encrypt(string text, string password) {
            var salt = GenerateEntropy256();
            var iv = GenerateEntropy256();
            var data = Encoding.UTF8.GetBytes(text);
            using (var protection = new Rfc2898DeriveBytes(password, salt, DERIVATION_ITERATIONS)) {
                using (var key = new RijndaelManaged()) {
                    key.BlockSize = 256;
                    key.Mode = CipherMode.CBC;
                    key.Padding = PaddingMode.PKCS7;
                    var bytes = protection.GetBytes(PROTECTION_KEY_SIZE / 8);
                    using (var encryptor = key.CreateEncryptor(bytes, iv)) {
                        using (var memory = new MemoryStream()) {
                            using (var crypto = new CryptoStream(memory, encryptor, CryptoStreamMode.Write)) {
                                crypto.Write(data, 0, data.Length);
                                crypto.FlushFinalBlock();
                                var cipher = salt;
                                cipher = cipher.Concat(iv).ToArray();
                                cipher = cipher.Concat(memory.ToArray()).ToArray();
                                return Convert.ToBase64String(cipher);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string text, string password) {
            if (string.IsNullOrWhiteSpace(text)) {
                return text;
            }
            var heap = Convert.FromBase64String(text);
            var salt = heap
                .Take(PROTECTION_KEY_SIZE / 8)
                .ToArray();
            var iv = heap.Skip(PROTECTION_KEY_SIZE / 8)
                .Take(PROTECTION_KEY_SIZE / 8)
                .ToArray();
            var cipher = heap
                .Skip((PROTECTION_KEY_SIZE / 8) * 2)
                .Take(heap.Length - ((PROTECTION_KEY_SIZE / 8) * 2))
                .ToArray();
            using (var protection = new Rfc2898DeriveBytes(password, salt, DERIVATION_ITERATIONS)) {
                using (var symmetricKey = new RijndaelManaged()) {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    var bytes = protection.GetBytes(PROTECTION_KEY_SIZE / 8);
                    using (var decryptor = symmetricKey.CreateDecryptor(bytes, iv)) {
                        using (var memory = new MemoryStream(cipher)) {
                            using (var crypto = new CryptoStream(memory, decryptor, CryptoStreamMode.Read)) {
                                var data = new byte[cipher.Length];
                                var count = crypto.Read(data, 0, data.Length);
                                return Encoding.UTF8.GetString(data, 0, count);
                            }
                        }
                    }
                }
            }
        }

        public static string ToBase64(string value) {
            return ToBase64(value, DefaultEncoding);
        }

        public static string ToBase64(string value, Encoding encoding) {
            return Convert.ToBase64String(encoding.GetBytes(value));
        }

        public static string ToBase64URL(string value) {
            return ToBase64URL(value, DefaultEncoding);
        }

        public static string ToBase64URL(string value, Encoding encoding) {
            return ToBase64(value, encoding)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        public static string FromBase64(string value) {
            return FromBase64(value, DefaultEncoding);
        }

        public static string FromBase64(string value, Encoding encoding) {
            value = Regex.Replace(value, "\\s+", string.Empty, RegexOptions.Compiled);
            if (value.Contains(".")) {
                var text = new StringBuilder();
                using (var memory = new MemoryStream()) {
                    var options = StringSplitOptions.RemoveEmptyEntries;
                    var parts = value.Split(new char[] { '.' }, options);
                    for (int i = 0; i < parts.Length; i++) {
                        var origin = Convert.FromBase64String(NormalizeBase64(parts[i]));
                        text.AppendLine(encoding.GetString(origin));
                        memory.Write(origin, 0, origin.Length);
                    }
                    return text.ToString();
                }
            }
            else {
                return encoding.GetString(
                    Convert.FromBase64String(
                        NormalizeBase64(value)
                        ));
            }
        }

        public static string UrlEncode(string value) {
            return value != null ? WebUtility.UrlEncode(value) : null;
        }

        public static string UrlDecode(string value) {
            return value != null ? WebUtility.UrlDecode(value) : null;
        }

        public static string HtmlEncode(string value) {
            return value != null ? WebUtility.HtmlEncode(value) : null;
        }

        public static string HtmlDecode(string value) {
            return value != null ? WebUtility.HtmlDecode(value) : null;
        }

        public static string HexEncode(string value) {
            return HexEncode(value, DefaultEncoding);
        }

        public static string HexEncode(string value, Encoding encoding) {
            var text = new StringBuilder();
            foreach (var b in encoding.GetBytes(value)) {
                text.AppendFormat("%{0:x2}", b);
            }
            return text.ToString();
        }

        public static string HexDecode(string value) {
            return HexDecode(value, DefaultEncoding);
        }

        public static string HexDecode(string value, Encoding encoding) {
            using (var memory = new MemoryStream()) {
                var count = -1;
                var index = 0;
                while (index < value.Length) {
                    var c = value[index];
                    if (c <= ' ') {
                        switch (c) {
                            case '\t':
                            case '\n':
                            case '\r':
                                break;
                            case '\v':
                            case '\f':
                                goto IL_27E;
                            default:
                                if (c != ' ') {
                                    goto IL_27E;
                                }
                                break;
                        }
                    }
                    else if (c != '$' && c != '%') {
                        goto IL_27E;
                    }
                IL_2AA:
                    index++;
                    continue;
                IL_27E:
                    int num2 = CharToInt(c);
                    if (num2 < 0) {
                        goto IL_2AA;
                    }
                    if (count < 0) {
                        count = num2;
                        goto IL_2AA;
                    }
                    memory.WriteByte((byte)(count * 16 + num2));
                    count = -1;
                    goto IL_2AA;
                }
                return encoding.GetString(memory.ToArray());
            }

        }

        public static string ToJavaScript(string value) {
            var text = new StringBuilder(value);
            text.Replace("\\", "\\\\");
            text.Replace("\r", "\\r");
            text.Replace("\n", "\\n");
            text.Replace("\"", "\\\"");
            value = text.ToString();
            text = new StringBuilder();
            foreach (var c in value) {
                if ('\u007f' < c) {
                    text.AppendFormat("\\u{0:X4}", (int)c);
                }
                else {
                    text.Append(c);
                }
            }
            return text.ToString();
        }

        public static string FromJavaScript(string value) {
            if (!string.IsNullOrEmpty(value) && value.Contains("\\")) {
                var text = new StringBuilder();
                int length = value.Length;
                int i = 0;
                while (i < length) {
                    char c = value[i];
                    if (c != '\\') {
                        goto IL_188;
                    }
                    if (i < length - 5 && value[i + 1] == 'u') {
                        int num = CharToInt(value[i + 2]);
                        int num2 = CharToInt(value[i + 3]);
                        int num3 = CharToInt(value[i + 4]);
                        int num4 = CharToInt(value[i + 5]);
                        if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0) {
                            goto IL_188;
                        }
                        c = (char)(num << 12 | num2 << 8 | num3 << 4 | num4);
                        i += 5;
                        text.Append(c);
                    }
                    else if (i < length - 3 && value[i + 1] == 'x') {
                        int num5 = CharToInt(value[i + 2]);
                        int num6 = CharToInt(value[i + 3]);
                        if (num5 < 0 || num6 < 0) {
                            goto IL_188;
                        }
                        c = (char)(num5 << 4 | num6);
                        i += 3;
                        text.Append(c);
                    }
                    else {
                        if (i >= length - 1) {
                            goto IL_188;
                        }
                        char c2 = value[i + 1];
                        if (c2 != '\\') {
                            if (c2 != 'n') {
                                if (c2 != 't') {
                                    goto IL_188;
                                }
                                text.Append("\t");
                                i++;
                            }
                            else {
                                text.Append("\n");
                                i++;
                            }
                        }
                        else {
                            text.Append("\\");
                            i++;
                        }
                    }
                IL_190:
                    i++;
                    continue;
                IL_188:
                    text.Append(c);
                    goto IL_190;
                }
                return text.ToString();
            }
            return value;
        }

        public static string ToMD5(string value) {
            return ToMD5(value, DefaultEncoding);
        }

        public static string ToMD5(string value, Encoding encoding) {
            return ToMD5(encoding.GetBytes(value));
        }

        public static string ToMD5(params byte[] bytes) {
            return MD5.Create().ToHash(bytes);
        }

        public static string ToSHA1(string value) {
            return ToSHA1(value, DefaultEncoding);
        }

        public static string ToSHA1(string value, Encoding encoding) {
            return ToSHA1(encoding.GetBytes(value));
        }

        public static string ToSHA1(params byte[] bytes) {
            return SHA1.Create().ToHash(bytes);
        }

        public static string ToSHA256(string value) {
            return ToSHA256(value, DefaultEncoding);
        }

        public static string ToSHA256(string value, Encoding encoding) {
            return ToSHA256(encoding.GetBytes(value));
        }

        public static string ToSHA256(params byte[] bytes) {
            return SHA256.Create().ToHash(bytes);
        }

        public static string ToSHA384(string value) {
            return ToSHA384(value, DefaultEncoding);
        }

        public static string ToSHA384(string value, Encoding encoding) {
            return ToSHA384(encoding.GetBytes(value));
        }

        public static string ToSHA384(params byte[] bytes) {
            return SHA384.Create().ToHash(bytes);
        }

        public static string ToSHA512(string value) {
            return ToSHA512(value, DefaultEncoding);
        }

        public static string ToSHA512(string value, Encoding encoding) {
            return ToSHA512(encoding.GetBytes(value));
        }

        public static string ToSHA512(params byte[] bytes) {
            return SHA512.Create().ToHash(bytes);
        }

        public static int HexToInt(char h) {
            if (h >= '0' && h <= '9') {
                return (int)(h - '0');
            }
            if (h >= 'a' && h <= 'f') {
                return (int)(h - 'a' + '\n');
            }
            if (h < 'A' || h > 'F') {
                return -1;
            }
            return (int)(h - 'A' + '\n');
        }

        public static char IntToHex(int n) {
            return n <= 9
                ? (char)(n + 48)
                : (char)(n - 10 + 97);
        }

        private static string ToHash(this HashAlgorithm algorithm, params byte[] bytes) {
            using (algorithm) {
                return BitConverter.ToString(
                    algorithm.ComputeHash(bytes)
                    );
            }
        }

        private static string NormalizeBase64(string value) {
            value = value.Replace('-', '+').Replace('_', '/');
            switch (value.Length % 4) {
                case 0: return value;
                case 2: return value + "==";
                case 3: return value + "=";
            }
            var message = "Invalid length for a base64 string";
            throw new InvalidDataException(message);
        }

        private static int CharToInt(char value) {
            if (value >= '0' && value <= '9') {
                return (int)(value - '0');
            }
            if (value >= 'a' && value <= 'f') {
                return (int)(value - 'a' + '\n');
            }
            if (value >= 'A' && value <= 'F') {
                return (int)(value - 'A' + '\n');
            }
            return -1;
        }

        private static byte[] GenerateEntropy256() {
            var randomBytes = new byte[32];
            using (var rngCsp = new RNGCryptoServiceProvider()) {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
