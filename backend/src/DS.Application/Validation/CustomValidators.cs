using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Shared.Kernel.AppFails;

namespace DS.Application.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Error> result = factoryMethod.Invoke(value);
            
            if (result.IsSuccess)
                return;
            
            context.AddFailure(new ValidationFailure(context.PropertyPath, result.Error.Message)
            {
                CustomState = result.Error,
            });
        });
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        return rule.WithState(_ => error).WithMessage(error.Message);
    }
}