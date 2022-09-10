using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Worker;

public class SendFilter<TMessage> : IFilter<SendContext<TMessage>> where TMessage : class
{
    private readonly ScopedService _scopedService;
    private readonly ILogger _logger;

    public SendFilter(ScopedService scopedService, ILogger<SendFilter<TMessage>> logger)
    {
        _scopedService = scopedService;
        _logger = logger;
    }

    public Task Send(SendContext<TMessage> context, IPipe<SendContext<TMessage>> next)
    {
        _logger.LogInformation("Scoped value for {Message} is : {Value}", typeof(TMessage), _scopedService.ScopedValue);

        return next.Send(context);
    }
    
    public void Probe(ProbeContext context) => throw new NotImplementedException();
}