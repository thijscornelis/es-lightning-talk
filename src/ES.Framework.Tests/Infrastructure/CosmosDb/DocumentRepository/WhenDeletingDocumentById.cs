using ES.Framework.Domain.Documents;
using ES.Framework.Domain.Events;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Moq;
using System.Net;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenDeletingDocumentById : IClassFixture<WhenDeletingDocumentById.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenDeletingDocumentById(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldReturnTrue() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  public EventId DocumentId { get; } = EventId.CreateNew(Guid.Parse("{A2966192-C1F9-4E2F-899F-84C4FB3E2075}"));
		  public string PartitionKey { get; } = "UNIT_TEST_PARTITION_KEY";

		  public bool Result { get; private set; }

		  /// <inheritdoc />
		  protected override async Task ActAsync(CancellationToken cancellationToken) => Result = await Repository.DeleteAsync<EventDocument, EventId>(DocumentId, PartitionKey, CancellationTokenSource.Token);

		  /// <inheritdoc />
		  protected override void ArrangeContainer(Mock<Container> mock) => mock.Setup(x => x.DeleteItemAsync<EventDocument>(DocumentId.Value, new(PartitionKey), It.IsAny<ItemRequestOptions>(), CancellationTokenSource.Token)).ReturnsAsync(ArrangeItemResponse);

		  private ItemResponse<EventDocument> ArrangeItemResponse() {
				var mock = new Mock<ItemResponse<EventDocument>>();
				mock.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);
				return mock.Object;
		  }
	 }
}
