import Component from '@glimmer/component';
import { tracked } from '@glimmer/tracking';
import { action } from '@ember/object';
import { guidFor } from '@ember/object/internals';
import PlannedProjectTime from 'songbird/models/planned-project-time';

export default class PlanningBoardProjectComponent extends Component {
  @tracked projectInputId = `project-input-${guidFor(this)}`;
  @tracked timeInputId = `time-input-${guidFor(this)}`;

  @action
  async timeChanged() {
    await this.updateProjectTime();
  }

  @action
  async projectChanged(value) {
    const model = this.args.model;
    model.projectId = value;

    await this.updateProjectTime();
  }

  async updateProjectTime() {
    const model = this.args.model;
    const schedule = this.args.schedule;

    const projectTime = await this.args.setProjectTime(schedule, model);
    if(!!projectTime.id) {
      model.id = projectTime.id;
      model.projectId = projectTime.projectId;
      model.project = new PlannedProjectTime(projectTime.project);
    } else {
      schedule.projects.removeObject(model);
    }
  }
}
