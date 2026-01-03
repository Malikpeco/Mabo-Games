using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reviews.Commands.Create
{
    public sealed class CreateReviewCommandHandler(IAppCurrentUser currentUser, IAppDbContext context)
        : IRequestHandler<CreateReviewCommand, Unit>
    {
        public async Task<Unit> Handle(CreateReviewCommand request, CancellationToken ct)
        {
            var userGame = await context.UserGames.FirstOrDefaultAsync(ug => ug.Id == request.UserGameId && ug.UserId == currentUser.UserId.Value, ct);

            if (userGame is null)
            {
                throw new MarketNotFoundException("UserGame not found");
            }

            if (await context.Reviews.AnyAsync(r => r.UserGameId == userGame.Id))
                throw new MarketBusinessRuleException("422", "User already has a review for this game.");

            var review = new ReviewEntity
            {
                Content = request.Content,
                Rating = request.Rating,
                UserGameId = userGame.Id,
            };
            context.Reviews.Add(review);

            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
