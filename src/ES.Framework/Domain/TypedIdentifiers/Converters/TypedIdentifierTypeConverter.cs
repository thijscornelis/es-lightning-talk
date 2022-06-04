using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;

namespace ES.Framework.Domain.TypedIdentifiers.Converters;

/// <summary>Type converter for typed identifiers</summary>
public class TypedIdentifierTypeConverter : TypeConverter
{
	 private static readonly ConcurrentDictionary<Type, TypeConverter> _actualConverters = new();
	 private readonly TypeConverter _innerConverter;

	 /// <summary>Initializes a new instance of the <see cref="TypedIdentifierTypeConverter" /> class.</summary>
	 /// <param name="typedIdentifierType">Type of the strongly typed identifier.</param>
	 public TypedIdentifierTypeConverter(Type typedIdentifierType) =>
		 _innerConverter = _actualConverters.GetOrAdd(typedIdentifierType, CreateActualConverter);

	 /// <inheritdoc />
	 public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
		 _innerConverter.CanConvertFrom(context, sourceType);

	 /// <inheritdoc />
	 public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
		 _innerConverter.CanConvertTo(context, destinationType);

	 /// <inheritdoc />
	 public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) =>
		 _innerConverter.ConvertFrom(context, culture, value);

	 /// <inheritdoc />
	 public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
		 Type destinationType) => _innerConverter.ConvertTo(context, culture, value, destinationType);

	 /// <summary>Creates the actual converter.</summary>
	 /// <param name="typedIdentifierType">Type of the strongly typed identifier.</param>
	 /// <returns>Type converter which can be used to convert the typed identifier to its actual value</returns>
	 /// <exception cref="InvalidOperationException">$"The type '{typedIdentifierType}' is not a strongly typed id</exception>
	 private static TypeConverter CreateActualConverter(Type typedIdentifierType) {
		  if(!TypedIdentifierHelper.IsTypedIdentifier(typedIdentifierType, out var valueType))
				throw new InvalidOperationException($"The type '{typedIdentifierType}' is not a strongly typed id");
		  var actualConverterType = typeof(TypedIdentifierConverter<,>).MakeGenericType(typedIdentifierType, valueType);
		  return (TypeConverter) Activator.CreateInstance(actualConverterType, typedIdentifierType)!;
	 }
}
