using WarehouseERP.Shared.Contracts.Customers;
using ApplicationCustomerDto = WarehouseERP.Application.Sales.Customers.CustomerDto;

namespace WarehouseERP.Api.Contracts.Customers;

internal static class CustomerDtoExtensions
{
    public static CustomerDto ToContract(this ApplicationCustomerDto dto)
    {
        return new CustomerDto
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            IsActive = dto.IsActive
        };
    }

    public static IReadOnlyList<CustomerDto> ToContract(this IReadOnlyList<ApplicationCustomerDto> dtos)
    {
        return dtos.Select(ToContract).ToList();
    }
}
