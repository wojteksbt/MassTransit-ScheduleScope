using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Worker.StateMachines;

public class SetScopedValueActivity : IStateMachineActivity<TestSaga, InitiateEvent>
{
    private readonly ScopedService _scopedService;
    private readonly ILogger _logger;

    public SetScopedValueActivity(ScopedService scopedService, ILogger<SetScopedValueActivity> logger)
    {
        _scopedService = scopedService;
        _logger = logger;
    }

    public async Task Execute(BehaviorContext<TestSaga, InitiateEvent> context, IBehavior<TestSaga, InitiateEvent> next)
    {
        _logger.LogInformation("Setting scoped value for {CorrelationId}", context.CorrelationId);
            
        _scopedService.ScopedValue = "Correct scoped value";

        await next.Execute(context);
    }

    public void Probe(ProbeContext context) => throw new NotImplementedException();
    public void Accept(StateMachineVisitor visitor) => throw new NotImplementedException();
    public Task Faulted<TException>(BehaviorExceptionContext<TestSaga, InitiateEvent, TException> context, IBehavior<TestSaga, InitiateEvent> next) where TException : Exception => throw new NotImplementedException();
}