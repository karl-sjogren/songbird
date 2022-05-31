import Controller from '@ember/controller';
import { tracked } from '@glimmer/tracking';
import { action } from '@ember/object';
import { inject } from '@ember/service';

export default class PlanningBoardViewController extends Controller {
  @inject planningService;

  @tracked model;
  @tracked date;

  @action
  async createBoard() {
    const board = await this.planningService.createForDate(this.date);
    this.model = board;
  }
}
