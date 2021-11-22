import Component from '@glimmer/component';
import { action } from '@ember/object';

export default class SimpleSelectComponent extends Component {
  @action
  onChange(event) {
    const value = event.target.value;
    const onChange = this.args.onChange;

    if(!!onChange) {
      onChange(value);
    }
  }
}
