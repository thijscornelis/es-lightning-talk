using ES.Framework.Domain.TypedIdentifiers.Converters;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ES.Framework.Domain.TypedIdentifiers.Serializers;

/// <summary>Factor for the typed identifier JSON converter</summary>
/// <seealso cref="System.Text.Json.Serialization.JsonConverterFactory" />
public class TypedIdentifierJsonConverterFactory : JsonConverterFactory
{
	private static readonly ConcurrentDictionary<Type, JsonConverter> _cache = new();

	/// <inheritdoc />
	public override bool CanConvert(Type typeToConvert) => TypedIdentifierHelper.IsTypedIdentifier(typeToConvert);

	/// <inheritdoc />
	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) => _cache.GetOrAdd(typeToConvert, CreateConverter);

	private static JsonConverter CreateConverter(Type typeToConvert) {
		if(!TypedIdentifierHelper.IsTypedIdentifier(typeToConvert, out var valueType))
			throw new InvalidOperationException($"Cannot create converter for '{typeToConvert}'");
		var type = typeof(TypedIdentifierJsonConverter<,>).MakeGenericType(typeToConvert, valueType);
		return (JsonConverter) Activator.CreateInstance(type);
	}
}