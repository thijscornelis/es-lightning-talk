using ES.Framework.Domain.Aggregates.Design;

namespace ES.Framework.Tests.Mocks;

public record TestAggregateState : IAggregateState<TestAggregateId>
{
	/// <inheritdoc />
	public TestAggregateId Id { get; init; }

	public string Name { get; init; }
	public string Description { get; init; }
}