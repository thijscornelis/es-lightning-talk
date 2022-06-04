using MediatR;

namespace ES.Framework.Application.Commands;

/// <inheritdoc />
public abstract class CommandBase<TResponse> : IRequest<TResponse>
where TResponse : CommandResult
{
}
