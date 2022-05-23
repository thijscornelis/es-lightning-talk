using ES.Framework.Tests.Mocks;
using FluentAssertions;

namespace ES.Framework.Tests.Aggregates;

public class WhenCreatingAggregate : IClassFixture<WhenCreatingAggregate.Fixture>
{
	private readonly Fixture _fixture;

	public WhenCreatingAggregate(Fixture fixture) => _fixture = fixture;

	[Fact]
	public void ItShouldRegisterEventHandlers() {
		_fixture.Aggregate.GetEventHandlers().Should().NotBeEmpty();
		_fixture.Aggregate.GetEventHandlers().Should().ContainKey(typeof(TestAggregateCreated)).And.ContainKey(typeof(TestAggregateDescriptionChanged));
	}

	[Fact]
	public void ItShouldHaveEmptyEventCollection() => _fixture.Aggregate.UncommittedEvents.Should().BeEmpty();

	public class Fixture : FixtureBase
	{
		/// <inheritdoc />
		protected override void Act() { }
	}

}