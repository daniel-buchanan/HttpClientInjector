using System.Net.Http;
using System.Net.Http.Headers;

namespace HttpClientInjector.Authentication;

public class BearerAuthentication() : 
    ClientAuthentication(AuthenticationType.Bearer), 
    IBearerAuthentication
{
    public static IBearerAuthentication Create(string token = null)
        => new BearerAuthentication().SetCredential(token);
    
    protected override void ApplyInternal(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(Kind.ToString(), Token);
    }

    public string Token { get; private set; }

    public IBearerAuthentication SetCredential(string token)
    {
        Token = token;
        return this;
    }
}