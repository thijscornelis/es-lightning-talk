using ES.Framework.Domain.Documents;
using FluentAssertions;

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
