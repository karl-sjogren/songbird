import { tracked } from '@glimmer/tracking';

export default class FilterObject {
  @tracked application = [];
  @tracked level = [];
}
