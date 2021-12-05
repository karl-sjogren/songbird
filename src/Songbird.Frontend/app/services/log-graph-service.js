import ServiceBase from 'songbird/services/service-base';
import queryHelper from 'qs';

export default class LogGraphService extends ServiceBase {
  async getLogGraph(filter) {
    let queryString = queryHelper.stringify({
      ...filter
    });

    return await this
      .fetch(`/api/log-graph?${queryString}`)
      .then(response => response.json());
  }
}
