import { tracked } from '@glimmer/tracking';

export default class CustomerFilter {
  @tracked name = '';
  @tracked applications = [];
  @tracked count = false;

  constructor(name, applications) {
    this.name = name;
    this.applications = applications;
  }
}
