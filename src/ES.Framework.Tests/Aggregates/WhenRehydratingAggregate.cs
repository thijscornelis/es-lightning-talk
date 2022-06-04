using ES.Framework.Domain;
using ES.Framework.Domain.Collections;
using ES.Framework.Tests.Mocks;
using FluentAssertions;

namespace ES.Framework.Tests.Aggregates;

public class WhenRehydratingAggregate : IClassFixture<WhenRehydratingAggregate.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenRehydratingAggregate(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldExposeAggregateId() => _fixture.Aggregate.Id.Should().Be(_fixture.TestAggregateId);

	 [Fact]
	 public void ItShouldHaveCombinedSnapshotAndEventCollection() => _fixture.Aggregate.State.Should().NotBeNull().And.Be(
		 new TestAggregateState {
			  Id = _fixture.TestAggregateId,
			  Name = _fixture.TestAggregateName,
			  Description = _fixture.TestAggregateDescription
		 });

	 [Fact]
	 public void ItShouldHaveCorrectVersion() => _fixture.Aggregate.Version.Should().Be(2);

	 [Fact]
	 public void ItShouldHaveEmptyEventCollection() => _fixture.Aggregate.UncommittedEvents.Should().BeEmpty();

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  public EventCollection<TestAggregateId> EventCollection { get; private set; }
		  public Snapshot<TestAggregateId, TestAggregateState> Snapshot { get; private set; }
		  public string TestAggregateDescription { get; } = "TEST AGGREGATE DESCRIPTION";

		  public TestAggregateId TestAggregateId { get; } =
			  TestAggregateId.CreateNew(Guid.Parse("{973BD04D-E779-434C-B46E-212EB7083233}"));

		  public string TestAggregateName { get; } = "TEST AGGREGATE NAME";

		  /// <inheritdoc />
		  protected override void Act() => Aggregate = TestAggregate.Rehydrate<TestAggregate>(Snapshot, EventCollection);

		  /// <inheritdoc />
		  protected override void Arrange() {
				base.Arrange();
				Snapshot = ArrangeSnapshot();
				EventCollection = ArrangeEventCollection();
		  }

		  /// <inheritdoc />
		  protected override TestAggregate ArrangeAggregate() => null;

		  private EventCollection<TestAggregateId> ArrangeEventCollection() => new()
			  {new TestAggregateDescriptionChanged {Id = TestAggregateId, Description = TestAggregateDescription}};

		  private Snapshot<TestAggregateId, TestAggregateState> ArrangeSnapshot() =>
			  new() {
					Id = new SnapshotId(Guid.Parse("{BD6F2CE9-B1E0-4FCA-8558-FE4F744F9597}")),
					AggregateId = TestAggregateId,
					State = new TestAggregateState {
						 Id = TestAggregateId, Name = TestAggregateName
					},
					Version = 1
			  };
	 }
}
