using System.Net.Http;

namespace HttpClientInjector;

public interface IHttpClient<T>
{
    public string TypeName { get; }
    public HttpClient Client { get; }
}

public class HttpClient<T>(HttpClient client) : IHttpClient<T>
{
    public string TypeName { get; } = typeof(T).Name;
    public HttpClient Client { get; } = client;
}