using ES.Framework.Domain.Repositories;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Moq;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenAddingEmptyCollection : IClassFixture<WhenAddingEmptyCollection.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenAddingEmptyCollection(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldNotCreateTransactionalBatch() => _fixture.ContainerMock.Verify(x => x.CreateTransactionalBatch(It.IsAny<PartitionKey>()), Times.Never);

	 [Fact]
	 public void ItShouldReturnTrue() => _fixture.Result.Should().BeTrue();

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  public List<EventDocument> EventDocuments { get; } = new();

		  public bool Result { get; private set; }

		  /// <inheritdoc />
		  protected override async Task ActAsync(CancellationToken cancellationToken) => Result = await Repository.AddAsync(EventDocuments, CancellationTokenSource.Token);
	 }
}
