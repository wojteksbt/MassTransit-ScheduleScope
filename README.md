# MassTransit Schedule Scope

## Description
This repository is created to reproduce MassTransit missing scope when scheduling issue.

## Steps to reproduce:
Simply run `Worker` project. It will automatically send an Initiate event to the `TestStateMachine`. That State machine will set some data in `ScopedService` and then it will attempt to send two messages: `OutgoingEvent` and `ExpiredEvent`. While `SendFilter` is able to read scoped data for the first message, it's `null` in case of the second one.

Expected output:
```
info: Worker.InitiateWorker[0]
      Initiate message has been sent
info: Worker.StateMachines.SetScopedValueActivity[0]
      Setting scoped value for d316cc63-72ba-4ff1-a70f-153ce89aaf43
info: Worker.SendFilter[0]
      Scoped value for Worker.StateMachines.OutgoingEvent is : Correct scoped value
info: Worker.SendFilter[0]
      Scoped value for Worker.StateMachines.ExpiredEvent is : Correct scoped value

```
Actual:
```
info: Worker.InitiateWorker[0]
      Initiate message has been sent
info: Worker.StateMachines.SetScopedValueActivity[0]
      Setting scoped value for d316cc63-72ba-4ff1-a70f-153ce89aaf43
info: Worker.SendFilter[0]
      Scoped value for Worker.StateMachines.OutgoingEvent is : Correct scoped value
info: Worker.SendFilter[0]
      Scoped value for Worker.StateMachines.ExpiredEvent is : (null)

```