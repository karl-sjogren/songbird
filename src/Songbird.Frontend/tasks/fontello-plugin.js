/* global require, module */
const Plugin = require('broccoli-plugin');
const fs = require('fs');
const path = require('path');

class FontelloPlugin extends Plugin {
  constructor(inputNodes, options = {}) {
    super([inputNodes], options);

    this.configFile = options.configFile || 'config.json';
    this.outputType = options.outputType || 'less';
    this.outputFilePath = options.outputFilePath || './';
    this.namespace = options.namespace || 'icons';

    this.lastBuiltConfig = null;
  }

  build() {
    if(!this.hasChanged()) {
      return;
    }

    const fontelloConfig = this.readConfigfile();

    let content;
    switch (this.outputType) {
      case 'less':
        content = this.generateLESSMixin(fontelloConfig, this.namespace);
        break;
      case 'scss':
      case 'sass':
        content = this.generateSassMixin(fontelloConfig, this.namespace);
        break;
      default:
        throw new Error(`Unsupported output type: ${this.outputType}`);
    }

    fs.writeFileSync(this.outputFilePath, content);
  }

  hasChanged() {
    const fontelloConfig = this.readConfigfile();

    const hasChanged = JSON.stringify(fontelloConfig) !== this.lastBuiltConfig;

    this.lastBuiltConfig = JSON.stringify(fontelloConfig);

    return hasChanged;
  }

  readConfigfile() {
    const configFilePath = path.join(this.inputPaths[0], this.configFile);
    const configFile = fs.readFileSync(configFilePath, { encoding: 'UTF-8' });
    return JSON.parse(configFile);
  }

  generateLESSMixin(fontelloConfig, namespace = 'icons') {
    const lineBreak = '\r\n';
    let output = `#${namespace} {${lineBreak}`;
    for(let glyph of fontelloConfig.glyphs) {
      output += `  .${glyph.css}() {${lineBreak}`;
      output += `    content: '\\${glyph.code.toString(16)}';${lineBreak}`;
      output += `  }${lineBreak}`;
    }
    output += '}';

    return output;
  }

  generateSassMixin(fontelloConfig, namespace = 'icons') {
    const lineBreak = '\r\n';
    let output = '';
    for(let glyph of fontelloConfig.glyphs) {
      output += `@mixin ${namespace}-${glyph.css}() {${lineBreak}`;
      output += `  content: '\\${glyph.code.toString(16)}';${lineBreak}`;
      output += `}${lineBreak}`;
    }

    return output;
  }
}

module.exports = function fontelloPlugin(...params) {
  return new FontelloPlugin(...params);
};

module.exports.ConcatPlugin = FontelloPlugin;
