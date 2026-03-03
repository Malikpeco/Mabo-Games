using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Genres.Commands.Create
{
    public sealed class CreateGenreCommandHanlder(IAppCurrentUser currentUser, IAppDbContext context) 
        : IRequestHandler<CreateGenreCommand, int>
    {
        public async Task<int> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAdmin)
                throw new MarketForbiddenException();


            bool exists = await context.Genres.AnyAsync(x => x.Name == request.Name);

            if (exists)
                throw new MarketConflictException($"Genres already contain: {request.Name}!");


            var genre = new GenreEntity
            {
                Name = request.Name
            };

            context.Genres.Add(genre);

            await context.SaveChangesAsync(cancellationToken);

            return genre.Id;



        }
    }
}
