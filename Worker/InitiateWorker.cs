using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Worker.StateMachines;

namespace Worker;

public class InitiateWorker : BackgroundService
{
    private readonly IBus _bus;
    private readonly ILogger _logger;

    public InitiateWorker(IBus bus, ILogger<InitiateWorker> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _bus.Publish(new  InitiateEvent(Guid.NewGuid()), stoppingToken);
            _logger.LogInformation("Initiate message has been sent");
            Console.ReadKey();
        }
    }
}