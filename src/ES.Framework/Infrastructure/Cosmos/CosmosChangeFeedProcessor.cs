using ES.Framework.Domain.Documents;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ES.Framework.Infrastructure.Cosmos;

/// <summary>
/// Class CosmosChangeFeedProcessor.
/// Implements the <see cref="IHostedService" />
/// </summary>
/// <seealso cref="IHostedService" />
public class CosmosChangeFeedProcessor : IHostedService
{
	private readonly ILogger<CosmosChangeFeedProcessor> _logger;
	private readonly ChangeFeedProcessor _changeFeedProcessor;
	private readonly string _processorName;
	/// <summary>
	/// Initializes a new instance of the <see cref="CosmosChangeFeedProcessor"/> class.
	/// </summary>
	/// <param name="leaseContainer">The leases container.</param>
	/// <param name="eventsContainer">The events container.</param>
	/// <param name="logger">The logger.</param>
	public CosmosChangeFeedProcessor(Container leaseContainer, Container eventsContainer, ILogger<CosmosChangeFeedProcessor> logger) {
		_logger = logger;
		_processorName = $"PROCESSOR_{Environment.MachineName}_{Environment.ProcessId}";
		var builder = eventsContainer.GetChangeFeedProcessorBuilder<EventDocument>("CosmosChangeFeedProcessor", OnChangesDelegate)
			.WithInstanceName(_processorName)
			.WithStartTime(DateTime.MinValue.ToUniversalTime())
			.WithLeaseAcquireNotification(OnLeaseAcquired)
			.WithLeaseReleaseNotification(OnLeaseReleased)
			.WithErrorNotification(OnError)
			.WithLeaseContainer(leaseContainer);

		_changeFeedProcessor = builder.Build();
	}

	private Task OnChangesDelegate(ChangeFeedProcessorContext context, IReadOnlyCollection<EventDocument> documents, CancellationToken cancellationToken) {

		foreach(var document in documents) {
			_logger.LogDebug("{0} {1}", document.AggregateType, document.EventType);
		}

		return Task.CompletedTask;
	}

	/// <summary>Gets called when there are errors which originate from the <see cref="ChangeFeedProcessor"/> </summary>
	/// <param name="leaseToken">The lease token.</param>
	/// <param name="exception">The exception.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task OnError(string leaseToken, Exception exception) {
		_logger.LogError(exception, "{ProcessorName}: Lease {LeaseToken} ERROR, {ErrorMessage}", _processorName, leaseToken, exception.GetBaseException().Message);
		return Task.CompletedTask;
	}

	/// <summary>Called when the <see cref="ChangeFeedProcessor"/> has acquired its lease</summary>
	/// <param name="leaseToken">The lease token.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task OnLeaseAcquired(string leaseToken) {
		_logger.LogInformation("{ProcessorName}: Lease {LeaseToken} ACQUIRED", _processorName, leaseToken);
		return Task.CompletedTask;
	}

	/// <summary>Called when the <see cref="ChangeFeedProcessor"/> has released its lease</summary>
	/// <param name="leaseToken">The lease token.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task OnLeaseReleased(string leaseToken) {
		_logger.LogInformation("{ProcessorName}: Lease {LeaseToken} RELEASED", _processorName, leaseToken);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public Task StartAsync(CancellationToken cancellationToken) => _changeFeedProcessor.StartAsync();

	/// <inheritdoc />
	public Task StopAsync(CancellationToken cancellationToken) => _changeFeedProcessor.StopAsync();
}