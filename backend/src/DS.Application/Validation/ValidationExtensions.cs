using System.Text.Json;
using FluentValidation.Results;
using Microsoft.Extensions.Validation;
using Shared.AppFails;

namespace DS.Application.Validation;

public static class ValidationExtensions
{
    public static ErrorsList ToErrors(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .Select(e => JsonSerializer.Deserialize<Error>(e.ErrorMessage)!)
            .ToList();
        
        return new ErrorsList(errors);
    }
}