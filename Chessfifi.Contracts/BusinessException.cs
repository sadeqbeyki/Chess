namespace Chessfifi.Contracts;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}
