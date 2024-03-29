const fs = require('fs');
const MultiReporter = require('testem-multi-reporter');
const JUnitReporter = require('./testem/junit-reporter');
// eslint-disable-next-line node/no-extraneous-require
const TAPReporter = require('testem/lib/reporters/tap_reporter');

let reporter = new MultiReporter({
  reporters: [
    {
      ReporterClass: TAPReporter,
      args: [false, null, { get: () => false }],
    },
    {
      ReporterClass: JUnitReporter,
      args: [false, fs.createWriteStream('junit.xml'), { get: () => false }],
    },
  ],
});

module.exports = {
  test_page: 'tests/index.html?hidepassed',
  disable_watching: true,
  reporter: reporter,
  launch_in_ci: [
    'Chrome'
  ],
  launch_in_dev: [
    'Chrome'
  ],
  browser_start_timeout: 120,
  browser_args: {
    Chrome: {
      ci: [
        // --no-sandbox is needed when running Chrome inside a container
        process.env.CI ? '--no-sandbox' : null,
        '--headless',
        '--disable-dev-shm-usage',
        '--disable-software-rasterizer',
        '--mute-audio',
        '--remote-debugging-port=0',
        '--window-size=1440,900'
      ].filter(Boolean)
    }
  }
};
