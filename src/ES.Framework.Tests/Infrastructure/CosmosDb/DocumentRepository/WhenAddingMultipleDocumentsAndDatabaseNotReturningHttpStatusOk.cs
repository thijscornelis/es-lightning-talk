using ES.Framework.Domain.Documents;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Moq;
using System.Net;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenAddingMultipleDocumentsAndDatabaseNotReturningHttpStatusOk : IClassFixture<
	WhenAddingMultipleDocumentsAndDatabaseNotReturningHttpStatusOk.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenAddingMultipleDocumentsAndDatabaseNotReturningHttpStatusOk(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldReturnFalse() => _fixture.Result.Should().BeFalse();

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  public List<EventDocument> EventDocuments { get; } = new() {
			new() {
				Id = DocumentId.CreateNew(Guid.Parse("{A2966192-C1F9-4E2F-899F-84C4FB3E2075}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				AggregateVersion = 1,
				Timestamp = DateTime.UtcNow
			}
		};

		  public bool Result { get; private set; }

		  public Mock<TransactionalBatch> TransactionalBatchMock { get; private set; } = new();

		  /// <inheritdoc />
		  protected override async Task ActAsync(CancellationToken cancellationToken) => Result = await Repository.AddAsync(EventDocuments, CancellationTokenSource.Token);

		  /// <inheritdoc />
		  protected override void Arrange() {
				base.Arrange();
				ArrangeTransactionalBatch(TransactionalBatchMock);
		  }

		  /// <inheritdoc />
		  protected override void ArrangeContainer(Mock<Container> mock) {
				var partitionKey = EventDocuments.Select(x => x.PartitionKey).Distinct().Single();
				mock.Setup(x => x.CreateTransactionalBatch(new PartitionKey(partitionKey))).Returns(() => TransactionalBatchMock.Object);
		  }

		  private void ArrangeTransactionalBatch(Mock<TransactionalBatch> mock) {
				mock
					.Setup(x => x.ExecuteAsync(It.IsAny<TransactionalBatchRequestOptions>(), CancellationTokenSource.Token))
					.ReturnsAsync(ArrangeTransactionalBatchResponse());
				mock
					.Setup(x => x.ExecuteAsync(CancellationTokenSource.Token))
					.ReturnsAsync(ArrangeTransactionalBatchResponse());
		  }

		  private Func<TransactionalBatchResponse> ArrangeTransactionalBatchResponse() => () => {
				var mock = new Mock<TransactionalBatchResponse>();
				mock.Setup(x => x.StatusCode).Returns(HttpStatusCode.NotFound);
				return mock.Object;
		  };
	 }
}
