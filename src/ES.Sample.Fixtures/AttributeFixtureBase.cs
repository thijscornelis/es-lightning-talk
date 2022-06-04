using ES.Framework.Domain.Abstractions;
using ES.Framework.Infrastructure.Attributes;

namespace ES.Sample.Fixtures;

public abstract class AttributeFixtureBase<TClass, TAttribute> : Sample.Fixtures.FixtureBase
	where TAttribute : Attribute
	where TClass : class
{
	public TAttribute Attribute { get; private set; }
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