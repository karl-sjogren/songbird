/* global require, module */
const fs = require('fs');
const config = require('../public/fonts/fontello/config');

// This method is used to read the fontello config and output
// the icons as LESS mixins
const fontelloToLess = function fontelloToLess(outputPath) {
  const lineBreak = '\r\n';
  let output = '#icons {' + lineBreak;
  for(let glyph of config.glyphs) {
    output += `  .${glyph.css}() {` + lineBreak;
    output += `    content: '\\${glyph.code.toString(16)}';` + lineBreak;
    output += `  }` + lineBreak;
  }
  output += '}' + lineBreak;

  fs.writeFileSync(outputPath, output);
};

module.exports = fontelloToLess;
