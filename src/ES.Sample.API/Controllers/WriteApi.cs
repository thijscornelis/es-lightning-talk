using ES.Sample.Application.Commands.ChangeName.V1;
using ES.Sample.Application.Commands.Create.V1;
using ES.Sample.Application.Commands.Deposit.V1;
using ES.Sample.Application.Commands.Withdraw.V1;
using ES.Sample.Domain.Aggregates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ES.Sample.API.Controllers;

/// <summary>
/// Class WriteApi.
/// </summary>
public static class WriteApi
{
	 /// <summary>
	 /// Maps the write endpoints.
	 /// </summary>
	 /// <param name="app">The application.</param>
	 /// <returns>WebApplication.</returns>
	 public static WebApplication MapWriteEndpoints(this WebApplication app) {
		  app.MapPost("api/bank-accounts", async (IMediator mediator, [FromBody] CreateCommand command, CancellationToken cancellationToken) => {
				var result = await mediator.Send(command, cancellationToken);
				return result.ToHttpResult();
		  });
		  app.MapPut("api/bank-accounts/{id}/name", async (IMediator mediator, [FromQuery] Guid id, [FromBody] ChangeNameCommand command, CancellationToken cancellationToken) => {
				command = command with { BankAccountId = BankAccountId.CreateNew(id) };
				var result = await mediator.Send(command, cancellationToken);
				return result.ToHttpResult();
		  });
		  app.MapPut("api/bank-accounts/{id}/withdraw", async (IMediator mediator, [FromQuery] Guid id, [FromBody] WithdrawCommand command, CancellationToken cancellationToken) => {
				command = command with { BankAccountId = BankAccountId.CreateNew(id) };
				var result = await mediator.Send(command, cancellationToken);
				return result.ToHttpResult();
		  });
		  app.MapPut("api/bank-accounts/{id}/deposit", async (IMediator mediator, [FromQuery] Guid id, [FromBody] DepositCommand command, CancellationToken cancellationToken) => {
				command = command with { BankAccountId = BankAccountId.CreateNew(id) };
				var result = await mediator.Send(command, cancellationToken);
				return result.ToHttpResult();
		  });
		  return app;
	 }
}