using System;
using System.Threading;
using System.Threading.Tasks;
using Songbird.Web.DataObjects;
using Songbird.Web.Models;

namespace Songbird.Web.Contracts;

public interface IEditableEntityServiceBase<T> where T : ModelBase {
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<T>> GetPagedAsync(Int32 pageIndex, Int32 pageSize, CancellationToken cancellationToken);
    Task<T> CreateAsync(T model, CancellationToken cancellationToken);
    Task<T> UpdateAsync(T model, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
