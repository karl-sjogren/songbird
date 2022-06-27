import EmberRouter from '@ember/routing/router';
import config from 'songbird/config/environment';

export default class Router extends EmberRouter {
  location = config.locationType;
  rootURL = config.rootURL;
}

Router.map(function() {
  this.route('fika-buddies');
  this.route('log-search');
  this.route('log-graphs');

  this.route('planning-board', function() {
    this.route('view', { path: '/:date' });
  });
});
