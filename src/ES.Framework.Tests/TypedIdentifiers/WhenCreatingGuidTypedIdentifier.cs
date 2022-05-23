using FluentAssertions;

namespace ES.Framework.Tests.TypedIdentifiers;

public class WhenCreatingGuidTypedIdentifier
{
	[Fact]
	public void WithStaticMethod_ShouldGenerateValue() {
		//Act.
		var id = GuidTypedIdentifier.CreateNew();

		//Assert.
		id.Should().NotBeNull();
		id.Value.Should().NotBeNullOrWhiteSpace();
		id.TypedValue.Should().NotBeEmpty();
	}

	[Fact]
	public void WithPublicDefaultConstructor_ShouldSetValue() {
		//Arrange.
		var expectedId = Guid.Parse("{671F4357-057A-4BB3-A4D3-567F0A5E0054}");

		//Act.
		var id = new GuidTypedIdentifier(expectedId);

		//Assert.
		id.Should().NotBeNull();
		id.Value.Should().NotBeNullOrWhiteSpace();
		id.TypedValue.Should().Be(expectedId);
	}
}