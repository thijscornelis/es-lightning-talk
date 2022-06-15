using ES.Framework.Domain.Aggregates.Attributes;
using ES.Framework.Domain.Projections;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Projections;
[PartitionKey("{0}")]
public record BankAccountProjection
{
	 public BankAccountId BankAccountId { get; init; }
	 public string Name { get; init; }
	 public decimal Balance { get; init; }
	 public int NumberOfDeposits { get; init; }
	 public int NumberOfWithdrawals { get; init; }

	 /// <summary>Gets the projection identifier.</summary>
	 /// <param name="eventId">The event identifier.</param>
	 /// <returns>ProjectionId.</returns>
	 public static ProjectionId GetProjectionId(BankAccountId eventId) => new($"{nameof(BankAccountProjection)}-{eventId.TypedValue:N}");
}
