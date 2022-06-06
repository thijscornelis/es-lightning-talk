using ES.Framework.Tests.Mocks;
using FluentAssertions;

namespace ES.Framework.Tests.Aggregates;

public class WhenCreatingAggregate : IClassFixture<WhenCreatingAggregate.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenCreatingAggregate(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldHaveEmptyEventCollection() => _fixture.Aggregate.UncommittedEvents.Should().BeEmpty();

	 [Fact]
	 public void ItShouldHaveEmptyState() => _fixture.Aggregate.State.Should().NotBeNull().And.Be(new TestAggregateState());

	 [Fact]
	 public void ItShouldHaveInitialVersion() => _fixture.Aggregate.Version.Should().Be(0);

	 [Fact]
	 public void ItShouldRegisterEventHandlers() {
		  _fixture.Aggregate.EventHandlers.Should().NotBeEmpty();
		  _fixture.Aggregate.EventHandlers.Should().ContainKey(typeof(TestAggregateCreated)).And
			  .ContainKey(typeof(TestAggregateDescriptionChanged));
	 }

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  /// <inheritdoc />
		  protected override void Act() => Aggregate = new();

		  /// <inheritdoc />
		  protected override TestAggregate ArrangeAggregate() => null;
	 }
}
