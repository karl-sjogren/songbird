const { buildEmberPlugins } = require('ember-cli-babel');
const isProduction = process.env.EMBER_ENV === 'production';

// For now this is duplicated in ember-cli-build.js due to
// missing support from ember-auto-import
module.exports = function(api) {
  api.cache(true);

  return {
    sourceMaps: isProduction ? undefined : 'inline',
    presets: [
      [
        require.resolve('@babel/preset-env'),
        {
          targets: require('./config/targets'),
        },
      ],
    ],
    plugins: [
      '@babel/plugin-proposal-numeric-separator',
      '@babel/plugin-transform-literals',
      '@babel/plugin-proposal-optional-chaining',
      ...buildEmberPlugins(__dirname, { }),
    ],
  };
};
