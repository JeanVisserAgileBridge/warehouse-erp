namespace WarehouseERP.Application.Common.Exceptions;

public class InactiveProductException : Exception
{
    public InactiveProductException(string message) : base(message)
    {
    }
}
