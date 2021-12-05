'use strict';

const EmberApp = require('ember-cli/lib/broccoli/ember-app');
const fontelloPlugin = require('./tasks/fontello-plugin');
const merge = require('broccoli-merge-trees');

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

  app.import('node_modules/qs/dist/qs.js', {
    using: [
      { transformation: 'amd', as: 'qs' }
    ]
  });

  app.import('node_modules/metrics-graphics/dist/metricsgraphics.css');
  app.import('node_modules/metrics-graphics/dist/metricsgraphics.js', {
    using: [
      { transformation: 'amd', as: 'metrics-graphics' }
    ]
  });

  app.import('node_modules/d3/build/d3.js', {
    using: [
      { transformation: 'amd', as: 'd3' }
    ]
  });

  const fontello = fontelloPlugin('./public/fonts/fontello/', { outputFilePath: './app/styles/fontello-icon-definitions.less' });

  return merge([fontello, app.toTree()]);
};
