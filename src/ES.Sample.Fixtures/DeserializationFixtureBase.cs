using ES.Framework.Infrastructure.Json;

namespace ES.Sample.Fixtures;

public abstract class DeserializationFixtureBase<TTarget> : FixtureBase
{
	 /// <summary>Gets the result.</summary>
	 /// <value>The result.</value>
	 public TTarget Result { get; private set; }

	 /// <summary>Gets the source.</summary>
	 /// <value>The source.</value>
	 public string Source { get; private set; }

	 protected virtual TTarget Act(string source) => source.Deserialize<TTarget>();

	 /// <inheritdoc />
	 protected override void Arrange() {
		  base.Arrange();
		  Source = ArrangeSource();
	 }

	 protected abstract string ArrangeSource();

	 /// <inheritdoc />
	 protected override Task InternalActAsync() {
		  Result = Act(Source);
		  return Task.CompletedTask;
	 }
}
