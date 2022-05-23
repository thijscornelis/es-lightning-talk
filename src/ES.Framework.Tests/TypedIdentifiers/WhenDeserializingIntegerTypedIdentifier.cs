using ES.Framework.Tests.Mocks;
using ES.Sample.Fixtures;
using FluentAssertions;

namespace ES.Framework.Tests.TypedIdentifiers;

public class WhenDeserializingIntegerTypedIdentifier : IClassFixture<WhenDeserializingIntegerTypedIdentifier.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenDeserializingIntegerTypedIdentifier(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 [Fact]
	 public void ItShouldHaveTypedValue() => _fixture.Result.Id.TypedValue.Should().NotBe(null).And.NotBe(0);

	 [Fact]
	 public void ItShouldDeserializeToTypedIdentifier() => _fixture.Result.Id.Should().NotBeNull().And.BeOfType<IntegerTypedIdentifier>();
	 public class Fixture : DeserializationFixtureBase<TypedIdentifierDto<IntegerTypedIdentifier>>
	 {
		 /// <inheritdoc />
		 protected override string ArrangeSource() => "{\r\n  \"id\": 9001\r\n}";
	 }
}