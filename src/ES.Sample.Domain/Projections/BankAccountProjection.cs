using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Projections;

public record BankAccountProjection
{
	public BankAccountId BankAccountId { get; init; }
	public string Name { get; init; }
	public decimal Balance { get; init; }
	public int NumberOfDeposits { get; init; }
	public int NumberOfWithdrawals { get; init; }
}