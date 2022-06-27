import TrackedModelBase from './tracked-model-base';
import Project from './project';

export default class PlannedProjectTime extends TrackedModelBase {
  constructor(source) {
    if(!!source) {
      source.projects = new Project(source.project);
    }

    super(source);

    this.ensureProperty('time', () => 0);
    this.ensureProperty('projectId', () => '');
    this.ensureProperty('project', () => null);
  }
}
