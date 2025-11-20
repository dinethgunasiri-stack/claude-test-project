using MediatR;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<Result<int>>
{
    public string ShippingFirstName { get; init; } = string.Empty;
    public string ShippingLastName { get; init; } = string.Empty;
    public string ShippingAddressLine1 { get; init; } = string.Empty;
    public string? ShippingAddressLine2 { get; init; }
    public string ShippingCity { get; init; } = string.Empty;
    public string ShippingState { get; init; } = string.Empty;
    public string ShippingZipCode { get; init; } = string.Empty;
    public string ShippingCountry { get; init; } = string.Empty;
    public string ShippingPhone { get; init; } = string.Empty;
    public string? CustomerNotes { get; init; }
}
