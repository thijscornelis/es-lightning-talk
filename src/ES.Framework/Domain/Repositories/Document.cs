using ES.Framework.Domain.TypedIdentifiers;
using System.Text.Json.Serialization;

namespace ES.Framework.Domain.Repositories;

public abstract record Document //: IContainPartitionKey
{
	[JsonPropertyName("id")]
	 public DocumentId Id { get; init; }

	 [JsonPropertyName("timestamp")]
	 public DateTime Timestamp { get; init; }

	 [JsonPropertyName("payload")]
	 public object Payload { get; init; }

	 [JsonPropertyName("eventType")]
	 public string EventType { get; init; }

	 [JsonPropertyName("_partitionKey")]
	 public string PartitionKey { get; init; }
}

public record ProjectionDocument : Document
{
	 [JsonPropertyName("checkpoint")]
	 public Checkpoint Checkpoint { get; set; }
}

public record EventDocument : Document
{
	 [JsonPropertyName("version")]
	 public long Version { get; init; }

	 [JsonPropertyName("aggregateId")]
	 public string AggregateId { get; init; }
}

public record Checkpoint
{
	 /// <summary>Gets or sets the identifier.</summary>
	 /// <value>The identifier.</value>
	 [JsonPropertyName("id")]
	 public CheckpointId Id { get; init; }

	 /// <summary>Gets or sets the timestamp.</summary>
	 /// <value>The timestamp.</value>
	 [JsonPropertyName("timestamp")]
	 public DateTime Timestamp { get; init; }

}

public record CheckpointId : TypedIdentifier<CheckpointId, Guid>
{
	 /// <inheritdoc />
	 public CheckpointId(Guid value) : base(value) {
	 }
}
public record DocumentId : TypedIdentifier<DocumentId, Guid>
{
	 /// <inheritdoc />
	 public DocumentId(Guid value) : base(value) { }
}

public interface IContainPartitionKey
{
	 string PartitionKey { get; }
}

public record ContinuationToken(string Value);