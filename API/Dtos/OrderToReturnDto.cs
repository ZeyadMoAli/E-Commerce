using Core.OrderAggregate;

namespace API.Dtos;

public class OrderToReturnDto
{
    public int Id { get; set; }
    public string BuyerEmail { get; set; }
    public DateTimeOffset OrderDate { get; set; } 
    public Address ShipToAddress { get; set; }
    public string DeliveryMethod { get; set; }
    public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
    public double Subtotal { get; set; }
    public double ShippingPrice { get; set; }
    public double Total { get; set; }
    public string Status { get; set; }

}