import Route from '@ember/routing/route';
import { inject } from '@ember/service';
import { hash } from 'rsvp';
import { Settings } from 'luxon';

export default class ApplicationRoute extends Route {
  @inject intl;
  @inject meService;

  beforeModel() {
    super.beforeModel(...arguments);

    this.intl.setLocale(['sv-se']);
    Settings.defaultLocale = 'sv';
  }

  model() {
    return hash({
      me: this.meService.initialize(),
      slowdown: new Promise(resolve => {
        window.setTimeout(() => resolve(), 2000);
      })
    })
      .then(models => {
        const warmupLoader = document.querySelector('.warmup-loader');
        warmupLoader?.remove();

        return models;
      });
  }
}
