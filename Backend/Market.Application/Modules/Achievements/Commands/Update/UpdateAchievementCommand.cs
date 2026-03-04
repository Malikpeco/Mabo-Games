using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Commands.Update
{
    public sealed class UpdateAchievementCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImageURL { get; set; }
    }
}
