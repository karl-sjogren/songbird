import Route from '@ember/routing/route';
import { inject } from '@ember/service';

export default class ApplicationRoute extends Route {
  @inject intl;

  beforeModel() {
    super.beforeModel(...arguments);

    this.intl.setLocale(['sv-se']);
  }
}
