import Route from '@ember/routing/route';
import { inject } from '@ember/service';
import { hash } from 'rsvp';

export default class LogSearchRoute extends Route {
  @inject applicationService;

  model() {
    return hash({
      applicationFacets: this.applicationService.getLogFilters()
    });
  }

  setupController(controller, models) {
    controller.setProperties(models);
  }
}
