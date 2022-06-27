import Component from '@glimmer/component';
import { action } from '@ember/object';

export default class SimpleSelectComponent extends Component {
  @action
  onChange(event) {
    const select = event.target;
    const option = select.options[select.selectedIndex];

    const value = select.value;
    const valueType = option.dataset.valueType || 'string';
    const onChange = this.args.onChange;

    let correctValue = value.toString();
    if(valueType === 'number') {
      correctValue = parseInt(value, 10);
    } else if(valueType === 'boolean') {
      correctValue = value === 'true';
    }

    if(!!onChange) {
      onChange(correctValue);
    }
  }
}
