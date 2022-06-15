using ES.Framework.Application.Commands;

namespace ES.Sample.Application.Commands.ChangeName
{
	 namespace V1
	 {
		  /// <inheritdoc />
		  public record ChangeNameCommandResult : CommandResult
		  {
				/// <summary>Gets the name.</summary>
				/// <value>The name.</value>
				public string Name { get; init; }
		  }
	 }
}
