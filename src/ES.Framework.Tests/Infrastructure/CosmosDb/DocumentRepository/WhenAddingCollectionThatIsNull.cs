using ES.Framework.Domain.Documents;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Moq;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenAddingCollectionThatIsNull : IClassFixture<WhenAddingCollectionThatIsNull.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenAddingCollectionThatIsNull(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldFail() => _fixture.HasExecutedSuccessfully.Should().BeFalse();

	 [Fact]
	 public void ItShouldThrowArgumentNullException() => _fixture.Throws.Should().BeOfType<ArgumentNullException>();

	 public class Fixture : FixtureBase
	 {
		  public List<EventDocument> EventDocuments { get; } = null;

		  public bool Result { get; private set; }

		  /// <inheritdoc />
		  protected override async Task ActAsync(CancellationToken cancellationToken) => Result = await Repository.AddAsync(EventDocuments, CancellationTokenSource.Token);
	 }
}


public class WhenGettingAsyncEnumerable : IClassFixture<WhenGettingAsyncEnumerable.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenGettingAsyncEnumerable(Fixture fixture) => _fixture = fixture;


	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 [Fact]
	 public async Task ItShouldReturnAsyncEnumerable() {
		 _fixture.Result.Should().NotBeNull();
		 
		 var documents = new List<EventDocument>();
		 await foreach(var item in _fixture.Result) {
			 documents.Add(item);
		 }

		 documents.Should().Contain(_fixture.EventDocuments);
	 }


	 public class Fixture : FixtureBase
	 {
		  public List<EventDocument> EventDocuments { get; } = new() {
			 new() {
				 Id = DocumentId.CreateNew(Guid.Parse("{A2966192-C1F9-4E2F-899F-84C4FB3E2075}")),
				 PartitionKey = "UNIT_TEST_PARTITION_KEY",
				 Version = 1,
				 Timestamp = DateTime.UtcNow
			 },
			 new() {
				 Id = DocumentId.CreateNew(Guid.Parse("{EC211614-664D-46FB-AB9D-9D0904E632D2}")),
				 PartitionKey = "UNIT_TEST_PARTITION_KEY",
				 Version = 1,
				 Timestamp = DateTime.UtcNow
			 },
			 new() {
				 Id = DocumentId.CreateNew(Guid.Parse("{6FD00868-B5B7-42CD-BD7D-64186504BFAC}")),
				 PartitionKey = "UNIT_TEST_PARTITION_KEY",
				 Version = 1,
				 Timestamp = DateTime.UtcNow
			 }, new() {
				 Id = DocumentId.CreateNew(Guid.Parse("{BB06F7BA-D805-4DC1-BE6A-850185F1CB51}")),
				 PartitionKey = "UNIT_TEST_PARTITION_KEY",
				 Version = 1,
				 Timestamp = DateTime.UtcNow
			 }
		 };

		  public IAsyncEnumerable<EventDocument> Result { get; private set; }
		  public string PartitionKey { get; set; } = "UNIT_TEST_PARTITION_KEY";
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
