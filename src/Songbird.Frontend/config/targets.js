'use strict';

const browsers = [
  'last 1 Chrome versions',
  'last 1 Firefox versions',
  //'last 1 Safari versions', // NOPE!
];

const isCI = Boolean(process.env.CI);
const isProduction = process.env.EMBER_ENV === 'production';

if(isCI || isProduction) {
  // Lets ignore IE11 since this is an internal tool
  //browsers.push('ie 11');
}

module.exports = {
  browsers,
};
