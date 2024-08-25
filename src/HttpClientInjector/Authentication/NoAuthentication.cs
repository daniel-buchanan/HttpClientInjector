using System.Net.Http;

namespace HttpClientInjector.Authentication;

public class NoAuthentication() : 
    ClientAuthentication(AuthenticationType.None), 
    INoAuthentication
{
    protected override void ApplyInternal(HttpClient client) { }
}