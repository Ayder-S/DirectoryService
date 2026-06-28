using CSharpFunctionalExtensions;
using DS.Application.Commands.Location;
using DS.Application.Interfaces.Abstractions;
using DS.Application.Interfaces.Database;
using DS.Application.Validation;
using DS.Contracts.Locations.Create;
using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Kernel.AppFails;

namespace DS.Application.Handlers.Locations;

public class CreateLocationHandler : ICommandHandler<Guid, CreateLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly ILogger<CreateLocationHandler> _logger;
    private readonly IValidator<CreateLocationRequest> _validator;

    public CreateLocationHandler(
        ILocationsRepository locationsRepository,
        ILogger<CreateLocationHandler> logger,
        IValidator<CreateLocationRequest> validator)
    {
        _locationsRepository = locationsRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorsList>> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
    {
        if (command.Request is null)
            return Error.Validation("request.is.required", "Тело запроса обязательно").ToErrors();
        
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrors();
        
        var nameResult = Name.Create(command.Request.Name).Value;
        
        if (await _locationsRepository.ExistsByName(nameResult, cancellationToken))
        {
            return Error.Conflict(
                "location.name.taken", $"Локация с названием '{nameResult.Value}' уже существует").ToErrors();
        }
        
        var addressResult = Address.Create(
            command.Request.Address.Country,
            command.Request.Address.Region,
            command.Request.Address.City,
            command.Request.Address.Street,
            command.Request.Address.Building).Value;
        
        var timezoneResult = Timezone.Create(command.Request.TimeZone).Value;

        var location = Location.Create(nameResult, addressResult, timezoneResult);
        if(location.IsFailure)
            return location.Error.ToErrors();

        var addResult = await _locationsRepository.Add(location.Value, cancellationToken);
        if (addResult.IsFailure)
            return addResult.Error.ToErrors();
        
        _logger.LogInformation("Локация {LocationId} успешно создана с названием {LocationName}", location.Value.Id, nameResult.Value);

        return location.Value.Id;
    }
}