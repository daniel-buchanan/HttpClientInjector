namespace HttpClientInjector.Tests;

public class MockService(IHttpClient<MockService> client)
{
    public HttpClient Client => client.Client;
}