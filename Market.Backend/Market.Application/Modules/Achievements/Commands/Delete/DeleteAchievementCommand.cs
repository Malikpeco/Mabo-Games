using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Commands.Delete
{
    public sealed class DeleteAchievementCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
