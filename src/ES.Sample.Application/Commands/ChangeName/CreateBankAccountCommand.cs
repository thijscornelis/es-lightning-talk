using ES.Framework.Application.Commands;

namespace ES.Sample.Application.Commands.ChangeName;

/// <inheritdoc />
public class ChangeNameCommand : CommandBase<ChangeNameCommandResult>
{
	 public string Name { get; init; }
}
