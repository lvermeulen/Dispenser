using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable once RedundantUsingDirective
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Dispenser
{
    public abstract class HasherBase : IHasher
    {
        private IEnumerable<string> _excludePropertyNames;

        protected abstract HashAlgorithm HashAlgorithm { get; }

        public string Hash(object obj, IEnumerable<string> excludePropertyNames = null, Encoding encoding = null)
        {
            if (obj == null)
            {
                return "";
            }

            _excludePropertyNames = excludePropertyNames;
            if (encoding == null)
            {
                encoding = Encoding.ASCII;
            }

            // get properties with values to hash
            var props = obj.GetType()
                .GetProperties()
                .Where(p => _excludePropertyNames == null || !_excludePropertyNames.Contains(p.Name, StringComparer.OrdinalIgnoreCase));

            // get property values to hash
            string hashSource = string.Join("þ", props.Select(x => x.GetValue(obj)?.ToString() ?? ""));

            // hash values
            var hashBytes = HashAlgorithm.ComputeHash(encoding.GetBytes(hashSource));

            // convert bytes to string
            var sb = new StringBuilder();
            foreach (byte hashByte in hashBytes)
            {
                sb.Append(hashByte.ToString("X2"));
            }

            return "0x" + sb;
        }
    }
}
