using EventLib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventLib
{
    public static class EventLibInjector
    {
        public static void InjectServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();
            services.AddScoped<IConsumer, Consumer>();
            services.AddScoped<IDispatcher, Dispatcher>();
        }
    }
}
