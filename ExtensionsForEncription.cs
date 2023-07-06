using System.Text;
using System.Security.Cryptography;
using JWT;
using JWT.Algorithms;

namespace AppEnd
{
    public static class ExtensionsForEncription
    {
        private static JWT.Serializers.JsonNetSerializer jsonNetSerializer = new JWT.Serializers.JsonNetSerializer();
        private static JwtBase64UrlEncoder jwtBase64UrlEncoder = new JwtBase64UrlEncoder();
        private static HMACSHA512Algorithm hMACSHA256Algorithm = new HMACSHA512Algorithm();
        private static UtcDateTimeProvider utcDateTimeProvider = new UtcDateTimeProvider();
        private static JwtValidator jwtValidator = new JwtValidator(jsonNetSerializer, utcDateTimeProvider);
        private static JwtEncoder enc = new JwtEncoder(hMACSHA256Algorithm, jsonNetSerializer, jwtBase64UrlEncoder);
        private static JwtDecoder dec = new JwtDecoder(jsonNetSerializer, jwtValidator, jwtBase64UrlEncoder, hMACSHA256Algorithm);

        public static string Encode(this object o, string secret)
        {
            return enc.Encode(null, o, Encoding.Unicode.GetBytes(secret));
        }

        public static string Decode(this string token, string secret)
        {
            if (token.FixNullOrEmpty("null").Equals("null")) new AppEndException("InputObjectForEncodingCanNotBeNullOrEmpty");
            JwtParts jpS;
            jpS = new JwtParts(token);
            return dec.Decode(jpS, Encoding.Unicode.GetBytes(secret), true);
        }

        public static string GetMD5Hash(this string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public static string GetMD4Hash(this string input)
        {
            return Md4Hash(input);
        }

        private static string Md4Hash(string input)
        {
            // get padded uints from bytes
            List<byte> bytes = Encoding.ASCII.GetBytes(input).ToList();
            uint bitCount = (uint)(bytes.Count) * 8;
            bytes.Add(128);
            while (bytes.Count % 64 != 56) bytes.Add(0);
            var uints = new List<uint>();
            for (int i = 0; i + 3 < bytes.Count; i += 4)
                uints.Add(bytes[i] | (uint)bytes[i + 1] << 8 | (uint)bytes[i + 2] << 16 | (uint)bytes[i + 3] << 24);
            uints.Add(bitCount);
            uints.Add(0);

            // run rounds
            uint a = 0x67452301, b = 0xefcdab89, c = 0x98badcfe, d = 0x10325476;
            Func<uint, uint, uint> rol = (x, y) => x << (int)y | x >> 32 - (int)y;
            for (int q = 0; q + 15 < uints.Count; q += 16)
            {
                var chunk = uints.GetRange(q, 16);
                uint aa = a, bb = b, cc = c, dd = d;
                Action<Func<uint, uint, uint, uint>, uint[]> round = (f, y) =>
                {
                    foreach (uint i in new[] { y[0], y[1], y[2], y[3] })
                    {
                        a = rol(a + f(b, c, d) + chunk[(int)(i + y[4])] + y[12], y[8]);
                        d = rol(d + f(a, b, c) + chunk[(int)(i + y[5])] + y[12], y[9]);
                        c = rol(c + f(d, a, b) + chunk[(int)(i + y[6])] + y[12], y[10]);
                        b = rol(b + f(c, d, a) + chunk[(int)(i + y[7])] + y[12], y[11]);
                    }
                };
                round((x, y, z) => (x & y) | (~x & z), new uint[] { 0, 4, 8, 12, 0, 1, 2, 3, 3, 7, 11, 19, 0 });
                round((x, y, z) => (x & y) | (x & z) | (y & z), new uint[] { 0, 1, 2, 3, 0, 4, 8, 12, 3, 5, 9, 13, 0x5a827999 });
                round((x, y, z) => x ^ y ^ z, new uint[] { 0, 2, 1, 3, 0, 8, 4, 12, 3, 9, 11, 15, 0x6ed9eba1 });
                a += aa; b += bb; c += cc; d += dd;
            }

            // return hex encoded string
            byte[] outBytes = new[] { a, b, c, d }.SelectMany(BitConverter.GetBytes).ToArray();
            return BitConverter.ToString(outBytes).Replace("-", "").ToUpper();
        }


    }
}