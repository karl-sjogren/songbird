import Route from '@ember/routing/route';
import { inject } from '@ember/service';
import { hash } from 'rsvp';

export default class IndexRoute extends Route {
  @inject meService;
  @inject fikaScheduleService;

  model() {
    return hash({
      me: this.meService.me,
      fikaBuddies: this.fikaScheduleService.getCurrentBuddies()
    });
  }

  setupController(controller, models) {
    controller.setProperties(models);
  }
}
