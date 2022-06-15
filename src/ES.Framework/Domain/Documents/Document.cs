using System.Text.Json.Serialization;

namespace ES.Framework.Domain.Documents;

/// <summary>Document that can be saved in a document database.</summary>
public abstract record Document
{
	 /// <summary>Gets the timestamp.</summary>
	 /// <value>The timestamp.</value>
	 [JsonPropertyName("timestamp")]
	 public DateTime Timestamp { get; init; }

	 /// <summary>Gets the partition key.</summary>
	 /// <value>The partition key.</value>
	 [JsonPropertyName("partitionKey")]
	 public string PartitionKey { get; init; }
}
