import Component from '@glimmer/component';
import { tracked } from '@glimmer/tracking';
import { action } from '@ember/object';
import { guidFor } from '@ember/object/internals';

export default class PlanningBoardProjectComponent extends Component {
  @tracked projectInputId = `project-input-${guidFor(this)}`;
  @tracked timeInputId = `time-input-${guidFor(this)}`;

  @tracked recentlyUpdated = false;

  get disableSelect() {
    const model = this.args.model;
    if(!!model?.id) {
      return true;
    }

    return this.recentlyUpdated;
  }

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
    if(!!projectTime) {
      model.id = projectTime.id;
      model.projectId = projectTime.projectId;
      this.recentlyUpdated = true;
    }
  }
}
