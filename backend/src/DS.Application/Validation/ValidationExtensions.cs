using FluentValidation.Results;
using Shared.Kernel.AppFails;

namespace DS.Application.Validation;

public static class ValidationExtensions
{
    public static ErrorsList ToErrors(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .Select(e => (Error)e.CustomState!)
            .ToList();
        
        return new ErrorsList(errors);
    }
}