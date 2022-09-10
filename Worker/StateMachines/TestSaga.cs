using System;
using MassTransit;

namespace Worker.StateMachines;

public class TestSaga : SagaStateMachineInstance 
{
    public int CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid? ExpiredTimeoutToken { get; set; }
}