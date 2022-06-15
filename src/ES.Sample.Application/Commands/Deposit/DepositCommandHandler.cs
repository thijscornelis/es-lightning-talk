using ES.Framework.Application.Commands;
using ES.Sample.Application.Commands.Deposit.V1;
using ES.Sample.Domain.Repositories.Design;
using Microsoft.Extensions.Logging;

namespace ES.Sample.Application.Commands.Deposit;

/// <summary>Handler for an <see cref="DepositCommand" />.</summary>
/// <inheritdoc cref="CommandHandlerBase{DepositCommand, DepositCommandCommandResult}" />
public class DepositCommandHandler : CommandHandlerBase<DepositCommand, DepositCommandResult>
{
	 private readonly IBankAccountRepository _repository;

	 /// <inheritdoc />
	 public DepositCommandHandler(ILogger<CommandHandlerBase<DepositCommand, DepositCommandResult>> logger, IBankAccountRepository repository) : base(logger) => _repository = repository;

	 /// <inheritdoc />
	 protected override async Task<DepositCommandResult> HandleAsync(DepositCommand request, CancellationToken cancellationToken) {
		  var aggregate = await _repository.GetAsync(request.BankAccountId, cancellationToken);
		  aggregate.Deposit(request.Amount, request.Description);
		  aggregate = await _repository.SaveAsync(aggregate, cancellationToken);

		  return new() {
				Balance = aggregate.State.Balance
		  };
	 }
}
