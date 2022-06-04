using ES.Framework.Domain.Abstractions;
using ES.Framework.Infrastructure.Attributes;

namespace ES.Sample.Fixtures;

public abstract class AttributeFixtureBase<TClass, TAttribute> : Sample.Fixtures.FixtureBase
	where TAttribute : Attribute
	where TClass : class
{
	 /// <summary>Gets the attribute.</summary>
	 /// <value>The attribute.</value>
	 public TAttribute Attribute { get; private set; }

	 /// <summary>Gets the resolver.</summary>
	 /// <value>The resolver.</value>
	 public IAttributeValueResolver Resolver { get; private set; }

	 protected virtual void Act() => Attribute = Resolver.GetValue<TAttribute>(typeof(TClass));

	 /// <inheritdoc />
	 protected override void Arrange() {
		  base.Arrange();
		  Resolver = ArrangeResolver();
	 }

	 protected virtual IAttributeValueResolver ArrangeResolver() => new AttributeValueResolver();

	 /// <inheritdoc />
	 protected override Task InternalActAsync() {
		  Act();
		  return Task.CompletedTask;
	 }
}
