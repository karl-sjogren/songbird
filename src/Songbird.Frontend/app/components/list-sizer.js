import Component from '@glimmer/component';
import { action } from '@ember/object';

export default class ListSizerComponent extends Component {
  @action
  sizeChanged(value) {
    let parsedValue = parseInt(value, 10);
    if(isNaN(parsedValue)) {
      parsedValue = this.availableSizes[0].value;
    }

    this.args.changePageSize(parsedValue);
  }

  get availableSizes() {
    return [
      {
        label: '100',
        value: 100
      },
      {
        label: '500',
        value: 500
      },
      {
        label: '1 000',
        value: 1_000
      },
      {
        label: '10 000',
        value: 10_000
      }
    ];
  }
}
