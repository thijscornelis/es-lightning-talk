using ES.Framework.Application.Commands;
using ES.Sample.Application.Commands.ChangeName.V1;
using ES.Sample.Domain.Repositories.Design;
using Microsoft.Extensions.Logging;

namespace ES.Sample.Application.Commands.ChangeName;

/// <summary>Handler for an <see cref="ChangeNameCommand" />.</summary>
/// <inheritdoc cref="CommandHandlerBase{ChangeNameCommand, ChangeNameCommandResult}" />
public class ChangeNameCommandHandler : CommandHandlerBase<ChangeNameCommand, ChangeNameCommandResult>
{
	 private readonly IBankAccountRepository _repository;

	 /// <inheritdoc />
	 public ChangeNameCommandHandler(ILogger<ChangeNameCommandHandler> logger, IBankAccountRepository repository) : base(logger) => _repository = repository;

	 /// <inheritdoc />
	 protected override async Task<ChangeNameCommandResult> HandleAsync(ChangeNameCommand request, CancellationToken cancellationToken) {
		  var aggregate = await _repository.GetAsync(request.BankAccountId, cancellationToken);
		  aggregate.SetName(request.Name);
		  aggregate = await _repository.SaveAsync(aggregate, cancellationToken);

		  return new() {
				Name = aggregate.State.Name
		  };
	 }
}
