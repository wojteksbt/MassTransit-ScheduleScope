using System;
using MassTransit;

namespace Worker.StateMachines;

public class TestStateMachine : MassTransitStateMachine<TestSaga>
{
    public TestStateMachine()
    {
        InstanceState(x => x.CurrentState, Created);

        Event(() => InitiateEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
        Schedule(() => ExpiredTimeout, x => x.ExpiredTimeoutToken, x => x.Received = r => r.CorrelateById(s => s.Message.CorrelationId));

        Initially(
            When(InitiateEvent)
                // Here we set scope data
                .Activity(selector => selector.OfType<SetScopedValueActivity>())
                // Here Send filter is able to get scope data
                .Then(context => context.Send(new Uri("queue:another-queue"), new OutgoingEvent(context.Saga.CorrelationId)))
                // Here Send filter is not able to get scope data
                .Schedule(ExpiredTimeout, ctx => new ExpiredEvent(ctx.Saga.CorrelationId), _ => TimeSpan.FromSeconds(10))
                .Finalize()
        );
        
        SetCompletedWhenFinalized();
    }

    public State Created { get; private set; }

    public Event<InitiateEvent> InitiateEvent { get; private set; }
    public Schedule<TestSaga, ExpiredEvent> ExpiredTimeout { get; set; } = null!;
}

public record ExpiredEvent(Guid CorrelationId);
public record InitiateEvent(Guid CorrelationId);
public record OutgoingEvent(Guid CorrelationId);