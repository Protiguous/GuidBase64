﻿using System;

namespace GuidBase64
{
    public class Base64Guid
    {
        public Guid Guid { get; }

        public Base64Guid() => Guid = new Guid();

        public Base64Guid(Guid guid) => Guid = guid;

        public Base64Guid(byte[] buffer) => Guid = new Guid(buffer);

        public Base64Guid(string encoded) => Guid = new Guid(ParseToByteArray(encoded));

        public override string ToString()
        {
            string enc = Convert.ToBase64String(Guid.ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "-");
            return enc.Substring(0, 22);
        }

        public static Base64Guid NewBase64Guid() => new Base64Guid(Guid.NewGuid());

        public static Base64Guid Parse(string encoded) => new Base64Guid(encoded);

        private static byte[] ParseToByteArray(string encoded)
        {
            if (encoded is null)
            {
                throw new ArgumentNullException(nameof(encoded));
            }

            if (encoded.Length < 22 || encoded.Length > 22)
            {
                throw new FormatException("String length does not meet encoded GUID requirement (22 characters)");
            }

            encoded = encoded.Replace("_", "/");
            encoded = encoded.Replace("-", "+");

            return Convert.FromBase64String(encoded + "==");
        }
    }
}
