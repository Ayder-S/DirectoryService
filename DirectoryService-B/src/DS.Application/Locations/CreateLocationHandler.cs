using CSharpFunctionalExtensions;
using DS.Application.Abstractions;
using DS.Application.Database;
using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.AppFails;

namespace DS.Application.Locations;

public class CreateLocationHandler : ICommandHandler<Guid, CreateLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly ILogger<CreateLocationHandler> _logger;
    
    public CreateLocationHandler(
        ILocationsRepository locationsRepository,
        ILogger<CreateLocationHandler> logger)
    {
        _locationsRepository = locationsRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorsList>> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
    {
        var nameResult = Name.Create(command.Request.Name);
        if(nameResult.IsFailure)
            return nameResult.Error.ToErrors();
        
        var addressResult = Address.Create(
            command.Request.Address.Country,
            command.Request.Address.Region,
            command.Request.Address.City,
            command.Request.Address.Street,
            command.Request.Address.Building);
        if(addressResult.IsFailure)
            return addressResult.Error.ToErrors();
        
        var timezoneResult = Timezone.Create(command.Request.Timezone);
        if(timezoneResult.IsFailure)
            return timezoneResult.Error.ToErrors();
        
        var location = Location.Create(nameResult.Value, addressResult.Value, timezoneResult.Value);
        if(location.IsFailure)
            return location.Error.ToErrors();

        var addResult = await _locationsRepository.Add(location.Value, cancellationToken);
        if (addResult.IsFailure)
            return addResult.Error.ToErrors();

        return location.Value.Id;

    }
}