import { helper } from '@ember/component/helper';

export default helper(function typeofHelper([input]) {
  return typeof(input);
});
