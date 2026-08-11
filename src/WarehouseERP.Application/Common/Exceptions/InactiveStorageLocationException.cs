namespace WarehouseERP.Application.Common.Exceptions;

public class InactiveStorageLocationException : Exception
{
    public InactiveStorageLocationException(string message) : base(message)
    {
    }
}
