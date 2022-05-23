using ES.Sample.Fixtures;
using FluentAssertions;

namespace ES.Framework.Tests.TypedIdentifiers;

public class WhenSerializingStringTypedIdentifier : IClassFixture<WhenSerializingStringTypedIdentifier.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenSerializingStringTypedIdentifier(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();
	 [Fact]
	 public void ItShouldSerializeToJson() =>
		 _fixture.Result.Should().NotBeNullOrWhiteSpace()
			 .And.NotBe("{}")
			 .And.NotBe("{\r\n  \"id\": null\r\n}")
			 .And.Contain(_fixture.Value, AtMost.Once());
	 public class Fixture : SerializationFixtureBase<TypedIdentifierDto<StringTypedIdentifier>>
	 {
		  public string Value { get; } = "It's over 9000!";
		  /// <inheritdoc />
		  protected override TypedIdentifierDto<StringTypedIdentifier> ArrangeSource() => new(new(Value));
	 }
}