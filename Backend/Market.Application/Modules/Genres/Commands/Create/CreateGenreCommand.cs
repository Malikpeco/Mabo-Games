using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Genres.Commands.Create
{
    public sealed class CreateGenreCommand: IRequest<int>
    {
        public string Name { get; set; }

    }
}
