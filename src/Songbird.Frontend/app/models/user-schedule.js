import TrackedModelBase from './tracked-model-base';
import PlannedProjectTime from './planned-project-time';
import ScheduledOfficeRole from './scheduled-office-role';
import ScheduledStatus from './scheduled-status';

export default class UserSchedule extends TrackedModelBase {
  constructor(source) {
    source.projects = source.projects?.map(x => new PlannedProjectTime(x));
    source.roles = source.roles?.map(x => new ScheduledOfficeRole(x));
    source.statuses = source.statuses?.map(x => new ScheduledStatus(x));

    super(source);
  }
}
