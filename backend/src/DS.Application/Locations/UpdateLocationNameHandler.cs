using CSharpFunctionalExtensions;
using DS.Application.Abstractions;
using DS.Application.Commands;
using DS.Application.Commands.Location;
using DS.Application.Database;
using DS.Application.Validation;
using DS.Contracts.Locations.Update;
using DS.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.AppFails;

namespace DS.Application.Locations;

public class UpdateLocationNameHandler : ICommandHandler<Guid, UpdateLocationNameCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly IValidator<UpdateLocationNameRequest> _validator;
    private readonly ILogger<UpdateLocationNameHandler> _logger;

    public UpdateLocationNameHandler(
        ILocationsRepository locationsRepository,
        IValidator<UpdateLocationNameRequest> validator,
        ILogger<UpdateLocationNameHandler> logger)
    {
        _locationsRepository = locationsRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorsList>> Handle(UpdateLocationNameCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrors();
        
        var name = Name.Create(command.Request.Name).Value;
        
        if (await _locationsRepository.ExistsByNameWithoutId(name, command.Id, cancellationToken))
            return Error.Conflict("location.name.taken", $"Локация с названием '{name.Value}' уже существует").ToErrors();

        var updateResult = await _locationsRepository.UpdateName(command.Id, name, cancellationToken);
        if (updateResult.IsFailure)
            return updateResult.Error.ToErrors();

        return command.Id;
    }

}