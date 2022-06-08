using ES.Framework.Domain.Events;
using ES.Framework.Domain.Projections;
using System.Text.Json.Serialization;

namespace ES.Framework.Domain.Documents;

/// <summary>Specific document used to wrap and save aggregate events.</summary>
public record EventDocument : Document
{
	 /// <summary>Gets the identifier.</summary>
	 /// <value>The identifier.</value>
	 [JsonPropertyName("id")]
	 public EventId Id { get; init; }

	 /// <summary>Gets the aggregate version this event corresponds to.</summary>
	 /// <value>The aggregate version.</value>
	 [JsonPropertyName("version")]
	 public long AggregateVersion { get; init; }

	 /// <summary>Gets the type of the aggregate.</summary>
	 /// <value>The type of the aggregate.</value>
	 [JsonPropertyName("aggregateType")]
	 public string AggregateType { get; init; }

	 /// <summary>Gets the payload.</summary>
	 /// <value>The payload.</value>
	 [JsonPropertyName("payload")]
	 public object Payload { get; init; }

	 /// <summary>Gets the type of the event.</summary>
	 /// <value>The type of the event.</value>
	 [JsonPropertyName("payloadType")]
	 public string PayloadType { get; init; }

	 /// <summary>Gets the aggregate identifier.</summary>
	 /// <value>The aggregate identifier.</value>
	 [JsonPropertyName("aggregateId")]
	 public string AggregateId { get; init; }
}

/// <summary>Specific document used to wrap and save projections.</summary>
/// <typeparam name="TProjection">The type of the projection.</typeparam>
public record ProjectionDocument<TProjection> : Document
{
	 /// <summary>Gets the identifier.</summary>
	 /// <value>The identifier.</value>
	 [JsonPropertyName("id")]
	 public ProjectionId Id { get; init; }

	 /// <summary>Gets the payload.</summary>
	 /// <value>The payload.</value>
	 [JsonPropertyName("payload")]
	 public TProjection Payload { get; init; }

	 /// <summary>Gets the type of the event.</summary>
	 /// <value>The type of the event.</value>
	 [JsonPropertyName("payloadType")]
	 public string PayloadType { get; init; }
}
