namespace HttpClientInjector.Tests;

public class MockService(IHttp<MockService> client)
{
    public HttpClient Client => client.Client;
}