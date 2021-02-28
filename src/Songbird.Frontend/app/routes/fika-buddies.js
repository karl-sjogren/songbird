import Route from '@ember/routing/route';
import { inject } from '@ember/service';
import { hash } from 'rsvp';

export default class FikaBuddiesRoute extends Route {
  @inject fikaScheduleService;

  model() {
    return hash({
      model: this.fikaScheduleService.getLatestSchedules(5)
    });
  }

  setupController(controller, models) {
    controller.setProperties(models);
  }
}
