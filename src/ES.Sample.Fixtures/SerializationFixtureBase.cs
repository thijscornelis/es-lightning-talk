using ES.Framework.Infrastructure.Json;

namespace ES.Sample.Fixtures;

public abstract class SerializationFixtureBase<TSource> : FixtureBase
{
	 public TSource Source { get; private set; }
	 public string Result { get; private set; }

	 /// <inheritdoc />
	 protected override Task InternalActAsync() {
		  Result = Act(Source);
		  return Task.CompletedTask;
	 }

	 /// <inheritdoc />
	 protected override void Arrange() {
		  base.Arrange();
		  Source = ArrangeSource();
	 }

	 protected abstract TSource ArrangeSource();
	 protected virtual string Act(TSource source) => source.Serialize();
}