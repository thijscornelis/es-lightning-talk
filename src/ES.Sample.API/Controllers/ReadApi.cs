using ES.Framework.Domain.Repositories.Design;
using ES.Sample.Domain.Aggregates;
using ES.Sample.Domain.Projections;
using ES.Sample.Domain.Repositories.Design;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ES.Sample.API.Controllers;

/// <summary>
/// Class ReadApi.
/// </summary>
public static class ReadApi
{

	 /// <summary>
	 /// Maps the read endpoints.
	 /// </summary>
	 /// <param name="app">The application.</param>
	 /// <returns>Microsoft.AspNetCore.Builder.WebApplication.</returns>
	 public static WebApplication MapReadEndpoints(this WebApplication app) {

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
		  return app;
	 }
}