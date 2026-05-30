using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Supabase.Queries
{
    public sealed class GetGameDownloadUrlQueryValidator:AbstractValidator<GetGameDownloadUrlQuery>
    {
        public GetGameDownloadUrlQueryValidator()
        {

            RuleFor(x => x.GameId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Game id cannot be empty!");
           

        }


    }
}
