using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Attributes;

namespace ES.Framework.Tests.Mocks;

[PartitionKey("{0}-{1}-{2}")]
public class TypeWithAggregatePartitionKeyAttribute
{ }

[PartitionKey("{3}-{1}-{2}")]
public class TypeWithAggregatePartitionKeyAttributeWithCustomFormat
{ }

public class TypeWithoutAttribute
{ }

[PartitionKey("{0}-{1:N}")]
public class TestAggregate : Aggregate<TestAggregateId, TestAggregateState>
{
	 public TestAggregate(string name) : this(TestAggregateId.CreateNew(), name) {
	 }

	 public TestAggregate(TestAggregateId id, string name) : this() => Apply(new TestAggregateCreated(id) {
		  Name = name
	 });

	 /// <inheritdoc />
	 protected internal TestAggregate() {
		  Handle<TestAggregateCreated>(OnCreated);
		  Handle<TestAggregateDescriptionChanged>(OnDescriptionChanged);
	 }

	 public void SetDescription(string value) {
		  if(string.IsNullOrWhiteSpace(value))
				return;
		  if(State.Description != null && State.Description.Equals(value))
				return;
		  Apply(new TestAggregateDescriptionChanged(Id) {
				Description = value
		  });
	 }

	 private TestAggregateState OnCreated(TestAggregateCreated @event) => State with {
		  Id = @event.Id,
		  Name = @event.Name
	 };

	 private TestAggregateState OnDescriptionChanged(TestAggregateDescriptionChanged @event) => State with {
		  Description = @event.Description
	 };
}
