import { helper } from '@ember/component/helper';
import { guidFor as _guidFor } from '@ember/object/internals';
import { htmlSafe } from '@ember/template';

export default helper(function roughSeedStyle([input]) {
  return htmlSafe('--rough-seed:' + _guidFor(input));
});
