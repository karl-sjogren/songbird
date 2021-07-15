import Component from '@glimmer/component';
import { action } from '@ember/object';

export default class ListSizerComponent extends Component {
  @action
  sizeChanged(e) {
    let selectedValue = parseInt(e.target.value, 10);
    if(isNaN(selectedValue)) {
      selectedValue = this.availableSizes[0].value;
    }

    this.args.changePageSize(selectedValue);
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
