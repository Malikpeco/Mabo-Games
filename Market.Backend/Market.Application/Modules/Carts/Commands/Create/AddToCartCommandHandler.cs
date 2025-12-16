using Market.Application.Modules.RegisterUser.Dto;
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
    }

}
