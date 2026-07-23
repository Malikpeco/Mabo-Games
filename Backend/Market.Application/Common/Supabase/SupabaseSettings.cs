using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Common.Supabase
{
    public sealed class SupabaseSettings
    {
        public string BaseUrl { get; init; } = default!;

        public  string ServiceRoleSecret { get; init; } =default!;
        public string BucketName { get; init; } = default!;
    }
}
