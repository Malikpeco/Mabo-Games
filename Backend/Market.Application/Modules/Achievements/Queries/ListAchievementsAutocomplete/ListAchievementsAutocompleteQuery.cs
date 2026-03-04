using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Queries.ListAchievementsAutocomplete
{
    public sealed class ListAchievementsAutocompleteQuery : IRequest<List<ListAchievementsAutocompleteQueryDto>>
    {
        public string Term { get; set; }
    }
}
