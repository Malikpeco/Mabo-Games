using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Supabase.Commands
{

    
    public sealed class UploadGameFileCommand:IRequest<string>
    {
        public IFormFile File { get; set; }
    }
}
