using ES.Framework.Application.Commands;
using ES.Sample.Domain.Repositories.Design;
using Microsoft.Extensions.Logging;

namespace ES.Sample.Application.Commands.Create;

/// <summary>Handler for an <see cref="CreateCommand" />.</summary>
/// <inheritdoc cref="CommandHandlerBase{CreateBankAccountCommand, CreateBankAccountCommandResult}" />
public class CreateHandler : CommandHandlerBase<CreateCommand, CreateCommandResult>
{
	 private readonly IBankAccountRepository _repository;

	 /// <inheritdoc />
	 public CreateHandler(ILogger<CreateHandler> logger, IBankAccountRepository repository) : base(logger) => _repository = repository;

	 /// <inheritdoc />
	 protected override Task<CreateCommandResult> HandleAsync(CreateCommand request, CancellationToken cancellationToken) => throw new NotImplementedException();
}
