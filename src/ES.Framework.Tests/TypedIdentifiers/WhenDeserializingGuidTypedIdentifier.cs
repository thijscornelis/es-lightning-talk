using ES.Framework.Tests.Mocks;
using ES.Sample.Fixtures;
using FluentAssertions;

namespace ES.Framework.Tests.TypedIdentifiers;

public class WhenDeserializingGuidTypedIdentifier : IClassFixture<WhenDeserializingGuidTypedIdentifier.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenDeserializingGuidTypedIdentifier(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 [Fact]
	 public void ItShouldHaveTypedValue() => _fixture.Result.Id.TypedValue.Should().NotBeEmpty();

	 [Fact]
	 public void ItShouldDeserializeToTypedIdentifier() => _fixture.Result.Id.Should().NotBeNull().And.BeOfType<GuidTypedIdentifier>();

	 public class Fixture : DeserializationFixtureBase<TypedIdentifierDto<GuidTypedIdentifier>>
	 {
		 /// <inheritdoc />
		 protected override string ArrangeSource() => "{\r\n  \"id\": \"29afd117-cee8-4bb9-a0d0-fd9f49dcf5ec\"\r\n}";
	 }
}