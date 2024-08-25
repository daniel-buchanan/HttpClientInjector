namespace HttpClientInjector.Authentication;

public interface IBearerAuthentication : IClientAuthentication
{
    string Token { get; }
    
    IBearerAuthentication SetCredential(string token);
}