using ES.Framework.Infrastructure.Cosmos.Exceptions;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = ES.Framework.Infrastructure.Json.JsonSerializer;

namespace ES.Framework.Infrastructure.Cosmos.Json;
/// <summary>By default CosmosDB SDK uses Newtonsoft, this is the System.Text implementation instead</summary>
/// <seealso cref="Microsoft.Azure.Cosmos.CosmosSerializer" />
public class CosmosJsonSerializer : CosmosSerializer
{
	 private readonly JsonReaderOptions _readerOptions;
	 private readonly JsonWriterOptions _writerOptions;

	 /// <summary>Initializes a new instance of the <see cref="CosmosJsonSerializer" /> class.</summary>
	 public CosmosJsonSerializer() {
		  _readerOptions = new JsonReaderOptions {
				CommentHandling = JsonCommentHandling.Skip,
				AllowTrailingCommas = false,
		  };
		  _writerOptions = new JsonWriterOptions {
				Indented = false,
				SkipValidation = false
		  };

	 }

	 /// <summary>Convert a Stream of JSON to an object. The implementation is responsible for Disposing of the stream, including when an exception is thrown, to avoid memory leaks.</summary>
	 /// <typeparam name="T">Any type passed to <see cref="T:Microsoft.Azure.Cosmos.Container" />.</typeparam>
	 /// <param name="stream">The Stream response containing JSON from Cosmos DB.</param>
	 /// <returns>The object deserialized from the stream.</returns>
	 public override T FromStream<T>(Stream stream) {
		  T result;
		  try {
				using(stream) {
					 if(typeof(Stream).IsAssignableFrom(typeof(T))) {
						  return (T) (object) stream;
					 }

					 using(var sr = new StreamReader(stream)) {
						  var jsonString = sr.ReadToEnd();
						  if(string.IsNullOrWhiteSpace(jsonString))
								return default!;
						  ReadOnlySpan<byte> jsonReadOnlySpan = Encoding.UTF8.GetBytes(jsonString);
						  var reader = new Utf8JsonReader(jsonReadOnlySpan, _readerOptions);
						  result = JsonSerializer.Deserialize<T>(ref reader) ?? throw new CosmosSerializerException("Could not serialize object from stream");
					 }
				}
		  }
		  catch {
				stream.Dispose();
				throw;
		  }
		  return result;
	 }

	 /// <summary>Converts to stream.</summary>
	 /// <param name="input">Any type passed to <see cref="T:Microsoft.Azure.Cosmos.Container" />.</param>
	 /// <returns>A readable Stream containing JSON of the serialized object.</returns>
	 public override Stream ToStream<T>(T input) {
		  var stream = new MemoryStream();
		  using var writer = new Utf8JsonWriter(stream, _writerOptions);
		  stream.Write(JsonSerializer.SerializeToUtf8Bytes(input));
		  stream.Position = 0;
		  return stream;
	 }
}
