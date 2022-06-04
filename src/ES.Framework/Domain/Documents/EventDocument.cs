using System.Text.Json.Serialization;

namespace ES.Framework.Domain.Documents;

/// <summary>Specific document used to wrap and save AggregateEvents.</summary>
public record EventDocument : Document
{
	 /// <summary>Gets the aggregate version this event corresponds to.</summary>
	 /// <value>The aggregate version.</value>
	 [JsonPropertyName("version")]
	 public long AggregateVersion { get; init; }

	 /// <summary>Gets the type of the aggregate.</summary>
	 /// <value>The type of the aggregate.</value>
	 [JsonPropertyName("aggregateType")]
	 public string AggregateType { get; init; }

	 /// <summary>Gets the aggregate identifier.</summary>
	 /// <value>The aggregate identifier.</value>
	 [JsonPropertyName("aggregateId")]
	 public string AggregateId { get; init; }
}
