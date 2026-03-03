using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Common.IGDB
{
    public sealed class IGDBSettings
    {
        public string ClientID { get; init; } = default!;

        public string ClientSecret { get; init; } = default!;

        public string BaseUrl { get; init; } = default!;

        public string TokenUrl { get; init; } = default!;

    }
}
