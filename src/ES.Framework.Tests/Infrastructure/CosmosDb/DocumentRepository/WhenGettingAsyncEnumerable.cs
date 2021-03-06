using ES.Framework.Domain.Documents;
using ES.Framework.Domain.Events;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Moq;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenGettingAsyncEnumerable : IClassFixture<WhenGettingAsyncEnumerable.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenGettingAsyncEnumerable(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public async Task ItShouldReturnAsyncEnumerable() {
		  _fixture.Result.Should().NotBeNull();

		  var documents = new List<EventDocument>();
		  await foreach(var item in _fixture.Result) {
				documents.Add(item);
		  }

		  documents.Should().Contain(_fixture.EventDocuments);
	 }

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  public List<EventDocument> EventDocuments { get; } = new() {
			new() {
				Id = EventId.CreateNew(Guid.Parse("{A2966192-C1F9-4E2F-899F-84C4FB3E2075}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				Timestamp = DateTime.UtcNow
			},
			new() {
				Id = EventId.CreateNew(Guid.Parse("{EC211614-664D-46FB-AB9D-9D0904E632D2}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				Timestamp = DateTime.UtcNow
			},
			new() {
				Id = EventId.CreateNew(Guid.Parse("{6FD00868-B5B7-42CD-BD7D-64186504BFAC}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				Timestamp = DateTime.UtcNow
			}, new() {
				Id = EventId.CreateNew(Guid.Parse("{BB06F7BA-D805-4DC1-BE6A-850185F1CB51}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				Timestamp = DateTime.UtcNow
			}
		};

		  public string PartitionKey { get; set; } = "UNIT_TEST_PARTITION_KEY";
		  public IAsyncEnumerable<EventDocument> Result { get; private set; }

		  /// <inheritdoc />
		  protected override Task ActAsync(CancellationToken cancellationToken) {
				Result = Repository.GetAsyncEnumerable<EventDocument>(PartitionKey, null, CancellationTokenSource.Token);
				return Task.CompletedTask;
		  }

		  /// <inheritdoc />
		  protected override void ArrangeContainer(Mock<Container> mock)
			  => mock.Setup(x => x.GetItemLinqQueryable<EventDocument>(false, It.IsAny<string>(), It.IsAny<QueryRequestOptions>(), It.IsAny<CosmosLinqSerializerOptions>()))
				  .Returns(new EnumerableQuery<EventDocument>(EventDocuments));
	 }
}
