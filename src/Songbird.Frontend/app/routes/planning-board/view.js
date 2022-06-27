import Route from '@ember/routing/route';
import { inject } from '@ember/service';
import { hash } from 'rsvp';

export default class ViewRoute extends Route {
  @inject planningService;

  model(params) {
    let promise;
    if(params.date) {
      promise = this.planningService.getByDate(params.date).catch(() => null);
    } else {
      promise = this.planningService.getCurrentPlanningBoard().catch(() => null);
    }

    return hash({
      model: promise,
      projects: this.planningService.getEligibleProjects(),
      date: params.date
    });
  }

  setupController(controller, models) {
    controller.setProperties(models);
  }
}
