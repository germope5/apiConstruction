namespace apiConstruction.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key) 
        : base($"{name} con id '{key}' no fue encontrado.")
    {
    }
}
