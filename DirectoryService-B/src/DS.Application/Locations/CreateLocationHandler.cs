using CSharpFunctionalExtensions;
using DS.Application.Abstractions;
using DS.Application.Database;
using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

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

    public async Task<Result<Guid>> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
    {
        var nameResult = Name.Create(command.Request.Name);
        if (!nameResult.IsSuccess)
            return Result.Failure<Guid>(nameResult.Error);
        
        var addressResult = Address.Create(
            command.Request.Address.Country,
            command.Request.Address.Region,
            command.Request.Address.City,
            command.Request.Address.Street,
            command.Request.Address.Building);
        if (!addressResult.IsSuccess)
            return Result.Failure<Guid>(addressResult.Error);
        
        var timezoneResult = Timezone.Create(command.Request.Timezone);
        if(!timezoneResult.IsSuccess)
            return Result.Failure<Guid>(timezoneResult.Error);
        
        var location = Location.Create(nameResult.Value, addressResult.Value, timezoneResult.Value);

        await _locationsRepository.Add(location.Value, cancellationToken);
        
        return location.Value.Id;
    }
}