using System.Net.Http;

namespace HttpClientInjector;

public interface IHttp<T>
{
    public string TypeName { get; }
    public HttpClient Client { get; }
}

public interface IHttp<TInterface, TImplementation> : IHttp<TInterface>;

public class Http<T>(HttpClient client) : IHttp<T>
{
    public string TypeName { get; } = typeof(T).Name;
    public HttpClient Client { get; } = client;
}

public class Http<TInterface, TImplementation>(HttpClient client) : 
    Http<TInterface>(client), IHttp<TInterface, TImplementation>;