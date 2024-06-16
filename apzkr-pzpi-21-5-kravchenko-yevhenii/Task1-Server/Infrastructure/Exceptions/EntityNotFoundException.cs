namespace Infrastructure.Exceptions;
public class EntityNotFoundException : ParkyException
{
    public EntityNotFoundException(string message)
        :base(message) { }
}
