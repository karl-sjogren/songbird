import { module, test } from 'qunit';
import { setupRenderingTest } from 'ember-qunit';
import { render } from '@ember/test-helpers';
import { hbs } from 'ember-cli-htmlbars';

module('Integration | Helper | luxon-format', function(hooks) {
  setupRenderingTest(hooks);

  test('it renders', async function(assert) {
    this.set('inputValue', '2021-02-23');

    await render(hbs`{{luxon-format this.inputValue "W"}}`);

    assert.strictEqual(this.element.textContent.trim(), '8');
  });
});
