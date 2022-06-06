using MediatR;

namespace ES.Framework.Application.Commands;

/// <inheritdoc />
public record class CommandBase<TResponse> : IRequest<TResponse>
where TResponse : CommandResult
{
}
