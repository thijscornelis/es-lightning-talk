using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ES.Framework.Application.Commands;

/// <inheritdoc />
public abstract class CommandHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	where TResponse : CommandResult, new()
{
	 protected readonly ILogger<CommandHandlerBase<TRequest, TResponse>> _logger;

	 /// <summary>Initializes a new instance of the <see cref="CommandHandlerBase{TRequest,TResponse}" /> class.</summary>
	 /// <param name="logger">The logger.</param>
	 protected CommandHandlerBase(ILogger<CommandHandlerBase<TRequest, TResponse>> logger) => _logger = logger;

	 /// <inheritdoc />
	 public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken) {
		  var stopwatch = new Stopwatch();
		  stopwatch.Start();
		  var response = new TResponse();
		  try {
				response = await HandleAsync(request, cancellationToken);
		  }
		  catch(Exception ex) {
				response.SetException(ex);
		  }
		  finally {
				stopwatch.Stop();
				response.SetElapsedTime(stopwatch.Elapsed);
		  }
		  return response;
	 }

	 /// <summary>Handles the command asynchronously.</summary>
	 /// <param name="request">The request.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>Task.</returns>
	 protected abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
