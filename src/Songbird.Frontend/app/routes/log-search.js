import Route from '@ember/routing/route';
import { inject } from '@ember/service';
import { hash } from 'rsvp';

export default class LogSearchRoute extends Route {
  @inject logSearchService;

  model() {
    return hash({
      applicationFacets: this.logSearchService.getApplicationFacets()
    });
  }

  setupController(controller, models) {
    controller.setProperties(models);
  }
}
