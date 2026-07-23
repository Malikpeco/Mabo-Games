using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Supabase.Queries
{
    public sealed class GetGameDownloadUrlQuery:IRequest<string>
    {

        public int GameId { get; set; }


    }
}
