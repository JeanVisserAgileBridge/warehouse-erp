namespace WarehouseERP.Application.Common.Exceptions;

public class DuplicateSkuException : Exception
{
    public DuplicateSkuException(string message) : base(message)
    {
    }
}
