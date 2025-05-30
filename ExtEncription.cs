﻿using System.Text;
using System.Security.Cryptography;
using JWT;
using JWT.Algorithms;

namespace AppEndCommon
{
    public static class ExtEncription
    {
        private static readonly JWT.Serializers.JsonNetSerializer JsonNetSerializer = new();
        private static readonly JwtBase64UrlEncoder JwtBase64UrlEncoder = new();
        private static readonly HMACSHA512Algorithm HMacsha256Algorithm = new();
        private static readonly UtcDateTimeProvider UtcDateTimeProvider = new();
        private static readonly JwtValidator JwtValidator = new(JsonNetSerializer, UtcDateTimeProvider);
        private static readonly JwtEncoder Enc = new(HMacsha256Algorithm, JsonNetSerializer, JwtBase64UrlEncoder);
        private static readonly JwtDecoder Dec = new(JsonNetSerializer, JwtValidator, JwtBase64UrlEncoder, HMacsha256Algorithm);

        public static string Encode(this object o, string secret)
        {
            return Enc.Encode(null, o, Encoding.Unicode.GetBytes(secret));
        }

        public static string Decode(this string token, string secret)
        {
            if (token.FixNullOrEmpty("null").Equals("null")) new ExtException("InputObjectForEncodingCanNotBeNullOrEmpty", System.Reflection.MethodBase.GetCurrentMethod()).GetEx();
            JwtParts jpS;
            jpS = new JwtParts(token);
            return Dec.Decode(jpS, Encoding.Unicode.GetBytes(secret), true);
        }

        public static string GetMD5Hash(this string input)
		{
			byte[] inputBytes = Encoding.ASCII.GetBytes(input);
			byte[] hash = MD5.HashData(inputBytes);
			StringBuilder sb = new();
			for (int i = 0; i < hash.Length; i++) sb.Append(hash[i].ToString("X2"));
			return sb.ToString();
		}
		public static string GetMD4Hash(this string input)
        {
            return Md4Hash(input);
        }

        private static string Md4Hash(string input)
        {
            // get padded uints from bytes
            List<byte> bytes = [.. Encoding.ASCII.GetBytes(input)];
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
			uint rol(uint x, uint y) => x << (int)y | x >> 32 - (int)y;
			for (int q = 0; q + 15 < uints.Count; q += 16)
            {
                var chunk = uints.GetRange(q, 16);
                uint aa = a, bb = b, cc = c, dd = d;
				void round(Func<uint, uint, uint, uint> f, uint[] y)
				{
					foreach (uint i in new[] { y[0], y[1], y[2], y[3] })
					{
						a = rol(a + f(b, c, d) + chunk[(int)(i + y[4])] + y[12], y[8]);
						d = rol(d + f(a, b, c) + chunk[(int)(i + y[5])] + y[12], y[9]);
						c = rol(c + f(d, a, b) + chunk[(int)(i + y[6])] + y[12], y[10]);
						b = rol(b + f(c, d, a) + chunk[(int)(i + y[7])] + y[12], y[11]);
					}
				}
				round((x, y, z) => (x & y) | (~x & z), [0, 4, 8, 12, 0, 1, 2, 3, 3, 7, 11, 19, 0]);
                round((x, y, z) => (x & y) | (x & z) | (y & z), [0, 1, 2, 3, 0, 4, 8, 12, 3, 5, 9, 13, 0x5a827999]);
                round((x, y, z) => x ^ y ^ z, [0, 2, 1, 3, 0, 8, 4, 12, 3, 9, 11, 15, 0x6ed9eba1]);
                a += aa; b += bb; c += cc; d += dd;
            }

            // return hex encoded string
            byte[] outBytes = new[] { a, b, c, d }.SelectMany(BitConverter.GetBytes).ToArray();
            return BitConverter.ToString(outBytes).Replace("-", "").ToUpper();
        }


    }
}