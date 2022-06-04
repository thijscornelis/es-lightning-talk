using ES.Framework.Application.Commands;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Application.Commands.ChangeName
{
	 namespace V1
	 {
		  /// <inheritdoc />
		  public record ChangeNameCommand : CommandBase<ChangeNameCommandResult>
		  {
				/// <summary>Gets the bank account identifier.</summary>
				/// <value>The bank account identifier.</value>
				public BankAccountId BankAccountId { get; init; }

				/// <summary>Gets the name.</summary>
				/// <value>The name.</value>
				public string Name { get; init; }
		  }
	 }
}
