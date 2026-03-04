using Market.Application.Modules.Carts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Queries
{
    public sealed class GetCartByUserIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<GetCartByUserIdQuery, CartDto>
    {

        public async Task<CartDto> Handle(GetCartByUserIdQuery request, CancellationToken ct)
        {
            var cart = await context.Carts.AsNoTracking()
                .Where(c => c.UserId == currentUser.UserId)
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    TotalPrice = c.TotalPrice,
                    CartItems = c.CartItems
                        .Select(ci => new CartItemDto 
                        {
                            Id = ci.Id,
                            GameId = ci.GameId,
                            GameName = ci.Game.Name,
                            CoverImageURL = ci.Game.CoverImageURL!,
                            Price = ci.Game.Price,
                            AddedAt = ci.AddedAt,
                            IsSaved = ci.IsSaved
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);

            return cart ?? new CartDto();

        }
    }
}
