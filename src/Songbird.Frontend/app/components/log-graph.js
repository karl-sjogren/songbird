import Component from '@glimmer/component';
import { tracked } from '@glimmer/tracking';
import { action } from '@ember/object';
import { guidFor } from '@ember/object/internals';
import { inject } from '@ember/service';
import MG from 'metrics-graphics';
import d3 from 'd3';

export default class LogGraphComponent extends Component {
  @inject logGraphService;
  chart = null;

  @tracked graphId = `log-graph-${guidFor(this)}`;

  get dateRange() {
    return this.args.dateRange || '2021-11-28 - 2021-12-05';
  }

  get errorLevels() {
    return this.args.levels || ['ERROR', 'FATAL', 'Error'];
  }

  get warningLevels() {
    return this.args.levels || ['WARN', 'Warning'];
  }

  get applications() {
    return this.args.applications || [];
  }

  @action
  async renderGraph() {
    let data = await Promise.all([
      this.logGraphService.getLogGraph({dateRange: this.dateRange, level: this.errorLevels, application: this.applications}),
      this.logGraphService.getLogGraph({dateRange: this.dateRange, level: this.warningLevels, application: this.applications})
    ]);

    for(var i = 0; i < data.length; i++) {
      data[i] = MG.convert.date(data[i], 'label', '%Y-%m-%d %H:%M');
    }

    this.chart = MG.data_graphic({
      title: 'Line Chart',
      description: 'This is a simple line chart. You can remove the area portion by adding area: false to the arguments list.',
      data: data,
      full_height: true,
      full_width: true,
      show_tooltips: false,
      target: '#' + this.graphId,
      colors: ['#c41a0e', '#fff3cd'],
      x_accessor: 'label',
      y_accessor: 'count',
      interpolate: d3.curveLinear
    });
  }

  @action
  destroyGraph() {
    if(!!this.chart) {
      this.chart.destroy();
      this.chart = null;
    }
  }
}
