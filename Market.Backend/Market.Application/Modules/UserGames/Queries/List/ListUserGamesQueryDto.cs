using Market.Application.Modules.Games.Dto;
using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.UserGames.Queries.List
{
    public class ListUserGamesQueryDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public int GameId { get; init; }
        public StorefrontGameDto Game { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
