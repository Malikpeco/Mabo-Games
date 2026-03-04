using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Queries.GetById
{
    public sealed class GetCountryByIdQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IReadOnlyList<string> Cities { get; set; } = new List<string>();
    }
}
