using Core.Entities;
using Core.Interfaces;
using Core.OrderAggregate;
using Core.Specifications;

namespace Infrastructure.Services;

public class OrderService: IOrderService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string baskedId, Address shippingAddress)
    {
        var basket = await _basketRepository.GetBasketAsync(baskedId);
        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }
        
        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        var subtotal = items.Sum(x => x.Price * x.Quantity);
        var order = new Order( buyerEmail, shippingAddress, deliveryMethod, items , subtotal);
        _unitOfWork.Repository<Order>().Add(order);
        var result = await _unitOfWork.Complete();
        if(result < 0)
            return null;

        await _basketRepository.DeleteBasketAsync(baskedId);
        return order;
        
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrderWithItemsAndOrderingSpecification(buyerEmail);
        return await _unitOfWork.Repository<Order>().ListAsync(spec);
    }

    public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        var spec = new OrderWithItemsAndOrderingSpecification(id,buyerEmail);
        return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }
}