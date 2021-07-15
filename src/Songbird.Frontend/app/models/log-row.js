import { tracked } from '@glimmer/tracking';

export default class LogRow {
  @tracked timestamp;
  @tracked level;
  @tracked message;
  @tracked application;
  @tracked context;

  constructor(source) {
    this.timestamp = source['@timestamp'];
    this.level = source.level; // This could possibly be cleaned up to not differentiante WARN and Warning etc
    this.message = source.message;
    this.application = source['log4net:UserName'];
    this.context = source.loggerName;
  }
}
