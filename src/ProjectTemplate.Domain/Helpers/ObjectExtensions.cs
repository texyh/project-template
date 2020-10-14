using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ProjectTemplate.Domain.Helpers
{
    public static class ObjectExtensions
    {
        public static T FromJson<T>(this string source, bool caseSensitive = false)
        {
            if (caseSensitive)
            {
                return JsonSerializer.Deserialize<T>(source);
            }

            var settings = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(source, settings);
        }

        public static string ToJson<T>(this T data, bool camelCasing = false)
        {
            if (!camelCasing)
            {
                return JsonSerializer.Serialize(data);
            }

            var settings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(data, settings);
        }

        public static StringContent ToStringContent<T>(this T data)
        {
            return new StringContent(data.ToJson(), Encoding.UTF8, "application/json");
        }
    }
}
