namespace Infrastructure.Exceptions;
public class ParkyException : Exception
{
    public ParkyException(string? message = null)
        : base(message) { }
}
