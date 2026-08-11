namespace WarehouseERP.Application.Common.Exceptions;

public class InactiveSupplierException : Exception
{
    public InactiveSupplierException(string message) : base(message)
    {
    }
}
