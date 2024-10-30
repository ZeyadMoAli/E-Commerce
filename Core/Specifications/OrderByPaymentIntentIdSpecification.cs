using System.Linq.Expressions;
using Core.OrderAggregate;

namespace Core.Specifications;

public class OrderByPaymentIntentIdSpecification:BaseSpecification<Order>
{
    public OrderByPaymentIntentIdSpecification(string paymentIntentId) : base(o=>o.PaymentIntentId == paymentIntentId)
    {
    }
}