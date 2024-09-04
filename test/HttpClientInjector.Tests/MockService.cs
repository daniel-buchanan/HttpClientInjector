namespace HttpClientInjector.Tests;

public interface IMockService
{
    HttpClient Client { get; }
}

public class MockService(IHttp<IMockService> client) : IMockService
{
    public HttpClient Client => client.Client;
}

public class MockService2(IHttp<MockService2> client)
{
    public HttpClient Client => client.Client;
}