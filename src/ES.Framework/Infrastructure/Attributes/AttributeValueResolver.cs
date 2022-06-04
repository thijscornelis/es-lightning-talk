using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Exceptions;

namespace ES.Framework.Infrastructure.Attributes;

/// <inheritdoc />
public class AttributeValueResolver : IAttributeValueResolver
{
	 /// <inheritdoc />
	 public TAttribute GetValue<TAttribute>(Type type) where TAttribute : Attribute {
		  var attributes = type.GetCustomAttributes(typeof(TAttribute), true);
		  var attribute = attributes.FirstOrDefault() ?? throw new MissingAttributeException(typeof(TAttribute), type);
		  return (TAttribute) attribute;
	 }
}
