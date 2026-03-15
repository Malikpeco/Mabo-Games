using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Abstractions
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileName, CancellationToken ct = default);
        Task<List<string>> UploadImagesAsync(List<Stream> imageStreams, List<string> fileNames, CancellationToken ct = default);

    }
}
