using ES.Framework.Domain.Abstractions;
using ES.Framework.Tests.Mocks;
using ES.Sample.Fixtures;
using FluentAssertions;
using Moq;

namespace ES.Framework.Tests.Documents.Factory;

public abstract class FixtureBase : EventDocumentFactoryFixture<TestAggregate, TestAggregateId, TestAggregateState, Guid>
{
}

public class WhenUncommittedEventListIsEmpty : IClassFixture<WhenUncommittedEventListIsEmpty.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenUncommittedEventListIsEmpty(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldReturnEmptyList() => _fixture.Result.Should().BeEmpty();

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  /// <inheritdoc />
		  protected override TestAggregate ArrangeAggregate() => new();
	 }
}

public class WhenUncommittedEventListContainsSingleItem : IClassFixture<WhenUncommittedEventListContainsSingleItem.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenUncommittedEventListContainsSingleItem(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldReturnListWithSingleItem() {
		  _fixture.Result.Should().HaveCount(1);
		  var document = _fixture.Result.Single();
		  document.Should().NotBeNull();
		  document.AggregateId.Should().Be(_fixture.Aggregate.Id.Value);
		  document.Payload.Should().Be(_fixture.Aggregate.UncommittedEvents.Single());
		  document.AggregateType.Should().Be(typeof(TestAggregate).FullName);
		  document.EventType.Should().Be(_fixture.Aggregate.UncommittedEvents.Single().GetType().AssemblyQualifiedName);
		  document.AggregateVersion.Should().Be(1);
		  document.PartitionKey.Should().Be(_fixture.PartitionKey);
	 }

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  public string PartitionKey = "UNIT_TEST_PARTITION_KEY";

		  /// <inheritdoc />
		  protected override TestAggregate ArrangeAggregate() => new(TestAggregateId.CreateNew(Guid.Parse("{4C4152E5-718A-46B2-97E9-FADA42F2D53C}")), "UNIT TEST NAME");

		  /// <inheritdoc />
		  protected override void ArrangePartitionKeyResolver(Mock<IPartitionKeyResolver<TestAggregate, TestAggregateId, TestAggregateState, Guid>> mock) => mock.Setup(x => x.CreateSyntheticPartitionKey(Aggregate.Id)).Returns(PartitionKey);
	 }
}
