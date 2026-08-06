namespace WarehouseERP.Application.Common.Exceptions;

public class InactiveCategoryException : Exception
{
    public InactiveCategoryException(string message) : base(message)
    {
    }
}
