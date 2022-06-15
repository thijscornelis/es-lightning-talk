using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Documents;
using ES.Framework.Domain.Documents.Design;
using ES.Framework.Domain.Projections.Design;
using ES.Framework.Domain.Repositories;
using ES.Framework.Domain.Repositories.Design;
using ES.Framework.Infrastructure.Attributes;
using ES.Framework.Infrastructure.Cosmos;
using ES.Framework.Infrastructure.Cosmos.Json;
using ES.Framework.Infrastructure.Dates;
using ES.Framework.Infrastructure.Json;
using ES.Sample.Application.Commands.Create.V1;
using ES.Sample.Domain.Repositories.Design;
using ES.Sample.Infrastructure.Projectors;
using ES.Sample.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Sample.API.Extensions;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds Swagger.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <returns>IServiceCollection.</returns>
	public static IServiceCollection WithSwagger(this IServiceCollection services) {
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
		return services;
	}

	 /// <summary>
	 /// Withes the mediator.
	 /// </summary>
	 /// <param name="services">The services.</param>
	 /// <returns>IServiceCollection.</returns>
	 public static IServiceCollection WithMediatR(this IServiceCollection services) =>
		services.AddMediatR(typeof(CreateCommand).Assembly);

	 /// <summary>
	 /// Withes the framework.
	 /// </summary>
	 /// <param name="services">The services.</param>
	 /// <returns>IServiceCollection.</returns>
	 public static IServiceCollection WithFramework(this IServiceCollection services) {
		services.AddTransient<IDateTimeProvider, UniversalDateTimeProvider>();
		services.AddTransient<IAttributeValueResolver, AttributeValueResolver>();
		services.AddTransient<IAggregatePartitionKeyResolver, AggregatePartitionKeyResolver>();
		services.AddTransient<IEventDocumentConverter, EventDocumentConverter>();
		services.AddTransient<IProjectionPartitionKeyResolver, ProjectionPartitionKeyResolver>();
		services.AddTransient<IProjectionDocumentConverter, ProjectionDocumentConverter>();
		return services;
	}

	 /// <summary>
	 /// Withes the cosmos database.
	 /// </summary>
	 /// <param name="services">The services.</param>
	 /// <returns>IServiceCollection.</returns>
	 public static IServiceCollection WithCosmosDatabase(this IServiceCollection services) {
		services.AddTransient<ICosmosQuery, CosmosQuery>();
		services.AddSingleton(c => new CosmosClient("https://localhost:8081",
			"C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
			new CosmosClientOptions {
				Serializer = new CosmosJsonSerializer(),
				ConnectionMode = ConnectionMode.Gateway
			}));

		services.AddSingleton(c => {
			var client = c.GetRequiredService<CosmosClient>();
			return client.GetDatabase("es-lightning-talk");
		});
		return services;
	 }

	 /// <summary>
	 /// Withes the cosmos change feed processor.
	 /// </summary>
	 /// <param name="services">The services.</param>
	 /// <returns>IServiceCollection.</returns>
	 public static IServiceCollection WithCosmosChangeFeedProcessor(this IServiceCollection services) {
		services.AddSingleton(typeof(IHostedService), c => {
			var db = c.GetRequiredService<Database>();
			var service = new CosmosChangeFeedProcessor(
				c.GetService<ILogger<CosmosChangeFeedProcessor>>(),
				db.GetContainer("leases"),
				db.GetContainer("events"),
				c.GetService<IEnumerable<IProjector>>(),
				c.GetService<IEventDocumentConverter>()
			);
			return service;
		});
		return services;
	}

	 /// <summary>
	 /// Withes the json.
	 /// </summary>
	 /// <param name="services">The services.</param>
	 /// <returns>IServiceCollection.</returns>
	 public static IServiceCollection WithJson(this IServiceCollection services) {
		services.Configure<JsonOptions>(o => {
			var options = JsonSerializer.DefaultSerializerOptions.Value;
			o.JsonSerializerOptions.PropertyNamingPolicy = options.PropertyNamingPolicy;
			o.JsonSerializerOptions.NumberHandling = options.NumberHandling;
			o.JsonSerializerOptions.WriteIndented = options.WriteIndented;
			foreach(var converter in options.Converters)
				o.JsonSerializerOptions.Converters.Add(converter);
		});
		return services;
	}

	 /// <summary>
	 /// Withes the projectors.
	 /// </summary>
	 /// <param name="services">The services.</param>
	 /// <returns>IServiceCollection.</returns>
	 public static IServiceCollection WithProjectors(this IServiceCollection services) {

		var projectorTypes = typeof(BankAccountProjector).Assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(typeof(IProjector)));
		foreach(var projectorType in projectorTypes) {
			services.AddTransient(typeof(IProjector), projectorType);
		}

		return services;
	}

	 /// <summary>
	 /// Withes the repositories.
	 /// </summary>
	 /// <param name="services">The services.</param>
	 /// <returns>IServiceCollection.</returns>
	 public static IServiceCollection WithRepositories(this IServiceCollection services) {
		services.AddScoped<IBankAccountRepository, BankAccountRepository>(c => {
			var database = c.GetRequiredService<Database>();
			var container = database.GetContainer("events");
			return new BankAccountRepository(new CosmosDocumentRepository(container, c.GetService<ICosmosQuery>()),
				c.GetService<IEventDocumentConverter>(),
				c.GetService<IAggregatePartitionKeyResolver>());
		});


		services.AddTransient<IProjectionRepository>(c => {
			var database = c.GetRequiredService<Database>();
			var documentRepository = new CosmosDocumentRepository(database.GetContainer("projections"), c.GetService<ICosmosQuery>());
			return new ProjectionRepository(c.GetService<IProjectionPartitionKeyResolver>(), documentRepository, c.GetService<IProjectionDocumentConverter>());
		});
		return services;
	 }
}
