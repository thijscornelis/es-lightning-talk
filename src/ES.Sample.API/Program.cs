using ES.Framework.Domain.Repositories.Design;
using ES.Framework.Infrastructure.Cosmos;
using ES.Framework.Infrastructure.Cosmos.Json;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(c => new CosmosClient("https://localhost:8081",
	"C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", new CosmosClientOptions {
		Serializer = new CosmosJsonSerializer(),
		ConnectionMode = ConnectionMode.Gateway
	}));

builder.Services.AddScoped(c => {
	var client = c.GetService<CosmosClient>() ??
	             throw new ArgumentNullException(nameof(CosmosClient), "Could not be resolved by the IOC");
	return client.GetDatabase("es-database");
});

builder.Services.AddTransient<IDocumentRepository, CosmosDocumentRepository>(c => {
	var database = c.GetService<Database>() ??
	               throw new ArgumentNullException(nameof(Database), "Could not be resolved by the IOC");
	var container = database.GetContainer("es-framework");
	return new CosmosDocumentRepository(container);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await EnsureDatabaseAvailability(app);

app.Run();

async Task EnsureDatabaseAvailability(WebApplication webApplication) {
	using(var scope = webApplication.Services.CreateScope()) {
		var cancellationTokenSource = new CancellationTokenSource();
		var cosmosClient = scope.ServiceProvider.GetService<CosmosClient>();
		if(cosmosClient != null) {
			var dbResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync("es-database",
				cancellationToken: cancellationTokenSource.Token);
			var db = dbResponse.Database;

			await Task.WhenAll(db.CreateContainerIfNotExistsAsync(
				"events",
				"/_partitionKey",
				cancellationToken: cancellationTokenSource.Token), db.CreateContainerIfNotExistsAsync(
				"snapshots",
				"/_partitionKey",
				cancellationToken: cancellationTokenSource.Token), db.CreateContainerIfNotExistsAsync(
				"leases",
				"/_partitionKey",
				cancellationToken: cancellationTokenSource.Token), db.CreateContainerIfNotExistsAsync(
				"checkpoints",
				"/_partitionKey",
				cancellationToken: cancellationTokenSource.Token), db.CreateContainerIfNotExistsAsync(
				"projections",
				"/_partitionKey",
				cancellationToken: cancellationTokenSource.Token));
		}
	}
}