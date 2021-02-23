import { helper } from '@ember/component/helper';
import { DateTime } from 'luxon';

export default helper(function formatDate([input, format = 'yyyy-MM-dd']) {
  let date;
  if(input instanceof Date) {
    date = DateTime.fromJSDate(input);
  } else {
    date = DateTime.fromISO(input);
  }

  return date.toFormat(format);
});
