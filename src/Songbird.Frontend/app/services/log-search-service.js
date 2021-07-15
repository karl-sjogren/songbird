import ServiceBase from 'songbird/services/service-base';
import queryHelper from 'qs';
import LogRow from 'songbird/models/log-row';

export default class LogSearchService extends ServiceBase {
  getResults(query, filter, settings) {
    let queryString = queryHelper.stringify({
      ...query,
      ...filter,
      ...settings
    });

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
      ...filter,
      ...settings
    });

    return this.fetch(`/api/log-search/facets?${queryString}`)
      .then(response => response.json());
  }
}
