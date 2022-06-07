using System.Text.Json.Serialization;

namespace ES.Framework.Domain.Documents;

/// <summary>Base class for a document that can be saved in a document database.</summary>
public abstract record Document
{
	 /// <summary>Gets the identifier.</summary>
	 /// <value>The identifier.</value>
	 [JsonPropertyName("id")]
	 public DocumentId Id { get; init; }

	 /// <summary>Gets the timestamp.</summary>
	 /// <value>The timestamp.</value>
	 [JsonPropertyName("timestamp")]
	 public DateTime Timestamp { get; init; }

	 /// <summary>Gets the payload.</summary>
	 /// <value>The payload.</value>
	 [JsonPropertyName("payload")]
	 public object Payload { get; init; }

	 /// <summary>Gets the type of the event.</summary>
	 /// <value>The type of the event.</value>
	 [JsonPropertyName("eventType")]
	 public string EventType { get; init; }

	 /// <summary>Gets the partition key.</summary>
	 /// <value>The partition key.</value>
	 [JsonPropertyName("partitionKey")]
	 public string PartitionKey { get; init; }
}
