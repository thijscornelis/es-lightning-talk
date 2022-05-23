using ES.Framework.Domain.Events.Design;

namespace ES.Framework.Tests.Mocks;

public record TestAggregateDescriptionChanged(TestAggregateId Id, string Description) : IEvent;