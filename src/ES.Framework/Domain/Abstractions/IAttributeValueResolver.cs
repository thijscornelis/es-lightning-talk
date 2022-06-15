namespace ES.Framework.Domain.Abstractions;

/// <summary>AttributeValueResolver</summary>
public interface IAttributeValueResolver
{
	 /// <summary>Gets the value for the attribute assigned to the <see cref="Type" /></summary>
	 /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
	 /// <param name="type">The type.</param>
	 /// <returns>TAttribute.</returns>
	 public TAttribute GetValue<TAttribute>(Type type)
		  where TAttribute : Attribute;
}
