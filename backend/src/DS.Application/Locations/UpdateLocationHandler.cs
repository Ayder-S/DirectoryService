using CSharpFunctionalExtensions;
using DS.Application.Abstractions;
using DS.Application.Commands;
using DS.Application.Commands.Location;
using DS.Application.Database;
using DS.Application.Validation;
using DS.Contracts.Locations.Update;
using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.AppFails;

namespace DS.Application.Locations;

public class UpdateLocationHandler : ICommandHandler<Guid, UpdateLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly IValidator<UpdateLocationRequest> _validator;
    private readonly ILogger<UpdateLocationHandler> _logger;

    public UpdateLocationHandler(
        ILocationsRepository locationsRepository,
        IValidator<UpdateLocationRequest> validator,
        ILogger<UpdateLocationHandler> logger)
    {
        _locationsRepository = locationsRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorsList>> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        var name = Name.Create(command.Request.Name).Value;

        var address = Address.Create(
            command.Request.Address.Country,
            command.Request.Address.Region,
            command.Request.Address.City,
            command.Request.Address.Street,
            command.Request.Address.Building).Value;

        var timezone = Timezone.Create(command.Request.TimeZone).Value;
        
        if (await _locationsRepository.ExistsByNameWithoutId(name, command.Id, cancellationToken))
            return Error.Conflict("location.name.taken", $"Локация с названием '{name.Value}' уже существует").ToErrors();

        var updateResult = await _locationsRepository.Update(command.Id, name, address, timezone, cancellationToken);
        if (updateResult.IsFailure)
            return updateResult.Error.ToErrors();

        return command.Id;
    }
}