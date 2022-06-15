using ES.Framework.Tests.Mocks;
using FluentAssertions;

namespace ES.Framework.Tests.TypedIdentifiers;

public class WhenCreatingStringTypedIdentifier
{
	[Fact]
	public void WithStaticMethod_ShouldThrowException() {
		//Act.
		void Act() => StringTypedIdentifier.CreateNew();

		//Assert.
		Assert.Throws<MissingMethodException>(Act);
	}

	[Fact]
	public void WithPublicDefaultConstructor_ShouldSetValue() {
		//Arrange.
		var expectedId = "identifier";

		//Act.
		var id = new StringTypedIdentifier(expectedId);

		//Assert.
		id.Should().NotBeNull();
		id.Value.Should().NotBeNullOrWhiteSpace();
		id.TypedValue.Should().Be(expectedId);
	}
}