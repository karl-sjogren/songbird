'use strict';

const EmberApp = require('ember-cli/lib/broccoli/ember-app');
const fontelloToLess = require('./tasks/fontello-to-less');

const isProduction = process.env.EMBER_ENV === 'production';
const disableFingerprinting = process.argv.indexOf('--disable-fingerprinting') !== -1;

module.exports = function(defaults) {
  const app = new EmberApp(defaults, {
    'ember-fetch': {
      preferNative: true
    },
    babel: {
      sourceMaps: isProduction ? undefined : 'inline',
      plugins: [
        '@babel/plugin-proposal-numeric-separator',
        '@babel/plugin-transform-literals',
        '@babel/plugin-proposal-optional-chaining'
      ]
    },
    'ember-cli-babel': {
      includePolyfill: false,
      includeExternalHelpers: true
    },
    fingerprint: {
      enabled: isProduction && !disableFingerprinting,
      extensions: ['js', 'css', 'png', 'jpg', 'gif', 'map', 'svg', 'eot', 'ttf', 'woff', 'woff2']
    }
  });

  fontelloToLess('./app/styles/fontello-icon-definitions.less');

  app.import('node_modules/qs/dist/qs.js', {
    using: [
      { transformation: 'amd', as: 'qs' }
    ]
  });

  return app.toTree();
};
