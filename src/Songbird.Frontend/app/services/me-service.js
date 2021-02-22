import ServiceBase from 'songbird/services/service-base';
import { tracked } from '@glimmer/tracking';

export default class MeService extends ServiceBase {
  @tracked me = null;

  get userId() {
    return this.me?.userId;
  }

  get name() {
    return this.me?.name;
  }

  get email() {
    return this.me?.email;
  }

  initialize() {
    return this.fetch(`/api/me`)
      .then(response => response.json())
      .then(me => {
        this.me = me;
      })
      .catch(() => {
        this.me = { offline: true };
      });
  }
}
