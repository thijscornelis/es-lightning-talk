using ES.Framework.Domain.Documents;
using ES.Framework.Infrastructure.Cosmos.Exceptions;
using FluentAssertions;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenAddingMultipleDocumentsWithDifferentPartitionKeys : IClassFixture<WhenAddingMultipleDocumentsWithDifferentPartitionKeys.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenAddingMultipleDocumentsWithDifferentPartitionKeys(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldFail() => _fixture.HasExecutedSuccessfully.Should().BeFalse();

	 [Fact]
	 public void ItShouldThrowMultiplePartitionKeysDetected() => _fixture.Throws.Should().BeOfType<MultiplePartitionKeysDetected>();

	 public class Fixture : FixtureBase
	 {
		  public List<EventDocument> EventDocuments { get; } = new() {
			new() {
				Id = DocumentId.CreateNew(Guid.Parse("{A2966192-C1F9-4E2F-899F-84C4FB3E2075}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				AggregateVersion = 1,
				Timestamp = DateTime.UtcNow
			},
			new() {
				Id = DocumentId.CreateNew(Guid.Parse("{EC211614-664D-46FB-AB9D-9D0904E632D2}")),
				PartitionKey = "UNIT_TEST_DIFFERENT_PARTITION_KEY",
				AggregateVersion = 1,
				Timestamp = DateTime.UtcNow
			},
			new() {
				Id = DocumentId.CreateNew(Guid.Parse("{6FD00868-B5B7-42CD-BD7D-64186504BFAC}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				AggregateVersion = 1,
				Timestamp = DateTime.UtcNow
			}, new() {
				Id = DocumentId.CreateNew(Guid.Parse("{BB06F7BA-D805-4DC1-BE6A-850185F1CB51}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				AggregateVersion = 1,
				Timestamp = DateTime.UtcNow
			}
		};

		  public bool Result { get; private set; }

		  /// <inheritdoc />
		  protected override async Task ActAsync(CancellationToken cancellationToken) => Result = await Repository.AddAsync(EventDocuments, CancellationTokenSource.Token);
	 }
}
