using ES.Framework.Domain.TypedIdentifiers;
using System.Diagnostics;

namespace ES.Framework.Domain;

public record SnapshotId : TypedIdentifier<SnapshotId, Guid>
{
	 /// <inheritdoc />
	 public SnapshotId(Guid value) : base(value) { }
}

public static class StopwatchHelper
{
	 public static void WrapWithTimer(string name, Action action) {
		  var stopwatch = new Stopwatch();
		  stopwatch.Start();
		  action();
		  stopwatch.Stop();
		  Console.WriteLine("{Action} took {Millis} milliseconds", name, stopwatch.ElapsedMilliseconds);
	 }

	 public static async Task WrapWithTimerAsync(string name, Func<Task> action) {
		  var stopwatch = new Stopwatch();
		  stopwatch.Start();
		  await action();
		  stopwatch.Stop();
		  Console.WriteLine("{Action} took {Millis} milliseconds", name, stopwatch.ElapsedMilliseconds);
	 }
}
