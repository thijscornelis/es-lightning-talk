using ES.Framework.Application.Commands;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Application.Commands.Create;

/// <inheritdoc />
public record CreateCommandResult : CommandResult
{
	 /// <summary>Gets the bank account identifier.</summary>
	 /// <value>The identifier.</value>
	 public BankAccountId Id { get; init; }
}
