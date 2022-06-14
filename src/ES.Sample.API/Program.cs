using ES.Sample.API.Controllers;
using ES.Sample.API.Extensions;
using Microsoft.Azure.Cosmos;

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

app.MapWriteEndpoints();
app.MapReadEndpoints();

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
				"leases",
				"/partitionKey",
				cancellationToken: cancellationTokenSource.Token), db.CreateContainerIfNotExistsAsync(
				"projections",
				"/partitionKey",
				cancellationToken: cancellationTokenSource.Token));
		}
	}
}