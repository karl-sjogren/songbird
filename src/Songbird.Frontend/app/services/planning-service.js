import ServiceBase from 'songbird/services/service-base';
import { reject } from 'rsvp';
import PlanningBoard from 'songbird/models/planning-board';
import PlannedProjectTime from 'songbird/models/planned-project-time';

export default class FikaScheduleService extends ServiceBase {
  getCurrentPlanningBoard() {
    return this.fetch('/api/planning-board')
      .then(response => response.json())
      .then(pojo => new PlanningBoard(pojo));
  }

  getById(id) {
    if(!id) {
      return reject('Empty id supplied.');
    }

    return this.fetch(`/api/planning-board/${id}`)
      .then(response => response.json())
      .then(pojo => new PlanningBoard(pojo));
  }

  getByDate(date) {
    if(!date) {
      return reject('Empty date supplied.');
    }

    return this.fetch(`/api/planning-board/${date}`)
      .then(response => response.json())
      .then(pojo => new PlanningBoard(pojo));
  }

  getEligibleProjects() {
    return this.fetch('/api/planning-board/projects')
      .then(response => response.json());
  }

  createForDate(date) {
    if(!date) {
      return reject('Empty date supplied.');
    }

    return this.fetch(`/api/planning-board/${date}`, {
      method: 'POST'
    })
      .then(response => response.json())
      .then(pojo => new PlanningBoard(pojo));
  }

  setUserProjectTimeAsync(id, userId, projectId, hours) {
    if(!id) {
      return reject('Empty id supplied.');
    }

    if(!userId) {
      return reject('Empty userId supplied.');
    }

    if(!projectId) {
      return reject('Empty projectId supplied.');
    }

    if(typeof hours === 'string') {
      hours = hours.replace(',', '.');
    }

    if(isNaN(hours)) {
      return reject('Invalid hours supplied.');
    }

    return this.fetch(`/api/planning-board/${id}/${userId}/${projectId}/${hours}`, {
      method: 'PUT'
    })
      .then(response => response.json())
      .then(pojo => new PlannedProjectTime(pojo));
  }

  clearUserProjectTimeAsync(id, userId, projectId) {
    if(!id) {
      return reject('Empty id supplied.');
    }

    if(!userId) {
      return reject('Empty userId supplied.');
    }

    if(!projectId) {
      return reject('Empty projectId supplied.');
    }

    return this.fetch(`/api/planning-board/${id}/${userId}/${projectId}`, {
      method: 'DELETE'
    });
  }
}
