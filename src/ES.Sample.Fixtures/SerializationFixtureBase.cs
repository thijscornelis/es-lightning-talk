using ES.Framework.Infrastructure.Json;

namespace ES.Sample.Fixtures;

public abstract class SerializationFixtureBase<TSource> : FixtureBase
{
	 /// <summary>Gets the result.</summary>
	 /// <value>The result.</value>
	 public string Result { get; private set; }

	 /// <summary>Gets the source.</summary>
	 /// <value>The source.</value>
	 public TSource Source { get; private set; }

	 protected virtual string Act(TSource source) => source.Serialize();

	 /// <inheritdoc />
	 protected override void Arrange() {
		  base.Arrange();
		  Source = ArrangeSource();
	 }

	 protected abstract TSource ArrangeSource();

	 /// <inheritdoc />
	 protected override Task InternalActAsync() {
		  Result = Act(Source);
		  return Task.CompletedTask;
	 }
}
