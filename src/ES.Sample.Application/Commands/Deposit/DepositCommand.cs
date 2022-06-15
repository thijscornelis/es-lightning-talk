using ES.Framework.Application.Commands;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Application.Commands.Deposit
{
	 namespace V1
	 {
		  /// <inheritdoc />
		  public record DepositCommand : CommandBase<DepositCommandResult>
		  {
				/// <summary>Gets the bank account identifier.</summary>
				/// <value>The bank account identifier.</value>
				public BankAccountId BankAccountId { get; init; }
				/// <summary>Gets the amount.</summary>
				/// <value>The amount.</value>
				public decimal Amount { get; init; }

				/// <summary>Gets the description.</summary>
				/// <value>The description.</value>
				public string Description { get; init; }
		  }
	 }
}
