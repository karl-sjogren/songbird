import Component from '@glimmer/component';
import { tracked } from '@glimmer/tracking';
import { action } from '@ember/object';
import { inject } from '@ember/service';

export default class PlanningBoardComponent extends Component {
  @inject planningService;

  @tracked plannyFile;

  constructor() {
    super(...arguments);

    const value = Math.random();
    const headerLogo = document.getElementById('header-logo');

    if(value > 0.3) {
      if(!!headerLogo) {
        headerLogo.style.clipPath = '';
      }
      this.plannyFile = 'planny-the-cat';
    } else {
      if(!!headerLogo) {
        headerLogo.style.clipPath = 'inset(0 45% 0 0)';
      }
      this.plannyFile = 'planny-the-cat-feather';
    }
  }

  enableFullWidth() {
    const mainWrapper = document.querySelector('.main-wrapper');
    mainWrapper.classList.add('full-width');
  }

  disableFullWidth() {
    const mainWrapper = document.querySelector('.main-wrapper');
    mainWrapper.classList.remove('full-width');
  }

  @action
  async addProject(userSchedule) {
    userSchedule.projects.pushObject({ scheduleId: userSchedule.id, time: 0, projectId: '' });
  }

  @action
  async setProjectTime(userSchedule, project) {
    if(project.time === '') {
      return await this.planningService.clearUserProjectTimeAsync(userSchedule.planningBoardId, userSchedule.userId, project.projectId);
    } else {
      return await this.planningService.setUserProjectTimeAsync(userSchedule.planningBoardId, userSchedule.userId, project.projectId, project.time);
    }
  }
}
