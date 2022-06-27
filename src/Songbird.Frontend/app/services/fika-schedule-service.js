import ServiceBase from 'songbird/services/service-base';

export default class FikaScheduleService extends ServiceBase {
  getCurrentBuddies() {
    return this.fetch('/api/fika-schedule')
      .then(response => response.json());
  }

  getLatestSchedules(numberOfSchedules = 5) {
    return this.fetch(`/api/fika-schedule/latest/${numberOfSchedules}`)
      .then(response => response.json());
  }
}
