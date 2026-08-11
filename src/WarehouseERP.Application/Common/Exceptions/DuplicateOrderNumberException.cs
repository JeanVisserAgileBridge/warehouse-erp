namespace WarehouseERP.Application.Common.Exceptions;

public class DuplicateOrderNumberException : Exception
{
    public DuplicateOrderNumberException(string message) : base(message)
    {
    }
}
