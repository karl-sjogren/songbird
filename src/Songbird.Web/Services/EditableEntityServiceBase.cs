using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.DataObjects;
using Songbird.Web.Models;

namespace Songbird.Web.Services;

public abstract class EditableEntityServiceBase<T> : IEditableEntityServiceBase<T> where T : ModelBase {
    private readonly SongbirdContext _songbirdContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger _logger;

    protected EditableEntityServiceBase(SongbirdContext songbirdContext, IDateTimeProvider dateTimeProvider, ILogger logger) {
        _songbirdContext = songbirdContext;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    protected abstract IQueryable<T> GetPreparedQuery();
    protected abstract void MapChangesToModel(T existingItem, T model);

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
        return await GetPreparedQuery()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<PagedResult<T>> GetPagedAsync(Int32 pageIndex, Int32 pageSize, CancellationToken cancellationToken) {
        var items = await GetPreparedQuery()
            .AsNoTracking()
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await GetPreparedQuery()
            .CountAsync(cancellationToken);

        return new PagedResult<T>(items, pageIndex, pageSize, totalCount);
    }

    public async Task<T> CreateAsync(T model, CancellationToken cancellationToken) {
        if(model.Id != Guid.Empty) {
            return null;
        }

        model.CreatedAt = model.UpdatedAt = _dateTimeProvider.Now;

        _songbirdContext.Add(model);
        await _songbirdContext.SaveChangesAsync(cancellationToken);

        return model;
    }

    public async Task<T> UpdateAsync(T model, CancellationToken cancellationToken) {
        var existingItem = await GetByIdAsync(model.Id, cancellationToken);
        if(existingItem == null) {
            return null;
        }

        MapChangesToModel(existingItem, model);

        if(_songbirdContext.Entry(existingItem).State == EntityState.Modified) {
            existingItem.UpdatedAt = _dateTimeProvider.Now;
        }

        await _songbirdContext.SaveChangesAsync(cancellationToken);

        return existingItem;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken) {
        var existingItem = await GetByIdAsync(id, cancellationToken);
        if(existingItem == null) {
            return;
        }

        existingItem.IsDeleted = true;
        existingItem.DeletedAt = _dateTimeProvider.Now;

        await _songbirdContext.SaveChangesAsync(cancellationToken);
    }
}
