using ES.Framework.Application.Commands;

namespace ES.Sample.Application.Commands.Deposit
{
	 namespace V1
	 {
		  /// <inheritdoc />
		  public record DepositCommandResult : CommandResult
		  {
				/// <summary>Gets or sets the balance.</summary>
				/// <value>The balance.</value>
				public decimal Balance { get; init; }
		  }
	 }
}
