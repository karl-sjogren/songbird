using System;
using System.Threading;
using System.Threading.Tasks;
using Songbird.Web.Models;

namespace Songbird.Web.Contracts {
    public interface ICustomerService {
        Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Customer> CreateAsync(Customer model, CancellationToken cancellationToken);
        Task<Customer> UpdateAsync(Customer model, CancellationToken cancellationToken);
    }
}
