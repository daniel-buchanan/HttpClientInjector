using System;
using System.Linq;
using System.Net.Http;
using HttpClientInjector.Authentication;
using HttpClientInjector.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientInjector
{
    public static class ServiceCollectionExtensions
    {
        private static string GetHttpClientName<TInterface>()
        {
            var type = typeof(TInterface);
            var name = $"{type.Namespace}.{type.Name}";
            return name;
        }
        
        public static IServiceCollection InjectHttpClientFor<T>(
            this IServiceCollection self,
            Action<IHttpClientConfigurationBuilder> builder)
            where T : class
        {
            if (self.All(s => s.ServiceType != typeof(IHttpClientFactory)))
                self.AddHttpClient();
            
            var name = GetHttpClientName<T>();
            self.AddHttpClient<IHttp<T>, Http<T>>(name, (provider, client) =>
            {
                var internalBuilder = new HttpClientConfigurationBuilder(provider);
                builder(internalBuilder);
                internalBuilder.Apply(client);
            });
            
            return self;
        }

        public static IServiceCollection InjectHttpClientFor<TInterface, TImplementation>(
            this IServiceCollection self,
            Action<IHttpClientConfigurationBuilder> builder)
        {
            if (self.All(s => s.ServiceType != typeof(IHttpClientFactory)))
                self.AddHttpClient();
            
            var name = GetHttpClientName<TInterface>();
            self.AddHttpClient<IHttp<TInterface, TImplementation>, Http<TInterface, TImplementation>>(name, (provider, client) =>
            {
                var internalBuilder = new HttpClientConfigurationBuilder(provider);
                builder(internalBuilder);
                internalBuilder.Apply(client);
            });
            
            return self;
        }
        
        public static IServiceCollection InjectHttpClientFor<T>(
            this IServiceCollection self, 
            string baseUrl)
            where T : class 
            => self.InjectHttpClientFor<T>(b 
                => b.WithBaseUrl(baseUrl).WithoutAuthentication());

        public static IServiceCollection InjectHttpClientFor<TInterface, TImplementation>(
            this IServiceCollection self, 
            string baseUrl)
            where TInterface : class
            where TImplementation : class
            => self.InjectHttpClientFor<TInterface, TImplementation>(b 
                => b.WithBaseUrl(baseUrl).WithoutAuthentication());
        
        public static IServiceCollection InjectHttpClientFor<T>(
            this IServiceCollection self,
            string baseUrl,
            Action<IHttpClientConfigurationBuilder> builder)
            where T : class
            => self.InjectHttpClientFor<T>((b =>
                {
                    b.WithBaseUrl(baseUrl);
                    builder(b);
                }));
        
        public static IServiceCollection InjectHttpClientFor<TInterface, TImplementation>(
            this IServiceCollection self, 
            string baseUrl,
            Action<IHttpClientConfigurationBuilder> builder)
            where TInterface : class
            where TImplementation : class
            => self.InjectHttpClientFor<TInterface, TImplementation>(b =>
            {
                b.WithBaseUrl(baseUrl);
                builder(b);
            });
        
        public static IServiceCollection InjectHttpClientFor<T>(
            this IServiceCollection self, 
            string baseUrl, 
            Func<IServiceProvider, IBearerAuthentication> getBearerToken)
            where T : class 
            => self.InjectHttpClientFor<T>(b 
                => b.WithBaseUrl(baseUrl)
                    .WithBearerAuthentication(getBearerToken));
        
        public static IServiceCollection InjectHttpClientFor<TInterface, TImplementation>(
            this IServiceCollection self, 
            string baseUrl, 
            Func<IServiceProvider, IBearerAuthentication> getBearerToken)
            where TInterface : class
            where TImplementation : class
            => self.InjectHttpClientFor<TInterface, TImplementation>(b 
                => b.WithBaseUrl(baseUrl)
                    .WithBearerAuthentication(getBearerToken));
        
        public static IServiceCollection InjectHttpClientFor<T>(
            this IServiceCollection self, 
            string baseUrl, 
            Func<IServiceProvider, IBasicAuthentication> getBasicCredentials)
            where T : class 
            => self.InjectHttpClientFor<T>(b 
                => b.WithBaseUrl(baseUrl)
                    .WithBasicAuthentication(getBasicCredentials));
        
        public static IServiceCollection InjectHttpClientFor<TInterface, TImplementation>(
            this IServiceCollection self, 
            string baseUrl, 
            Func<IServiceProvider, IBasicAuthentication> getBasicCredentials)
            where TInterface : class
            where TImplementation : class
            => self.InjectHttpClientFor<TInterface, TImplementation>(b 
                => b.WithBaseUrl(baseUrl)
                    .WithBasicAuthentication(getBasicCredentials));

        public static IHttp<T> GetHttpFor<T>(this IServiceProvider provider)
            where T : class
            => provider.GetService<IHttp<T>>();
        
        public static IHttp<TInterface, TImplementation> GetHttpFor<TInterface, TImplementation>(this IServiceProvider provider)
            where TInterface : class
            where TImplementation : class
            => provider.GetService<IHttp<TInterface, TImplementation>>();
    }
}