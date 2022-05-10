using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Songbird.Web.Models;

namespace Songbird.Web.Contracts;

public interface IBinaryFileService {
    Task<BinaryFile> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<BinaryFile> StoreAsync(IFormFile formFile, CancellationToken cancellationToken);
}
