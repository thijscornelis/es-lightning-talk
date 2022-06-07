using ES.Framework.Application.Commands;
using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Documents;
using ES.Framework.Domain.Documents.Design;
using ES.Framework.Domain.Exceptions;
using ES.Framework.Infrastructure.Attributes;
using ES.Framework.Infrastructure.Cosmos;
using ES.Framework.Infrastructure.Cosmos.Json;
using ES.Framework.Infrastructure.Dates;
using ES.Framework.Infrastructure.Json;
using ES.Sample.Application.Commands.ChangeName.V1;
using ES.Sample.Application.Commands.Create.V1;
using ES.Sample.Application.Commands.Deposit.V1;
using ES.Sample.Application.Commands.Withdraw.V1;
using ES.Sample.Domain.Aggregates;
using ES.Sample.Domain.Exceptions;
using ES.Sample.Domain.Repositories.Design;
using ES.Sample.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(CreateCommand).Assembly);
builder.Services.AddTransient<IDateTimeProvider, UniversalDateTimeProvider>();
builder.Services.AddTransient<IAttributeValueResolver, AttributeValueResolver>();
builder.Services.AddSingleton(typeof(IHostedService), (c) => {
	var db = c.GetRequiredService<Database>();
	var service = new CosmosChangeFeedProcessor(db.GetContainer("leases"), db.GetContainer("events"),
		c.GetService<ILogger<CosmosChangeFeedProcessor>>());
	return service;
});
builder.Services.AddTransient(typeof(IPartitionKeyResolver<,,,>), typeof(PartitionKeyResolver<,,,>));
builder.Services.AddTransient(typeof(IEventDocumentConverter<,,,>), typeof(EventDocumentConverter<,,,>));

RegisterRepositories(builder);

builder.Services.Configure<JsonOptions>(o => {
	 var options = JsonSerializer.DefaultSerializerOptions.Value;
	 o.JsonSerializerOptions.PropertyNamingPolicy = options.PropertyNamingPolicy;
	 o.JsonSerializerOptions.NumberHandling = options.NumberHandling;
	 o.JsonSerializerOptions.WriteIndented = options.WriteIndented;
	 foreach(var converter in options.Converters) {
		  o.JsonSerializerOptions.Converters.Add(converter);
	 }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
	 app.UseSwagger();
	 app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/bank-accounts/{id}", async ([FromRoute] Guid id, IBankAccountRepository repository, CancellationToken cancellationToken) => {
	 var aggregate = await repository.FindAsync(BankAccountId.CreateNew(id), cancellationToken);
	 return aggregate == null ? Results.NotFound() : Results.Json(aggregate.State, statusCode: (int) HttpStatusCode.OK, options: JsonSerializer.DefaultSerializerOptions.Value);
});

app.MapPost("api/bank-accounts", async (IMediator mediator, [FromBody] CreateCommand command, CancellationToken cancellationToken) => (await mediator.Send(command, cancellationToken)).ToHttpResult());
app.MapPut("api/bank-accounts/{id}/name", async (IMediator mediator, [FromQuery] Guid id, [FromBody] ChangeNameCommand command, CancellationToken cancellationToken) => (await mediator.Send(command with { BankAccountId = BankAccountId.CreateNew(id) }, cancellationToken)).ToHttpResult());
app.MapPut("api/bank-accounts/{id}/withdraw", async (IMediator mediator, [FromQuery] Guid id, [FromBody] WithdrawCommand command, CancellationToken cancellationToken) => (await mediator.Send(command with { BankAccountId = BankAccountId.CreateNew(id) }, cancellationToken)).ToHttpResult());
app.MapPut("api/bank-accounts/{id}/deposit", async (IMediator mediator, [FromQuery] Guid id, [FromBody] DepositCommand command, CancellationToken cancellationToken) => (await mediator.Send(command with { BankAccountId = BankAccountId.CreateNew(id) }, cancellationToken)).ToHttpResult());

await EnsureDatabaseAvailability(app);

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
					"snapshots",
					"/partitionKey",
					cancellationToken: cancellationTokenSource.Token), db.CreateContainerIfNotExistsAsync(
					"leases",
					"/partitionKey",
					cancellationToken: cancellationTokenSource.Token), db.CreateContainerIfNotExistsAsync(
					"checkpoints",
					"/partitionKey",
					cancellationToken: cancellationTokenSource.Token), db.CreateContainerIfNotExistsAsync(
					"projections",
					"/partitionKey",
					cancellationToken: cancellationTokenSource.Token));
		  }
	 }
}

void RegisterRepositories(WebApplicationBuilder webApplicationBuilder) {
	 webApplicationBuilder.Services.AddTransient<ICosmosQuery, CosmosQuery>();
	 webApplicationBuilder.Services.AddSingleton(c => new CosmosClient("https://localhost:8081",
		 "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
		 new CosmosClientOptions {
			  Serializer = new CosmosJsonSerializer(),
			  ConnectionMode = ConnectionMode.Gateway
		 }));

	 webApplicationBuilder.Services.AddSingleton(c => {
		  var client = c.GetService<CosmosClient>() ??
							throw new ArgumentNullException(nameof(CosmosClient), "Could not be resolved by the IOC");
		  return client.GetDatabase("es-lightning-talk");
	 });
	 webApplicationBuilder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>(c => {
		  var database = c.GetService<Database>() ?? throw new ArgumentNullException(nameof(Database), "Could not be resolved by the IOC");
		  var container = database.GetContainer("events");
		  return new BankAccountRepository(new CosmosDocumentRepository(container, c.GetService<ICosmosQuery>()),
			  c.GetService<IEventDocumentConverter<BankAccount, BankAccountId, BankAccountState, Guid>>(),
			  c.GetService<IPartitionKeyResolver<BankAccount, BankAccountId, BankAccountState, Guid>>());
	 });
}

/// <summary>Class CommandResultExtensions.</summary>
public static class CommandResultExtensions
{
	 /// <summary>Converts to <see cref="CommandResult" /> to a valid <see cref="HttpStatusCode" />.</summary>
	 /// <param name="result">The result.</param>
	 /// <returns>Microsoft.AspNetCore.Http.IResult.</returns>
	 public static IResult ToHttpResult(this CommandResult result) {
		  if(result == null)
				return Results.StatusCode((int) HttpStatusCode.InternalServerError);

		  if(result.HasExecutedSuccessfully)
				return Results.Json(result, statusCode: (int) HttpStatusCode.OK, options: JsonSerializer.DefaultSerializerOptions.Value);

		  return result.Exception switch {
				AggregateNotFoundException e => Results.NotFound(),
				ValidationException e => Results.BadRequest(new ProblemDetails() {
					 Status = (int) HttpStatusCode.BadRequest,
					 Detail = e.GetBaseException().Message,
					 Title = "PEBCAK"
				}),
				ApplicationException e => Results.BadRequest(new ProblemDetails() {
					 Status = (int) HttpStatusCode.BadRequest,
					 Detail = e.GetBaseException().Message,
					 Title = e.GetBaseException().GetType().Name
				}),
				_ => Results.StatusCode((int) HttpStatusCode.InternalServerError)
		  };
	 }
}
