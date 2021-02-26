using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.Services {
    public class CustomerService : ICustomerService {
        private readonly SongbirdContext _songbirdContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(SongbirdContext songbirdContext, IDateTimeProvider dateTimeProvider, ILogger<CustomerService> logger) {
            _songbirdContext = songbirdContext;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
            return await _songbirdContext
                .Customers
                .Include(x => x.Projects)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        public async Task<Customer> CreateAsync(Customer model, CancellationToken cancellationToken) {
            if(model.Id != Guid.Empty) {
                return null;
            }

            model.CreatedAt = model.UpdatedAt = _dateTimeProvider.Now;

            _songbirdContext.Add(model);
            await _songbirdContext.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task<Customer> UpdateAsync(Customer model, CancellationToken cancellationToken) {
            var existingItem = await GetByIdAsync(model.Id, cancellationToken);
            if(existingItem == null) {
                return null;
            }

            existingItem.Name = model.Name;

            if(_songbirdContext.Entry(existingItem).State == EntityState.Modified) {
                existingItem.UpdatedAt = _dateTimeProvider.Now;
            }

            _songbirdContext.Add(model);
            await _songbirdContext.SaveChangesAsync(cancellationToken);

            return model;
        }
    }
}
