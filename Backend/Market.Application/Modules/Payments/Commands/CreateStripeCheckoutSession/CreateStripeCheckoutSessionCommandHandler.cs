using Market.Application.Modules.Payments.Dto;
using Market.Domain.Entities;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Application.Modules.Payments.Commands.CreateStripeCheckoutSession
{
    public sealed class CreateStripeCheckoutSessionCommandHandler(IAppDbContext context, IAppCurrentUser currentUser, IOptions<StripeOptions> stripeOptions)
        : IRequestHandler<CreateStripeCheckoutSessionCommand, CreateStripeCheckoutSessionResponse>
    {
        public async Task<CreateStripeCheckoutSessionResponse> Handle(CreateStripeCheckoutSessionCommand request, CancellationToken ct)
        {

            //get user and orders
            int userId = currentUser.UserId!.Value;

            var order = await context.Orders
                .Include(o=>o.OrderItems)
                .ThenInclude(oi=>oi.Game)
                .Include(o=>o.Payment)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == userId, ct);


            //validation
            if (order == null)
                throw new ValidationException("Order not found.");

            if (order.OrderStatus != "Pending")
                throw new ValidationException("Only a Pending order can be paid.");

            if (order.OrderItems is null)
                throw new ValidationException("Order has no items.");



            if(order.Payment is null)
            {
                order.Payment = new PaymentEntity 
                {
                    OrderId=order.Id,
                    Total= order.TotalAmount
                };
                context.Payments.Add(order.Payment);
            }


            //BUILD STRIPE LINE ITEMS FROM ORDER ITEMS 
            var lineItems = new List<SessionLineItemOptions>();

            foreach(var item in order.OrderItems) 
            {
                long unitAmount = (long)Math.Round(item.Price * 100m);

                lineItems.Add(new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur",
                        UnitAmount = unitAmount,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Game?.Name ?? $"Game #{item.GameId}"
                        }

                    }
                });
            }


            //stripe checkout session options
            var sessionCreate = new SessionCreateOptions
            {
                Mode = "payment",


                //urls stripe redirects the browser to
                SuccessUrl = stripeOptions.Value.SuccessUrl,
                CancelUrl = stripeOptions.Value.CancelUrl,


                //LINKS STRIPE BACK TO THE ORDER
                ClientReferenceId = order.Id.ToString(),



                Metadata = new Dictionary<string, string>
                {
                    ["orderId"] = order.Id.ToString(),
                    ["userId"] = userId.ToString()
                },

                LineItems = lineItems
            };




            //CALLS STRIPE API TO CREATE THE SESSION
            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(
                sessionCreate, cancellationToken: ct);


            //store the stripe id's in payment, will be used by the webhook
            order.Payment.StripeCheckoutSessionId = session.Id;
            order.Payment.StripePaymentIntentId = session.PaymentIntentId;
            order.Payment.Total = order.TotalAmount;

            await context.SaveChangesAsync(ct);


            var expiresAtUtc = session.ExpiresAt.ToUniversalTime();


            //frontend receives this and redirects user to checkoutUrl
            return new CreateStripeCheckoutSessionResponse(
                order.Id,
                session.Id,
                session.Url,
                expiresAtUtc
            );
            
        }
    }
}
