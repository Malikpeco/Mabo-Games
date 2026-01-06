using Market.Domain.Common.Attributes;
using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reviews.Commands.Create
{
    public sealed class CreateReviewCommand : IRequest<Unit>
    {
        [PreserveString]
        public string? Content { get; set; }
        public float Rating { get; set; }
        public int UserGameId { get; set; }
    }
}
