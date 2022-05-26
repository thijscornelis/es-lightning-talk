using ES.Framework.Domain.Repositories;
using FluentAssertions;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenAddingSingleDocumentWithoutPartitionKeyValue : IClassFixture<WhenAddingSingleDocumentWithoutPartitionKeyValue.Fixture>
{
	private readonly Fixture _fixture;

	public WhenAddingSingleDocumentWithoutPartitionKeyValue(Fixture fixture) => _fixture = fixture;

	[Fact]
	public void ItShouldFail() => _fixture.HasExecutedSuccessfully.Should().BeFalse();

	[Fact]
	public void ItShouldThrowArgumentNullException() => _fixture.Throws.Should().BeOfType<ArgumentNullException>();

	public class Fixture : FixtureBase
	{
		public EventDocument EventDocument { get; private set; } = new() {
			Id = DocumentId.CreateNew(Guid.Parse("{A2966192-C1F9-4E2F-899F-84C4FB3E2075}")),
			PartitionKey = null,
			Version = 1,
			Timestamp = DateTime.UtcNow
		};

		public bool Result { get; private set; }

		/// <inheritdoc />
		protected override async Task ActAsync(CancellationToken cancellationToken) => Result = await Repository.AddAsync(EventDocument, CancellationTokenSource.Token);
	}
}