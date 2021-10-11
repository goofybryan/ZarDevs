using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.Http.Api.Serializers
{
    /// <summary>
    /// A parser that will try and take a <see cref="object"/> and cast it to various enumerables of <see cref="Tuple{T1, T2}"/>, <see cref="ValueTuple{T1, T2}"/> or <see cref="KeyValuePair{TKey, TValue}"/> where key and value is a string.
    /// </summary>
    public interface IDefaultFormUrlEncodedContentParser
    {
        /// <summary>
        /// Parse the <paramref name="value"/> to the appropriate value.
        /// </summary>
        /// <param name="value">The value to parse</param>
        /// <param name="parsed">The parsed value</param>
        /// <returns>Returns a true of the value was parsed.</returns>
        bool TryParse(object value, out IEnumerable<KeyValuePair<string, string>> parsed);
    }

    /// <summary>
    /// A parser that will try and take a <see cref="object"/> and cast it to various enumerables of <see cref="Tuple{T1, T2}"/>, <see cref="ValueTuple{T1, T2}"/> or <see cref="KeyValuePair{TKey, TValue}"/> where key and value is a string.
    /// </summary>
    public class DefaultFormUrlEncodedContentParser : IDefaultFormUrlEncodedContentParser
    {
        /// <summary>
        /// Parse the <paramref name="value"/> to the appropriate value.
        /// </summary>
        /// <param name="value">The value to parse</param>
        /// <param name="parsed">The parsed value</param>
        /// <returns>Returns a true of the value was parsed.</returns>
        public bool TryParse(object value, out IEnumerable<KeyValuePair<string, string>> parsed)
        {
            parsed = null;

            if (value is IEnumerable<KeyValuePair<string, string>> kv)
                parsed = kv;

            if (value is IEnumerable<(string key, string value)> nv)
                parsed = nv.Select(i => new KeyValuePair<string, string>(i.key, i.value)).ToArray();

            if (value is IEnumerable<Tuple<string, string>> t)
                parsed = t.Select(i => new KeyValuePair<string, string>(i.Item1, i.Item2)).ToArray();

            return parsed != null;
        }
    }
}