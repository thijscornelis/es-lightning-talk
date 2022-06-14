using ES.Framework.Application.Commands;
using ES.Framework.Domain.Exceptions;
using ES.Framework.Infrastructure.Json;
using ES.Sample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
			return Results.Json(result, statusCode: (int) HttpStatusCode.OK,
				options: JsonSerializer.DefaultSerializerOptions.Value);

		return result.Exception switch {
			AggregateNotFoundException e => Results.NotFound(),
			ValidationException e => Results.BadRequest(new ProblemDetails {
				Status = (int) HttpStatusCode.BadRequest,
				Detail = e.GetBaseException().Message,
				Title = "PEBCAK"
			}),
			ApplicationException e => Results.BadRequest(new ProblemDetails {
				Status = (int) HttpStatusCode.BadRequest,
				Detail = e.GetBaseException().Message,
				Title = e.GetBaseException().GetType().Name
			}),
			_ => Results.StatusCode((int) HttpStatusCode.InternalServerError)
		};
	}
}