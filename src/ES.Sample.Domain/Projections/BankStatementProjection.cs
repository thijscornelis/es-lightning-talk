using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Projections;

public record BankStatementProjection
{
	public decimal Amount { get; init; }
	public DateTime On { get; init; }
	public BankAccountId BankAccountId { get; init; }
}