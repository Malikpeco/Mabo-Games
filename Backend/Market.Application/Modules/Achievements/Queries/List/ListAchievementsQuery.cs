using Market.Application.Modules.Achievements.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Queries.List
{
    public sealed class ListAchievementsQuery : BasePagedQuery<ListAchievementsQueryDto>
    {
        public string? Search {  get; set; }
    }
}
