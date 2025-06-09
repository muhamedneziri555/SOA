namespace EventMngt.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<string> errors) 
        : base(string.Join(", ", errors))
    {
    }
} 