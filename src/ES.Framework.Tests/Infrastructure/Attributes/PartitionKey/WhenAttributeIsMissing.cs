using ES.Framework.Domain.Exceptions;
using ES.Framework.Tests.Mocks;
using FluentAssertions;

namespace ES.Framework.Tests.Infrastructure.Attributes.PartitionKey;

public class WhenAttributeIsMissing : IClassFixture<WhenAttributeIsMissing.Fixture>
{
	private readonly Fixture _fixture;

	public WhenAttributeIsMissing(Fixture fixture) => _fixture = fixture;

	[Fact]
	public void ItShouldFail() => _fixture.HasExecutedSuccessfully.Should().BeFalse();

	[Fact]
	public void ItShouldThrowMissingAttributeException() => _fixture.Throws.Should().BeOfType<MissingAttributeException>();

	public class Fixture : FixtureBase<TypeWithoutAttribute>
	{
	}
}