using ES.Framework.Tests.Mocks;
using FluentAssertions;

namespace ES.Framework.Tests.TypedIdentifiers;

public class WhenCreatingIntegerTypedIdentifier
{
	[Fact]
	public void WithStaticMethod_ShouldGenerateDefaultValue() {
		//Act.
		var id = IntegerTypedIdentifier.CreateNew();

		//Assert.
		id.Should().NotBeNull();
		id.Value.Should().NotBeNull().And.NotBeEmpty();
		id.TypedValue.Should().Be(0);
	}

	[Fact]
	public void WithPublicDefaultConstructor_ShouldSetValue() {
		//Arrange.
		var expectedId = 9001;

		//Act.
		var id = new IntegerTypedIdentifier(expectedId);

		//Assert.
		id.Should().NotBeNull();
		id.Value.Should().NotBeNullOrWhiteSpace();
		id.TypedValue.Should().Be(expectedId);
	}
}