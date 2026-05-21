using System.ComponentModel.DataAnnotations;

namespace DS.Contracts.Locations.Get;

public record GetLocationsRequest(
    [Range(1, int.MaxValue)] int Page = 1,
    [Range(1, 100)] int PageSize = 20,
    string? Name = null); // если клиент будет передавать Name как фильтр для поиска