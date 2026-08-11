namespace WarehouseERP.Application.Common.Exceptions;

public class InactiveWarehouseException : Exception
{
    public InactiveWarehouseException(string message) : base(message)
    {
    }
}
