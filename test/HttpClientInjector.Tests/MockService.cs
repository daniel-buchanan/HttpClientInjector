namespace HttpClientInjector.Tests;

public class MockService(HttpClient<MockService> client)
{
    public HttpClient Client => client.Client;
}