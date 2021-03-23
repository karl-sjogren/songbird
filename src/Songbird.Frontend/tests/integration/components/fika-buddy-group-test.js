import { module, test } from 'qunit';
import { setupRenderingTest } from 'ember-qunit';
import { render } from '@ember/test-helpers';
import { hbs } from 'ember-cli-htmlbars';

module('Integration | Component | fika-buddy-group', function(hooks) {
  setupRenderingTest(hooks);

  test('it renders', async function(assert) {
    this.set('match', new { users: [] });
    await render(hbs`<FikaBuddyGroup @model={{this.match}}/>`);

    assert.ok(true);
  });
});
