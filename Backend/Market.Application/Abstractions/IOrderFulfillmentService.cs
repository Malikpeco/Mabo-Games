using System.Threading;
using System.Threading.Tasks;

namespace Market.Application.Abstractions
{
    public interface IOrderFulfillmentService
    {
        Task<bool> FulfillOrderAsync(int? orderId, string? sessionId, string? paymentIntentId, CancellationToken ct = default);
    }
}
