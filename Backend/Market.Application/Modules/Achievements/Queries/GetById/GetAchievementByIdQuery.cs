using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Queries.GetById
{
    public sealed class GetAchievementByIdQuery : IRequest<GetAchievementByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
