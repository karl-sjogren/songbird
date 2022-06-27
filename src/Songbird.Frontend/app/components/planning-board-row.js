import Component from '@glimmer/component';

export default class PlanningBoardRowComponent extends Component {
  get unusedProjects() {
    const model = this.args.model;
    const projects = this.args.projects;

    return projects.filter(project => !model.projects.some(x => x.projectId === project.id));
  }
}
