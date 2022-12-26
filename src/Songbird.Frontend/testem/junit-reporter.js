/* global require, process, module */

// Adapted from https://github.com/mainmatter/testem-gitlab-reporter/blob/f5773f11d9c633f3c7f9708e598a52e8f7f2643c/index.js
'use strict';

const builder = require('xmlbuilder');

class JUnitReporter {
  constructor(silent, out) {
    this.out = out || process.stdout;
    this.silent = silent;

    this.results = [];
  }

  report(launcher, data) {
    this.results.push({ launcher, data });
  }

  finish() {
    if(this.silent) {
      return;
    }

    this.out.write(this._formatOutput());
  }

  _formatOutput() {
    const suites = builder.create('testsuites');
    const suite = suites.element('testsuite');

    suite.attribute('name', 'Test Suite');
    console.log(JSON.stringify(this.results, null, 2));

    suite.attribute('tests', this.results.filter(it => !it.data.skipped).length);
    suite.attribute('failures', this.results.filter(it => it.data.failed).length);
    suite.attribute('skipped', this.results.filter(it => it.data.skipped).length);

    for(let { data } of this.results) {
      if(data.skipped) {
        continue;
      }

      const el = suite.element('testcase');
      el.attribute('name', data.name);
      el.attribute('classname', data.name);
      el.attribute('time', durationFromMs(data.runDuration));

      if(data.error) {
        const failure = el.element('failure');
        failure.text(data.error.message);
      }
    }

    return suite.end({ pretty: true });
  }
}

function durationFromMs(ms) {
  if(ms) {
    return (ms / 1000).toFixed(3);
  } else {
    return 0;
  }
}

module.exports = JUnitReporter;
