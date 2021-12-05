import ServiceBase from 'songbird/services/service-base';
import groupBy from 'songbird/utils/group-by';
import CustomerFilter from 'songbird/models/customer-filter';

export default class ApplicationService extends ServiceBase {
  async getLogFilters() {
    const filters = await this
      .fetch(`/api/application/log-filters`)
      .then(response => response.json());

    const groupedByCustomer = groupBy(filters, 'customerName');

    return Object.keys(groupedByCustomer).map(key => new CustomerFilter(key, groupedByCustomer[key]));
  }
}
