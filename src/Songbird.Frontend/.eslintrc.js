'use strict';

module.exports = {
  root: true,
  parser: 'babel-eslint',
  parserOptions: {
    ecmaVersion: 2018,
    sourceType: 'module',
    ecmaFeatures: {
      legacyDecorators: true,
    },
  },
  plugins: ['ember'],
  extends: [
    'eslint:recommended',
    'plugin:ember/recommended'
  ],
  env: {
    browser: true,
  },
  rules: {
    'no-console': 'off',
    'no-extra-boolean-cast': 'off',
    'no-trailing-spaces': 'error',
    'no-multi-spaces': 'error',
    'space-infix-ops': 'error',
    'brace-style': ['error', '1tbs', { 'allowSingleLine': true }],
    'space-before-function-paren': ['error', 'never'],
    'space-in-parens': ['error', 'never'],
    'keyword-spacing' : ['error', {
      'after': true,
      'overrides': {
        'if': { 'after': false },
        'for': { 'after': false },
        'catch': { 'after': false },
        'while': { 'after': false }
      }
    }],
    'arrow-spacing': ['error', { 'before': true, 'after': true }],
    'eqeqeq': 'error',
    'semi': 'error',
    'indent': ['error', 2, { 'SwitchCase': 1 }],
    'quotes': ['error', 'single', { 'allowTemplateLiterals': true }],
    'ember/no-new-mixins': 'off',
    'no-restricted-globals': [
      'error',
      {
        'name': 'fetch',
        'message': 'Use the service-base method instead or import fetch from "fetch".'
      }
    ]
  },
  overrides: [
    // node files
    {
      files: [
        '.eslintrc.js',
        '.prettierrc.js',
        '.template-lintrc.js',
        'ember-cli-build.js',
        'testem.js',
        'blueprints/*/index.js',
        'config/**/*.js',
        'lib/*/index.js',
        'server/**/*.js',
      ],
      parserOptions: {
        sourceType: 'script',
      },
      env: {
        browser: false,
        node: true,
      },
      plugins: ['node'],
      extends: ['plugin:node/recommended'],
      rules: {
        // this can be removed once the following is fixed
        // https://github.com/mysticatea/eslint-plugin-node/issues/77
        'node/no-unpublished-require': 'off',
      },
    },
  ],
};
