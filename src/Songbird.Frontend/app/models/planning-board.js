import TrackedModelBase from './tracked-model-base';
import UserSchedule from './user-schedule';

export default class PlanningBoard extends TrackedModelBase {
  constructor(source) {
    source.userSchedules = source.userSchedules?.map(x => new UserSchedule(x));

    super(source);
  }
}
