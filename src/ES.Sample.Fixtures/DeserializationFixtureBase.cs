using ES.Framework.Infrastructure.Json;

namespace ES.Sample.Fixtures;

public abstract class DeserializationFixtureBase<TTarget> : FixtureBase
{
	public string Source { get; private set; }
	public TTarget Result { get; private set; }

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

	protected virtual TTarget Act(string source) => source.Deserialize<TTarget>();
}