using CSharpFunctionalExtensions;
using Shared.AppFails;

namespace DS.Application.Abstractions;

public interface ICommand;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<UnitResult<ErrorsList>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<TResponse, in TCommand>
    where TCommand : ICommand
{
    Task<Result<TResponse, ErrorsList>> Handle(TCommand command, CancellationToken cancellationToken);
}
