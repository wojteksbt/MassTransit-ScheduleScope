using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Worker;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services
                    .AddMassTransit(x =>
                    {
                        x.AddDelayedMessageScheduler();
                        x.SetKebabCaseEndpointNameFormatter();
                        x.SetInMemorySagaRepositoryProvider();

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        x.AddSagaStateMachines(entryAssembly);
                        x.AddSagas(entryAssembly);
                        x.AddActivities(entryAssembly);

                        x.UsingInMemory((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                            cfg.UseDelayedMessageScheduler();
                            
                            cfg.UseSendFilter(typeof(SendFilter<>), context);
                        });
                    });

                services
                    .AddScoped<ScopedService>()
                    .AddScoped(typeof(SendFilter<>))
                    .AddHostedService<InitiateWorker>();
            });
}