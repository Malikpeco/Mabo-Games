using Market.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Market.Application.Modules.Carts.Commands.Create
{
    public sealed class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IAppCurrentUser _currentUser;

        public AddToCartCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            
            await EnsureNoPendingOrder(_context, userId.Value, cancellationToken);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci=>ci.Game)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
            {
                throw new Exception("Cart not found for this user. (It should exist by default.)");
            }

            var game = await _context.Games
                .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);

            if (game == null)
            {
                throw new Exception("Game not found.");
            }

            var alreadyOwned = await _context.UserGames.AnyAsync(ug => ug.UserId == userId.Value && ug.GameId == request.GameId);

            if (alreadyOwned) 
            {
                throw new ValidationException("You already own this game!");
            }


            var existingItem = cart.CartItems
                .FirstOrDefault(i => i.GameId == request.GameId);

            if (existingItem != null)
            {
                return Unit.Value;
            }

            var cartItem = new CartItemEntity
            {
                CartId = cart.Id,
                GameId = request.GameId
            };
            _context.CartItems.Add(cartItem);

            cart.TotalPrice += game.Price;
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private static async Task EnsureNoPendingOrder(IAppDbContext context, int userid, CancellationToken ct)
        {
            var hasPending = await context.Orders
                .AnyAsync(o => o.UserId == userid && o.OrderStatus == "Pending", ct);

            if (hasPending)
                throw new ValidationException("Checkout in progress. Cart is locked.");
        }

    }

}
