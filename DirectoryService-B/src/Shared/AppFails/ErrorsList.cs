using System.Collections;

namespace Shared.AppFails;

public class ErrorsList : IEnumerable<Error>
{
    private readonly List<Error> _errors;

    public ErrorsList(IEnumerable<Error> errors)
    {
        _errors = [..errors];
    }

    public int Count => _errors.Count;
    
    public bool HasErrors => _errors.Count > 0;

    public static implicit operator ErrorsList(Error[] errors) => new ErrorsList(errors);

    public static implicit operator ErrorsList(Error error) => new ErrorsList([error]);
    
    public IEnumerator<Error> GetEnumerator() => _errors.GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}