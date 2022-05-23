using ES.Framework.Domain.Events.Design;

namespace ES.Framework.Tests.Mocks;

public record TestAggregateCreated(TestAggregateId Id, string Name) : IEvent;