using ES.Framework.Domain.TypedIdentifiers.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = ES.Framework.Infrastructure.Json.JsonSerializer;

namespace ES.Framework.Domain.TypedIdentifiers.Serializers;

/// <summary>JSON converter for typed identifiers</summary>
/// <typeparam name="TTypedIdentifier">The type of the identity.</typeparam>
/// <typeparam name="TValue">The type of the key.</typeparam>
/// <seealso cref="JsonConverter{TTypedIdentifier}" />
internal class TypedIdentifierJsonConverter<TTypedIdentifier, TValue> : JsonConverter<TTypedIdentifier>
	where TTypedIdentifier : TypedIdentifier<TTypedIdentifier>
	where TValue : notnull
{
	/// <inheritdoc />
	public override TTypedIdentifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		if(reader.TokenType is JsonTokenType.Null)
			return null;

		var value = JsonSerializer.Deserialize<TValue>(ref reader);
		var factory = TypedIdentifierHelper.GetFactory<TValue>(typeToConvert);
		return (TTypedIdentifier) factory(value);
	}

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, TTypedIdentifier value, JsonSerializerOptions options) {
		if(value is null) {
			writer.WriteNullValue();
		}
		else {
			if(int.TryParse(value.Value, out var intValue))
				writer.WriteNumberValue(intValue);
			else if(decimal.TryParse(value.Value, out var decimalValue))
				writer.WriteNumberValue(decimalValue);
			else
				writer.WriteStringValue(value.Value);
		}
	}
}