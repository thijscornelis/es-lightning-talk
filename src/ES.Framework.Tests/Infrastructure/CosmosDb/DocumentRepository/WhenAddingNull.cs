using ES.Framework.Domain.Repositories;
using FluentAssertions;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenAddingNull : IClassFixture<WhenAddingNull.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenAddingNull(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldFail() => _fixture.HasExecutedSuccessfully.Should().BeFalse();

	 [Fact]
	 public void ItShouldThrowArgumentNullException() => _fixture.Throws.Should().BeOfType<ArgumentNullException>();

	 public class Fixture : FixtureBase
	 {
		  public EventDocument EventDocument = null;

		  public bool Result { get; private set; }

		  /// <inheritdoc />
		  protected override async Task ActAsync(CancellationToken cancellationToken) => Result = await Repository.AddAsync(EventDocument, CancellationTokenSource.Token);
	 }
}
