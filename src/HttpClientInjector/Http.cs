using System.Net.Http;

namespace HttpClientInjector;

public interface IHttp<T>
{
    public string TypeName { get; }
    public HttpClient Client { get; }
}

public class Http<T>(HttpClient client) : IHttp<T>
{
    public string TypeName { get; } = typeof(T).Name;
    public HttpClient Client { get; } = client;
}