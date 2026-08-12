namespace WarehouseERP.Application.Common.Exceptions;

public class InactiveCustomerException : Exception
{
    public InactiveCustomerException(string message) : base(message)
    {
    }
}
