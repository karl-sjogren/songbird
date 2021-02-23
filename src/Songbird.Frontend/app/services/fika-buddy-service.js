import ServiceBase from 'songbird/services/service-base';

export default class FikaBuddyService extends ServiceBase {
  getCurrentBuddies() {
    return this.fetch('/api/fika-schedule')
      .then(response => response.json());
  }

  getLatestSchedules(numberOfSchedules = 5) {
    return this.fetch('/api/fika-schedule/list/' + numberOfSchedules)
      .then(response => response.json());
  }
}
