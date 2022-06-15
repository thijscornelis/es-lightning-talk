using ES.Framework.Application.Commands;

namespace ES.Sample.Application.Commands.Create
{
	 namespace V1
	 {
		  /// <inheritdoc />
		  public record CreateCommand : CommandBase<CreateCommandResult>
		  {
				/// <summary>Gets the name.</summary>
				/// <value>The name.</value>
				public string Name { get; init; }

				/// <summary>Gets the withdraw limit.</summary>
				/// <value>The withdraw limit.</value>
				public decimal WithdrawLimit { get; init; }
		  }
	 }
}
