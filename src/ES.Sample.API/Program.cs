using ES.Framework.Domain.Repositories.Design;
using ES.Sample.API.Extensions;
using ES.Sample.Application.Commands.ChangeName.V1;
using ES.Sample.Application.Commands.Create.V1;
using ES.Sample.Application.Commands.Deposit.V1;
using ES.Sample.Application.Commands.Withdraw.V1;
using ES.Sample.Domain.Aggregates;
using ES.Sample.Domain.Projections;
using ES.Sample.Domain.Repositories.Design;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.WithSwagger()
	.WithMediatR()
	.WithFramework()
	.WithCosmosDatabase()
	.WithCosmosChangeFeedProcessor()
	.WithRepositories()
	.WithJson()
	.WithProjectors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await EnsureDatabaseAvailability(app);

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

app.MapGet("api/bank-accounts/{id}/statements",
			  ([FromRoute] Guid id, IProjectionRepository repository) => {
					var result = new Result<BankStatementProjection[]>();
					var stopwatch = new Stopwatch();
					try {
						 stopwatch.Start();

						 var bankAccountId = BankAccountId.CreateNew(id);
						 var queryable = repository.Get<BankStatementProjection>(x => x.BankAccountId == bankAccountId);
						 result.SetResult(queryable.ToArray());
					}
					catch(Exception e) {
						 result.SetException(e);
					}
					finally {
						 stopwatch.Stop();
						 result.SetElapsedTime(stopwatch.Elapsed);
					}

					return result.ToHttpResult();
			  });

app.MapGet("api/bank-accounts/{id}",
	([FromRoute] Guid id, IProjectionRepository repository) => {
		 var result = new Result<BankAccountProjection>();
		 var stopwatch = new Stopwatch();
		 try {
			  stopwatch.Start();

			  var bankAccountId = BankAccountId.CreateNew(id);
			  var queryable = repository.Get<BankAccountProjection>(x => x.BankAccountId == bankAccountId);
			  result.SetResult(queryable.AsEnumerable().SingleOrDefault());
		 }
		 catch(Exception e) {
			  result.SetException(e);
		 }
		 finally {
			  stopwatch.Stop();
			  result.SetElapsedTime(stopwatch.Elapsed);
		 }

		 return result.ToHttpResult();
	});
app.MapGet("api/bank-accounts/{id}/state",
	async ([FromRoute] Guid id, IBankAccountRepository repository, CancellationToken cancellationToken) => {
		 var result = new Result<BankAccountState>();
		 var stopwatch = new Stopwatch();
		 try {
			  stopwatch.Start();

			  var bankAccountId = BankAccountId.CreateNew(id);
			  var bankAccount = await repository.GetAsync(bankAccountId, cancellationToken);
			  result.SetResult(bankAccount.State);
		 }
		 catch(Exception e) {
			  result.SetException(e);
		 }
		 finally {
			  stopwatch.Stop();
			  result.SetElapsedTime(stopwatch.Elapsed);
		 }

		 return result.ToHttpResult();
	});

app.Run();

async Task EnsureDatabaseAvailability(WebApplication webApplication) {
	using(var scope = webApplication.Services.CreateScope()) {
		var cancellationTokenSource = new CancellationTokenSource();
		var cosmosClient = scope.ServiceProvider.GetService<CosmosClient>();
		if(cosmosClient != null) {
			var dbResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync("es-lightning-talk",
				cancellationToken: cancellationTokenSource.Token);
			var db = dbResponse.Database;

			await Task.WhenAll(db.CreateContainerIfNotExistsAsync(
				"events",
				"/partitionKey",
				cancellationToken: cancellationTokenSource.Token), db.CreateContainerIfNotExistsAsync(
				"leases",
				"/partitionKey",
				cancellationToken: cancellationTokenSource.Token), db.CreateContainerIfNotExistsAsync(
				"projections",
				"/partitionKey",
				cancellationToken: cancellationTokenSource.Token));
		}
	}
}