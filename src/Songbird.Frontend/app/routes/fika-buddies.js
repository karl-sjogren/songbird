import Route from '@ember/routing/route';
import { inject } from '@ember/service';
import { hash } from 'rsvp';

export default class FikaBuddiesRoute extends Route {
  @inject fikaBuddyService;

  model() {
    return hash({
      model: this.fikaBuddyService.getCurrentBuddies()
    });
  }

  setupController(controller, models) {
    controller.setProperties(models);
  }
}
