using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Events.Design;

namespace ES.Framework.Tests.Mocks;

public class TestAggregate : Aggregate<TestAggregateId, TestAggregateState>
{
	/// <inheritdoc />
	protected internal TestAggregate() {
		Handle<TestAggregateCreated>(OnCreated);
		Handle<TestAggregateDescriptionChanged>(OnDescriptionChanged);
	}

	private TestAggregateState OnDescriptionChanged(TestAggregateDescriptionChanged @event) => State with {
		Description = @event.Description
	};

	private TestAggregateState OnCreated(TestAggregateCreated @event) => State with {
		Id = @event.Id,
		Name = @event.Name
	};

	public TestAggregate(string name) : this(TestAggregateId.CreateNew(), name) { }
	public TestAggregate(TestAggregateId id, string name) : this() => Apply(new TestAggregateCreated(id, name));

	public void SetDescription(string value) {
		if(string.IsNullOrWhiteSpace(value))
			return;
		if(State.Description != null && State.Description.Equals(value))
			return;
		Apply(new TestAggregateDescriptionChanged(Id, value));
	}
}