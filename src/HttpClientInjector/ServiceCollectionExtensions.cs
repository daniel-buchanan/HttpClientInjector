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
        private static string GetHttpClientName<T>()
        {
            var type = typeof(T);
            var name = $"{type.Namespace}.{type.Name}";
            return name;
        }
        
        /// <summary>
        /// Inject an HttpClient for the given type <see cref="T"/>.
        /// This means that in your constructor for <see cref="T"/> you can add <see cref="IHttp{T}"/> as a parameter.
        /// </summary>
        /// <param name="self">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="builder">The builder method to configure the <see cref="HttpClient"/>.</param>
        /// <typeparam name="T">The type to register the HttpClient for.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/> acted on, for fluent usage.</returns>
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

        /// <summary>
        /// Inject an HttpClient for the given type <see cref="TInterface"/> and <see cref="TImplementation"/>.
        /// This means that in your constructor for <see cref="TImplementation"/> you can add <see cref="IHttp{TInterface, TImplementation}"/> as a parameter.
        /// </summary>
        /// <param name="self">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="builder">The builder method to configure the <see cref="HttpClient"/>.</param>
        /// <typeparam name="TInterface">The interface to with this should be registered</typeparam>
        /// <typeparam name="TImplementation">The implementation into which this <see cref="IHttp{TInterface, TImplementation}"/> should be registered.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/> acted on, for fluent usage.</returns>
        public static IServiceCollection InjectHttpClientFor<TInterface, TImplementation>(
            this IServiceCollection self,
            Action<IHttpClientConfigurationBuilder> builder) 
            where TInterface : class 
            where TImplementation : class, TInterface
        {
            if (self.All(s => s.ServiceType != typeof(IHttpClientFactory)))
                self.AddHttpClient();
            
            self.AddHttpClient<IHttp<TInterface>, Http<TInterface>>(GetHttpClientName<TInterface>(), 
                (provider, client) =>
                {
                    var internalBuilder = new HttpClientConfigurationBuilder(provider);
                    builder(internalBuilder);
                    internalBuilder.Apply(client);
                });
            self.AddHttpClient<IHttp<TImplementation>, Http<TImplementation>>(GetHttpClientName<TImplementation>(), 
                (provider, client) =>
                {
                    var internalBuilder = new HttpClientConfigurationBuilder(provider);
                    builder(internalBuilder);
                    internalBuilder.Apply(client);
                });
            
            return self;
        }
        
        /// <summary>
        /// Inject an HttpClient for the given type <see cref="T"/>.
        /// This means that in your constructor for <see cref="T"/> you can add <see cref="IHttp{T}"/> as a parameter.
        /// </summary>
        /// <param name="self">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="baseUrl">The base url for this <see cref="HttpClient" />.</param>
        /// <typeparam name="T">The type to register the HttpClient for.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/> acted on, for fluent usage.</returns>
        public static IServiceCollection InjectHttpClientFor<T>(
            this IServiceCollection self, 
            string baseUrl)
            where T : class 
            => self.InjectHttpClientFor<T>(b 
                => b.WithBaseUrl(baseUrl).WithoutAuthentication());

        /// <summary>
        /// Inject an HttpClient for the given type <see cref="TInterface"/> and <see cref="TImplementation"/>.
        /// This means that in your constructor for <see cref="TImplementation"/> you can add <see cref="IHttp{TInterface, TImplementation}"/> as a parameter.
        /// </summary>
        /// <param name="self">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="baseUrl">The base url for this <see cref="HttpClient" />.</param>
        /// <typeparam name="TInterface">The interface to with this should be registered</typeparam>
        /// <typeparam name="TImplementation">The implementation into which this <see cref="IHttp{TInterface, TImplementation}"/> should be registered.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/> acted on, for fluent usage.</returns>
        public static IServiceCollection InjectHttpClientFor<TInterface, TImplementation>(
            this IServiceCollection self, 
            string baseUrl)
            where TInterface : class 
            where TImplementation : class, TInterface
            => self.InjectHttpClientFor<TInterface, TImplementation>(b 
                => b.WithBaseUrl(baseUrl).WithoutAuthentication());
        
        /// <summary>
        /// Inject an HttpClient for the given type <see cref="T"/>.
        /// This means that in your constructor for <see cref="T"/> you can add <see cref="IHttp{T}"/> as a parameter.
        /// </summary>
        /// <param name="self">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="baseUrl">The base url for this <see cref="HttpClient" />.</param>
        /// <param name="builder">The builder method to configure the <see cref="HttpClient"/>.</param>
        /// <typeparam name="T">The type to register the HttpClient for.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/> acted on, for fluent usage.</returns>
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
        
        /// <summary>
        /// Inject an HttpClient for the given type <see cref="TInterface"/> and <see cref="TImplementation"/>.
        /// This means that in your constructor for <see cref="TImplementation"/> you can add <see cref="IHttp{TInterface, TImplementation}"/> as a parameter.
        /// </summary>
        /// <param name="self">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="baseUrl">The base url for this <see cref="HttpClient" />.</param>
        /// <param name="builder">The builder method to configure the <see cref="HttpClient"/>.</param>
        /// <typeparam name="TInterface">The interface to with this should be registered</typeparam>
        /// <typeparam name="TImplementation">The implementation into which this <see cref="IHttp{TInterface, TImplementation}"/> should be registered.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/> acted on, for fluent usage.</returns>
        public static IServiceCollection InjectHttpClientFor<TInterface, TImplementation>(
            this IServiceCollection self, 
            string baseUrl,
            Action<IHttpClientConfigurationBuilder> builder)
            where TInterface : class 
            where TImplementation : class, TInterface
            => self.InjectHttpClientFor<TInterface, TImplementation>(b =>
            {
                b.WithBaseUrl(baseUrl);
                builder(b);
            });
        
        /// <summary>
        /// Inject an HttpClient for the given type <see cref="T"/>.
        /// This means that in your constructor for <see cref="T"/> you can add <see cref="IHttp{T}"/> as a parameter.
        /// </summary>
        /// <param name="self">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="baseUrl">The base url for this <see cref="HttpClient" />.</param>
        /// <param name="getBearerToken">A function which returns an instance of <see cref="IBearerAuthentication"/>, used to configure the <see cref="HttpClient"/>.</param>
        /// <typeparam name="T">The type to register the HttpClient for.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/> acted on, for fluent usage.</returns>
        public static IServiceCollection InjectHttpClientFor<T>(
            this IServiceCollection self, 
            string baseUrl, 
            Func<IServiceProvider, IBearerAuthentication> getBearerToken)
            where T : class 
            => self.InjectHttpClientFor<T>(b 
                => b.WithBaseUrl(baseUrl)
                    .WithBearerAuthentication(getBearerToken));
        
        /// <summary>
        /// Inject an HttpClient for the given type <see cref="TInterface"/> and <see cref="TImplementation"/>.
        /// This means that in your constructor for <see cref="TImplementation"/> you can add <see cref="IHttp{TInterface, TImplementation}"/> as a parameter.
        /// </summary>
        /// <param name="self">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="baseUrl">The base url for this <see cref="HttpClient" />.</param>
        /// <param name="getBearerToken">A function which returns an instance of <see cref="IBearerAuthentication"/>, used to configure the <see cref="HttpClient"/>.</param>
        /// <typeparam name="TInterface">The interface to with this should be registered</typeparam>
        /// <typeparam name="TImplementation">The implementation into which this <see cref="IHttp{TInterface, TImplementation}"/> should be registered.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/> acted on, for fluent usage.</returns>
        public static IServiceCollection InjectHttpClientFor<TInterface, TImplementation>(
            this IServiceCollection self, 
            string baseUrl, 
            Func<IServiceProvider, IBearerAuthentication> getBearerToken)
            where TInterface : class 
            where TImplementation : class, TInterface
            => self.InjectHttpClientFor<TInterface, TImplementation>(b 
                => b.WithBaseUrl(baseUrl)
                    .WithBearerAuthentication(getBearerToken));
        
        /// <summary>
        /// Inject an HttpClient for the given type <see cref="T"/>.
        /// This means that in your constructor for <see cref="T"/> you can add <see cref="IHttp{T}"/> as a parameter.
        /// </summary>
        /// <param name="self">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="baseUrl">The base url for this <see cref="HttpClient" />.</param>
        /// <param name="getBasicCredentials">A function which returns an instance of <see cref="IBasicAuthentication"/>, used to configure the <see cref="HttpClient"/>.</param>
        /// <typeparam name="T">The type to register the HttpClient for.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/> acted on, for fluent usage.</returns>
        public static IServiceCollection InjectHttpClientFor<T>(
            this IServiceCollection self, 
            string baseUrl, 
            Func<IServiceProvider, IBasicAuthentication> getBasicCredentials)
            where T : class 
            => self.InjectHttpClientFor<T>(b 
                => b.WithBaseUrl(baseUrl)
                    .WithBasicAuthentication(getBasicCredentials));
        
        /// <summary>
        /// Inject an HttpClient for the given type <see cref="TInterface"/> and <see cref="TImplementation"/>.
        /// This means that in your constructor for <see cref="TImplementation"/> you can add <see cref="IHttp{TInterface, TImplementation}"/> as a parameter.
        /// </summary>
        /// <param name="self">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="baseUrl">The base url for this <see cref="HttpClient" />.</param>
        /// <param name="getBasicCredentials">A function which returns an instance of <see cref="IBasicAuthentication"/>, used to configure the <see cref="HttpClient"/>.</param>
        /// <typeparam name="TInterface">The interface to with this should be registered</typeparam>
        /// <typeparam name="TImplementation">The implementation into which this <see cref="IHttp{TInterface, TImplementation}"/> should be registered.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/> acted on, for fluent usage.</returns>
        public static IServiceCollection InjectHttpClientFor<TInterface, TImplementation>(
            this IServiceCollection self, 
            string baseUrl, 
            Func<IServiceProvider, IBasicAuthentication> getBasicCredentials)
            where TInterface : class 
            where TImplementation : class, TInterface
            => self.InjectHttpClientFor<TInterface, TImplementation>(b 
                => b.WithBaseUrl(baseUrl)
                    .WithBasicAuthentication(getBasicCredentials));

        /// <summary>
        /// Get an <see cref="IHttp{T}"/> for the provided service <see cref="T"/>.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> to fetch from.</param>
        /// <typeparam name="T">The type of the service to get an <see cref="IHttp{T}"/>.</typeparam>
        /// <returns>An instance of <see cref="IHttp{T}"/> or NULL.</returns>
        public static IHttp<T> GetHttpFor<T>(this IServiceProvider provider)
            where T : class
            => provider.GetService<IHttp<T>>();
        
        /// <summary>
        /// Get an <see cref="IHttp{TInterface, TImplementation}"/> for the provided service <see cref="TInterface"/>.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> to fetch from.</param>
        /// <typeparam name="TInterface">The interface type for <see cref="IHttp{TInterface, TImplementation}"/>.</typeparam>
        /// <typeparam name="TImplementation">The implementation type for <see cref="IHttp{TInterface, TImplementation}"/>.</typeparam>
        /// <returns>An instance of <see cref="IHttp{TInterface, TImplementation}"/> or NULL.</returns>
        public static IHttp<TInterface, TImplementation> GetHttpFor<TInterface, TImplementation>(this IServiceProvider provider)
            where TInterface : class
            where TImplementation : class
            => provider.GetService<IHttp<TInterface, TImplementation>>();
    }
}