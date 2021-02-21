import ServiceBase from 'songbird/services/service-base';

export default class FikaBuddyService extends ServiceBase {
  getCurrentBuddies() {
    return this.fetch('/api/fika-schedule')
      .then(response => response.json());
  }
}
