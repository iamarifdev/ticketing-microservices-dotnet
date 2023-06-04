using System.Collections;
using System.Text;
using System.Text.Json;

namespace Ticketing.Common.Extensions;

public static class HttpClientExtension
{
    public static async Task<HttpResponseMessage> GetJsonAsync(this HttpClient client, string requestUri,
        object? value = null)
    {
        if (value != null) requestUri += $"?{value.ToQueryString()}";

        return await client.GetAsync(requestUri);
    }

    public static async Task<HttpResponseMessage> PostJsonAsync(this HttpClient client, string requestUri, object value)
    {
        var stringContent = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        return await client.PostAsync(requestUri, stringContent);
    }


    public static async Task<HttpResponseMessage> PatchJsonAsync(this HttpClient client, string requestUri,
        object value)
    {
        var stringContent = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        return await client.PatchAsync(requestUri, stringContent);
    }


    public static async Task<HttpResponseMessage> PutJsonAsync(this HttpClient client, string requestUri, object value)
    {
        var stringContent = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        return await client.PutAsync(requestUri, stringContent);
    }

    private static string ToQueryString(this object? obj)
    {
        if (obj is null) return string.Empty;

        var properties = obj.GetType().GetProperties()
            .Where(p => p.GetValue(obj, null) != null);

        var queryString = string.Join("&", properties.Select(p =>
        {
            var value = p.GetValue(obj, null);

            if (value is IEnumerable items && !(value is string))
            {
                var queryStringParts = items
                    .Cast<object>()
                    .Select(i => $"{p.Name}={ToQueryString(i)}");

                return string.Join("&", queryStringParts);
            }

            return $"{p.Name}={value}";
        }));

        return queryString;
    }
}