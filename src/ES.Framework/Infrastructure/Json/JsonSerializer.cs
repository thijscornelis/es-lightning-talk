using ES.Framework.Domain.TypedIdentifiers.Serializers;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace ES.Framework.Infrastructure.Json;

/// <summary>Custom JsonSerializer overwriting the default serialization settings</summary>
public static class JsonSerializer
{
	 public static readonly Lazy<JsonSerializerOptions> DefaultSerializerOptions = new(() => {
		  var options = new JsonSerializerOptions(JsonSerializerDefaults.Web) {
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				NumberHandling = JsonNumberHandling.Strict,
				WriteIndented = true
		  };
		  options.Converters.Add(new TypedIdentifierJsonConverterFactory());
		  return options;
	 });

	 public static TValue Deserialize<TValue>(ref Utf8JsonReader reader)
		 => System.Text.Json.JsonSerializer.Deserialize<TValue>(ref reader, DefaultSerializerOptions.Value);

	 public static string Serialize<TValue>(this TValue value) =>
		 System.Text.Json.JsonSerializer.Serialize(value, DefaultSerializerOptions.Value);

	 public static TValue Deserialize<TValue>(this string value) =>
		 System.Text.Json.JsonSerializer.Deserialize<TValue>(value, DefaultSerializerOptions.Value);

	 public static byte[] SerializeToUtf8Bytes<TValue>(TValue value) =>
		 System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value, DefaultSerializerOptions.Value);

}