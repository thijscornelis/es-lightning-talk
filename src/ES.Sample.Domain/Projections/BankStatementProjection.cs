using ES.Framework.Domain.Aggregates.Attributes;
using ES.Framework.Domain.Projections;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Projections;

/// <summary>Class BankStatementProjection.</summary>
[PartitionKey("{0}")]
public record BankStatementProjection
{
	 public decimal Amount { get; init; }
	 public string Description { get; init; }
	 public DateTime On { get; init; }
	 public BankAccountId BankAccountId { get; init; }
	 public StatementId StatementId { get; init; }

	 /// <summary>Gets the projection identifier.</summary>
	 /// <param name="id">The identifier.</param>
	 /// <returns>ProjectionId.</returns>
	 public static ProjectionId GetProjectionId(StatementId id) => new($"{nameof(BankStatementProjection)}-{id.TypedValue:N}");
}
