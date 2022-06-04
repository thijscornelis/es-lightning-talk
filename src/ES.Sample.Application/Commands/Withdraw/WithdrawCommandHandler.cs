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
	 protected override Task<WithdrawCommandResult> HandleAsync(WithdrawCommand request, CancellationToken cancellationToken) => throw new NotImplementedException();
}
