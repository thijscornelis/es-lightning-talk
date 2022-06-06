using ES.Framework.Domain.TypedIdentifiers.Design;
using System.ComponentModel;
using System.Globalization;

namespace ES.Framework.Domain.TypedIdentifiers.Converters;

/// <summary>Type converter for typed identifiers</summary>
/// <typeparam name="TTypedIdentifier">The type of the identity.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <seealso cref="TypeConverter" />
public class TypedIdentifierConverter<TTypedIdentifier, TValue> : TypeConverter
	where TTypedIdentifier : ITypedIdentifier<TValue>
	where TValue : notnull
{
	private readonly TypeConverter _idValueConverter = GetIdValueConverter();
	private readonly Type _type;

	/// <summary>Initializes a new instance of the <see cref="TypedIdentifierConverter{TIdentity, TValue}" /> class.</summary>
	/// <param name="type">The type.</param>
	public TypedIdentifierConverter(Type type) => _type = type;

	/// <inheritdoc />
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(string)
		|| sourceType == typeof(TValue)
		|| base.CanConvertFrom(context, sourceType);

	/// <inheritdoc />
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
		destinationType == typeof(string)
		|| destinationType == typeof(TValue)
		|| base.CanConvertTo(context, destinationType);

	/// <inheritdoc />
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
		if(value is string s)
			value = _idValueConverter.ConvertFrom(s);
		if(value is TValue idValue) {
			var factory = TypedIdentifierHelper.GetFactory<TValue>(_type);
			return factory(idValue);
		}

		return base.ConvertFrom(context, culture, value);
	}

	/// <inheritdoc />
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
		Type destinationType) {
		if(value is null)
			throw new ArgumentNullException(nameof(value));

		var typedIdentifier = (ITypedIdentifier<TValue>) value;
		var idValue = typedIdentifier.TypedValue;
		return destinationType == typeof(string)
			? idValue.ToString()
			: destinationType == typeof(TValue)
				? idValue
				: base.ConvertTo(context, culture, value, destinationType);
	}

	/// <summary>Gets the identifier value converter.</summary>
	/// <returns>The actual value converter</returns>
	/// <exception cref="InvalidOperationException">
	///     Type '{typeof(TValue)}' doesn't have a converter that can convert from
	///     string
	/// </exception>
	private static TypeConverter GetIdValueConverter() {
		var converter = TypeDescriptor.GetConverter(typeof(TValue));
		return !converter.CanConvertFrom(typeof(string))
			? throw new InvalidOperationException(
				$"Type '{typeof(TValue)}' doesn't have a converter that can convert from string")
			: converter;
	}
}