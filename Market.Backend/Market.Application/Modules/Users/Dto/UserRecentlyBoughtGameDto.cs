using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Dto
{
    public sealed class UserRecentlyBoughtGameDto
    {
        public int GameId { get; set; }

       public string CoverImageURL { get; set; }

        public string Name { get; set; }

    }
}
