using ES.Framework.Domain.Documents;
using Microsoft.Azure.Cosmos;
using Moq;
using System.Net;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenDeletingItemAndDatabaseIsNotReturningHttpStatusOk : IClassFixture<WhenDeletingItemAndDatabaseIsNotReturningHttpStatusOk.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenDeletingItemAndDatabaseIsNotReturningHttpStatusOk(Fixture fixture) => _fixture = fixture;

	 public class Fixture : FixtureBase
	 {
		  public EventDocument EventDocument { get; } = new() {
				Id = DocumentId.CreateNew(Guid.Parse("{A2966192-C1F9-4E2F-899F-84C4FB3E2075}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				AggregateVersion = 1,
				Timestamp = DateTime.UtcNow
		  };

		  public bool Result { get; private set; }

		  /// <inheritdoc />
		  protected override async Task ActAsync(CancellationToken cancellationToken) => Result = await Repository.DeleteAsync(EventDocument, CancellationTokenSource.Token);

		  /// <inheritdoc />
		  protected override void ArrangeContainer(Mock<Container> mock) => mock.Setup(x => x.DeleteItemAsync<EventDocument>(EventDocument.Id.Value, new(EventDocument.PartitionKey), It.IsAny<ItemRequestOptions>(), CancellationTokenSource.Token)).ReturnsAsync(ArrangeItemResponse);

		  private ItemResponse<EventDocument> ArrangeItemResponse() {
				var mock = new Mock<ItemResponse<EventDocument>>();
				mock.Setup(x => x.StatusCode).Returns(HttpStatusCode.NotFound);
				return mock.Object;
		  }
	 }
}
