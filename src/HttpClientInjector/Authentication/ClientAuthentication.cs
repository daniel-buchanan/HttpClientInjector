using System.Net.Http;

namespace HttpClientInjector.Authentication;

public abstract class ClientAuthentication(AuthenticationType kind) : 
    IClientAuthentication
{
    public AuthenticationType Kind { get; } = kind;

    public void Apply(HttpClient client)
        => ApplyInternal(client);

    protected abstract void ApplyInternal(HttpClient client);
}