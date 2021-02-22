import Route from '@ember/routing/route';
import { inject } from '@ember/service';
import { hash } from 'rsvp';

export default class IndexRoute extends Route {
  @inject meService;
  @inject fikaBuddyService;

  model() {
    return hash({
      me: this.meService.me,
      fikaBuddies: this.fikaBuddyService.getCurrentBuddies()
    });
  }

  setupController(controller, models) {
    controller.setProperties(models);
  }
}
