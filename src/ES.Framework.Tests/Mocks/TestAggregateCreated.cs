using ES.Framework.Domain.Events.Design;

namespace ES.Framework.Tests.Mocks;

public record TestAggregateCreated : IAggregateEvent<TestAggregateId>
{
	 public string Name { get; init; }

	 /// <inheritdoc />
	 public TestAggregateId Id { get; init; }
	 /// <inheritdoc />
	 public long Version { get; set; }
}
