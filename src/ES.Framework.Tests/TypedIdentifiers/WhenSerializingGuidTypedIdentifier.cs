using ES.Framework.Tests.Mocks;
using ES.Sample.Fixtures;
using FluentAssertions;

namespace ES.Framework.Tests.TypedIdentifiers;

public class WhenSerializingGuidTypedIdentifier : IClassFixture<WhenSerializingGuidTypedIdentifier.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenSerializingGuidTypedIdentifier(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 [Fact]
	 public void ItShouldSerializeToJson() =>
		 _fixture.Result.Should().NotBeNullOrWhiteSpace()
			 .And.NotBe("{}")
			 .And.NotBe("{\r\n  \"id\": null\r\n}")
			 .And.Contain(_fixture.Value.ToString(), AtMost.Once());

	 public class Fixture : SerializationFixtureBase<TypedIdentifierDto<GuidTypedIdentifier>>
	 {
		  /// <inheritdoc />
		  protected override TypedIdentifierDto<GuidTypedIdentifier> ArrangeSource() => new(new(Value));

		  public Guid Value { get; } = Guid.Parse("{29AFD117-CEE8-4BB9-A0D0-FD9F49DCF5EC}");
	 }
}