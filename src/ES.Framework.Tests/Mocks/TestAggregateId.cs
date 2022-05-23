using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Framework.Tests.Mocks;

public record TestAggregateId : TypedIdentifier<TestAggregateId, Guid>
{
	/// <inheritdoc />
	public TestAggregateId(Guid value) : base(value) {
	}
}