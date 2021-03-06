using ES.Framework.Tests.Mocks;
using FluentAssertions;

namespace ES.Framework.Tests.Infrastructure.Attributes.PartitionKey;

public class WhenAggregatePartitionKeyAttributeIsAvailable : IClassFixture<WhenAggregatePartitionKeyAttributeIsAvailable.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenAggregatePartitionKeyAttributeIsAvailable(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldBeUseableInStringFormat() {
		  var value = _fixture.Attribute.GetPartitionKey("replace {0}",
			  "replace {1}",
			  "replace {2}");
		  value.Should().NotBeNullOrWhiteSpace().And.Be("replace {0}-replace {1}-replace {2}");
	 }

	 [Fact]
	 public void ItShouldReturnValue() => _fixture.Attribute.Should().NotBeNull();

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase<TypeWithAggregatePartitionKeyAttribute>
	 {
	 }
}
