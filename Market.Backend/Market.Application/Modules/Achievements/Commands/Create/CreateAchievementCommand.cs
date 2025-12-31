using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Commands.Create
{
    public sealed class CreateAchievementCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImageURL { get; set; }
    }
}
