using ES.Framework.Domain.TypedIdentifiers.Design;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;

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

/// <summary>Helper class for <see cref="TypedIdentifier{TIdentity, TKey}" /></summary>
public static class TypedIdentifierHelper
{
	private static readonly ConcurrentDictionary<Type, Delegate> _typedIdentifierFactories = new();

	/// <summary>Gets the factory.</summary>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="typedIdentifierType">Type of the typed identifier.</param>
	/// <returns></returns>
	public static Func<TValue, object> GetFactory<TValue>(Type typedIdentifierType)
		where TValue : notnull => (Func<TValue, object>) _typedIdentifierFactories.GetOrAdd(
		typedIdentifierType,
		CreateFactory<TValue>);

	/// <summary>Determines whether [is typed identifier] [the specified type].</summary>
	/// <param name="type">The type.</param>
	/// <param name="valueType">Type of the actual value</param>
	/// <returns><c>true</c> if [is typed identifier] [the specified type]; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">nameof(type)</exception>
	public static bool IsTypedIdentifier(Type type, [NotNullWhen(true)] out Type valueType) {
		if(type is null)
			throw new ArgumentNullException(nameof(type));

		if(type.BaseType is Type baseType &&
		   baseType.IsGenericType &&
		   baseType.GetGenericTypeDefinition() == typeof(TypedIdentifier<,>)) {
			valueType = baseType.GetGenericArguments()[1];
			return true;
		}

		valueType = null;
		return false;
	}

	/// <summary>Determines whether [is typed identifier] [the specified type].</summary>
	/// <param name="type">The type.</param>
	/// <returns><c>true</c> if [is typed identifier] [the specified type]; otherwise, <c>false</c>.</returns>
	public static bool IsTypedIdentifier(Type type) => IsTypedIdentifier(type, out _);

	/// <summary>Creates the factory.</summary>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="typedIdentifierType">Type of the typed identifier.</param>
	/// <returns></returns>
	/// <exception cref="ArgumentException">
	///     $"Type '{typedIdentifierType}' is not a TypedIdentifier type,
	///     nameof(typedIdentifierType) or $"Type '{typedIdentifierType}' is not a TypedIdentifier type,
	///     nameof(typedIdentifierType)
	/// </exception>
	private static Func<TValue, object> CreateFactory<TValue>(Type typedIdentifierType) where TValue : notnull {
		if(!IsTypedIdentifier(typedIdentifierType))
			throw new ArgumentException($"Type '{typedIdentifierType}' is not a TypedIdentifier type",
				nameof(typedIdentifierType));

		var ctor = typedIdentifierType.GetConstructor(new[] {typeof(TValue)});
		if(ctor is null)
			throw new ArgumentException(
				$"Type '{typedIdentifierType}' doesn't have a constructor with one parameter of type '{typeof(TValue)}'",
				nameof(typedIdentifierType));

		var param = Expression.Parameter(typeof(TValue), "value");
		var body = Expression.New(ctor, param);
		var lambda = Expression.Lambda<Func<TValue, object>>(body, param);
		return lambda.Compile();
	}
}