using ES.Sample.Fixtures;
using FluentAssertions;

namespace ES.Framework.Tests.TypedIdentifiers;

public class WhenDeserializingStringTypedIdentifier : IClassFixture<WhenDeserializingStringTypedIdentifier.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenDeserializingStringTypedIdentifier(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 [Fact]
	 public void ItShouldHaveTypedValue() => _fixture.Result.Id.TypedValue.Should().NotBeNullOrWhiteSpace();

	 [Fact]
	 public void ItShouldDeserializeToTypedIdentifier() => _fixture.Result.Id.Should().NotBeNull().And.BeOfType<StringTypedIdentifier>();
	 public class Fixture : DeserializationFixtureBase<TypedIdentifierDto<StringTypedIdentifier>>
	 {
		  /// <inheritdoc />
		  protected override string ArrangeSource() => "{\r\n  \"id\": \"It\\u0027s over 9000!\"\r\n}";
	 }
}