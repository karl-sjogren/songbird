import { helper } from '@ember/component/helper';

export default helper(function eq(params/*, hash*/) {
  if(params.length !== 2) {
    console.warn('Invalid number or arguments passed to eq helper. Defaulting to false.');
    return false;
  }

  return params[0] === params[1];
});
