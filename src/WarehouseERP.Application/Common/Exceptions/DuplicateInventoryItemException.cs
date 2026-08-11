namespace WarehouseERP.Application.Common.Exceptions;

public class DuplicateInventoryItemException : Exception
{
    public DuplicateInventoryItemException(string message) : base(message)
    {
    }
}
