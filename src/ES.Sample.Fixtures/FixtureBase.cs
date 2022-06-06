using Xunit;

namespace ES.Sample.Fixtures;

public abstract class FixtureBase : IAsyncLifetime
{
	 public CancellationTokenSource CancellationTokenSource { get; private set; }
	 public bool HasExecutedSuccessfully => Throws == null;
	 public Exception Throws { get; private set; }

	 /// <inheritdoc />
	 public Task DisposeAsync() {
		  CancellationTokenSource.Dispose();
		  return Task.CompletedTask;
	 }

	 /// <inheritdoc />
	 public virtual async Task InitializeAsync() {
		  try {
				Arrange();
				await InternalActAsync();
		  }
		  catch(Exception ex) {
				Throws = ex;
		  }
	 }

	 protected virtual void Arrange() => CancellationTokenSource = ArrangeCancellationTokenSource();

	 protected virtual CancellationTokenSource ArrangeCancellationTokenSource() => new();

	 protected abstract Task InternalActAsync();
}
