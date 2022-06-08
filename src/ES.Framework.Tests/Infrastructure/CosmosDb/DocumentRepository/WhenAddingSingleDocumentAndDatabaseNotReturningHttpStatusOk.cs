using ES.Framework.Domain.Documents;
using ES.Framework.Domain.Events;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Moq;
using System.Net;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenAddingSingleDocumentAndDatabaseNotReturningHttpStatusOk : IClassFixture<WhenAddingSingleDocumentAndDatabaseNotReturningHttpStatusOk.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenAddingSingleDocumentAndDatabaseNotReturningHttpStatusOk(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldReturnFalse() => _fixture.Result.Should().BeFalse();

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  public EventDocument EventDocument { get; } = new() {
				Id = EventId.CreateNew(Guid.Parse("{A2966192-C1F9-4E2F-899F-84C4FB3E2075}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				AggregateVersion = 1,
				Timestamp = DateTime.UtcNow
		  };

		  public bool Result { get; private set; }

		  /// <inheritdoc />
		  protected override async Task ActAsync(CancellationToken cancellationToken) =>
			  Result = await Repository.AddAsync(EventDocument, CancellationTokenSource.Token);

		  /// <inheritdoc />
		  protected override void ArrangeContainer(Mock<Container> mock) => mock
			  .Setup(x => x.CreateItemAsync(EventDocument, new PartitionKey(EventDocument.PartitionKey),
				  It.IsAny<ItemRequestOptions>(), CancellationTokenSource.Token)).ReturnsAsync(ArrangeItemResponse());

		  private static Func<EventDocument, PartitionKey, ItemRequestOptions, CancellationToken, ItemResponse<EventDocument>>
			  ArrangeItemResponse() => (e, _, _, _) => {
					var mock = new Mock<ItemResponse<EventDocument>>();
					mock.Setup(x => x.StatusCode).Returns(HttpStatusCode.NotFound);
					return mock.Object;
			  };
	 }
}
