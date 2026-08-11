namespace WarehouseERP.Application.Common.Exceptions;

public class DuplicateCodeException : Exception
{
    public DuplicateCodeException(string message) : base(message)
    {
    }
}
