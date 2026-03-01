using Market.Application.Modules.Games.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.UserGames.Queries.List
{
    public sealed class ListUserGamesQuery : BasePagedQuery<ListUserGamesQueryDto>
    {
        //takes userId from IAppCurrentUser currentUser(handler) - (OBAVEZNO/NEMA LISTE SVIH USERGAMES NEGO SAMO PO USERU)
    }
}
