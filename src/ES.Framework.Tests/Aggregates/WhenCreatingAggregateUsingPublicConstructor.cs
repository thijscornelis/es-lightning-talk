using ES.Framework.Tests.Mocks;
using FluentAssertions;

namespace ES.Framework.Tests.Aggregates;

public class
	WhenCreatingAggregateUsingPublicConstructor : IClassFixture<WhenCreatingAggregateUsingPublicConstructor.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenCreatingAggregateUsingPublicConstructor(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldExposeAggregateId() => _fixture.Aggregate.Id.Should().Be(_fixture.TestAggregateId);

	 [Fact]
	 public void ItShouldHaveAppliedEventToState() => _fixture.Aggregate.State.Should().NotBeNull().And.Be(
		 new TestAggregateState {
			  Id = _fixture.TestAggregateId,
			  Name = _fixture.TestAggregateName
		 });

	 [Fact]
	 public void ItShouldHaveBumpedVersionNumber() => _fixture.Aggregate.Version.Should().Be(1);

	 [Fact]
	 public void ItShouldHaveUncommittedEvent() => _fixture.Aggregate.UncommittedEvents.OfType<TestAggregateCreated>().Should().ContainSingle();

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  public TestAggregateId TestAggregateId { get; } =
			  TestAggregateId.CreateNew(Guid.Parse("{973BD04D-E779-434C-B46E-212EB7083233}"));

		  public string TestAggregateName { get; } = "TEST AGGREGATE NAME";

		  /// <inheritdoc />
		  protected override void Act() => Aggregate = new TestAggregate(TestAggregateId, TestAggregateName);

		  /// <inheritdoc />
		  protected override TestAggregate ArrangeAggregate() => null;
	 }
}
