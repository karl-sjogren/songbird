import ServiceBase from 'songbird/services/service-base';
import queryHelper from 'qs';
import FilterObject from 'songbird/models/filter-object';
import LogRow from 'songbird/models/log-row';
import groupBy from 'songbird/utils/group-by';
import CustomerFilter from 'songbird/models/customer-filter';

export default class LogSearchService extends ServiceBase {
  getResults(query, filter, settings) {
    let queryString = queryHelper.stringify({
      ...query,
      ...settings
    });

    // Can't use spred operator on classes so we do this manually
    for(let key of Object.getOwnPropertyNames(FilterObject.prototype)) {
      const value = filter[key];
      if(Array.isArray(value)) {
        query[key] = value;
      }
    }

    return this.fetch(`/api/log-search?${queryString}`)
      .then(response => response.json())
      .then(results => {
        const items = results.items;

        const rows = items.map(x => new LogRow(x));

        return {
          ...results,
          items: rows
        };
      });
  }

  getFacets(query, filter, settings) {
    let queryString = queryHelper.stringify({
      ...query,
      ...settings
    });

    // Can't use spred operator on classes so we do this manually
    for(let key of Object.getOwnPropertyNames(FilterObject.prototype)) {
      const value = filter[key];
      if(Array.isArray(value)) {
        query[key] = value;
      }
    }

    return this.fetch(`/api/log-search/facets?${queryString}`)
      .then(response => response.json());
  }

  async getApplicationFacets() {
    const filters = await this
      .fetch(`/api/log-search/facets/application`)
      .then(response => response.json());

    const groupedByCustomer = groupBy(filters, 'customerName');

    return Object.keys(groupedByCustomer).map(key => new CustomerFilter(key, groupedByCustomer[key]));
  }
}
