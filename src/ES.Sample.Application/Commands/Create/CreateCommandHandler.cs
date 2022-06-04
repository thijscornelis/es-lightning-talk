using ES.Framework.Application.Commands;
using ES.Sample.Application.Commands.Create.V1;
using ES.Sample.Domain.Aggregates;
using ES.Sample.Domain.Repositories.Design;
using Microsoft.Extensions.Logging;

namespace ES.Sample.Application.Commands.Create;

/// <summary>Handler for an <see cref="CreateCommand" />.</summary>
/// <inheritdoc cref="CommandHandlerBase{CreateBankAccountCommand, CreateBankAccountCommandResult}" />
public class CreateCommandHandler : CommandHandlerBase<CreateCommand, CreateCommandResult>
{
	 private readonly IBankAccountRepository _repository;

	 /// <inheritdoc />
	 public CreateCommandHandler(ILogger<CreateCommandHandler> logger, IBankAccountRepository repository) : base(logger) => _repository = repository;

	 /// <inheritdoc />
	 protected override async Task<CreateCommandResult> HandleAsync(CreateCommand request, CancellationToken cancellationToken) {
		  var account = new BankAccount(request.Name, request.WithdrawLimit);

		  account = await _repository.SaveAsync(account, cancellationToken);

		  return new() {
				Id = account.Id,
				Balance = account.State.Balance
		  };
	 }
}
