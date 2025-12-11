//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Market.Application.Modules.Carts.Queries
//{
//    public sealed class GetCartByUserIdQueryHandler (IAppDbContext context, IAppCurrentUser currentUser) 
//        : IRequestHandler<GetCartByUserIdQuery, Unit>
//    {

//        public async Task<Unit> Handle(GetCartByUserIdQuery request, CancellationToken ct)
//        {
//            var c = await context.Carts
//                .Where(x => x.UserId == currentUser.UserId)
//                .FirstOrDefaultAsync(ct);

//            return c;

//        }
//    }
//}
