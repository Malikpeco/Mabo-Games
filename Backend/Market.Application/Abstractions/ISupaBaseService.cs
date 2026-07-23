using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Abstractions
{
    public interface ISupaBaseService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, CancellationToken ct = default);

        Task<string> GetSignedUrlAsync(string filePath, int expiresInSeconds = 3600, CancellationToken ct = default);

        Task DeleteFileAsync(string filePath, CancellationToken ct = default);


    }
}
