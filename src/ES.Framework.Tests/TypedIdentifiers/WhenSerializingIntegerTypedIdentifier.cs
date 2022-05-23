using ES.Sample.Fixtures;
using FluentAssertions;

namespace ES.Framework.Tests.TypedIdentifiers;

public class WhenSerializingIntegerTypedIdentifier : IClassFixture<WhenSerializingIntegerTypedIdentifier.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenSerializingIntegerTypedIdentifier(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 [Fact]
	 public void ItShouldSerializeToJson() =>
		 _fixture.Result.Should().NotBeNullOrWhiteSpace()
			 .And.NotBe("{}")
			 .And.NotBe("{\r\n  \"id\": null\r\n}")
			 .And.Contain(_fixture.Value.ToString(), AtMost.Once());


	 public class Fixture : SerializationFixtureBase<TypedIdentifierDto<IntegerTypedIdentifier>>
	 {
		  public int Value { get; } = 9001;
		  /// <inheritdoc />
		  protected override TypedIdentifierDto<IntegerTypedIdentifier> ArrangeSource() => new(new(Value));
	 }
}