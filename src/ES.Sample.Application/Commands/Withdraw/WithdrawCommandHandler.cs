using ES.Framework.Application.Commands;
using ES.Sample.Application.Commands.Withdraw.V1;
using ES.Sample.Domain.Repositories.Design;
using Microsoft.Extensions.Logging;

namespace ES.Sample.Application.Commands.Withdraw;

/// <summary>Handler for an <see cref="WithdrawCommand" />.</summary>
/// <inheritdoc cref="CommandHandlerBase{WithdrawCommand, WithdrawCommandResult}" />
public class WithdrawCommandHandler : CommandHandlerBase<WithdrawCommand, WithdrawCommandResult>
{
	 private readonly IBankAccountRepository _repository;

	 /// <inheritdoc />
	 public WithdrawCommandHandler(ILogger<CommandHandlerBase<WithdrawCommand, WithdrawCommandResult>> logger, IBankAccountRepository repository) : base(logger) => _repository = repository;

	 /// <inheritdoc />
	 protected override async Task<WithdrawCommandResult> HandleAsync(WithdrawCommand request, CancellationToken cancellationToken) {
		  var aggregate = await _repository.GetAsync(request.BankAccountId, cancellationToken);
		  var @throw = aggregate.Withdraw(request.Amount, request.Description);
		  aggregate = await _repository.SaveAsync(aggregate, cancellationToken);

		  if(@throw != null)
				throw @throw;

		  return new() {
				Balance = aggregate.State.Balance
		  };
	 }
}
