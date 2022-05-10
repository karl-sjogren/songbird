using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Services;

public class BinaryFileService : IBinaryFileService {
    private readonly SongbirdContext _songbirdContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IContentTypeProvider _contentTypeProvider;
    private readonly ILogger<BinaryFileService> _logger;

    public BinaryFileService(SongbirdContext songbirdContext, IDateTimeProvider dateTimeProvider, IContentTypeProvider contentTypeProvider, ILogger<BinaryFileService> logger) {
        _songbirdContext = songbirdContext;
        _dateTimeProvider = dateTimeProvider;
        _contentTypeProvider = contentTypeProvider;
        _logger = logger;
    }

    public async Task<BinaryFile> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
        return await _songbirdContext
            .BinaryFiles
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<BinaryFile> StoreAsync(IFormFile formFile, CancellationToken cancellationToken) {
        var fileName = formFile.FileName;
        _contentTypeProvider.TryGetContentType(fileName, out var contentType);

        using var stream = formFile.OpenReadStream();
        using var ms = new MemoryStream((Int32)stream.Length);

        await stream.CopyToAsync(ms, cancellationToken);

        var buffer = ms.ToArray();

        var file = new BinaryFile() {
            Name = formFile.FileName,
            ContentType = contentType,
            Content = buffer,
            Checksum = CalculateChecksum(buffer)
        };

        file.CreatedAt = file.UpdatedAt = _dateTimeProvider.Now;

        _songbirdContext.BinaryFiles.Add(file);
        await _songbirdContext.SaveChangesAsync(cancellationToken);

        return file;
    }

    private static string CalculateChecksum(byte[] buffer) {
        using var md5Hash = MD5.Create();
        var hash = md5Hash.ComputeHash(buffer);
        return Convert.ToBase64String(hash);
    }
}
