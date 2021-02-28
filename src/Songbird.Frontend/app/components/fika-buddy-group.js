import Component from '@glimmer/component';
import { inject } from '@ember/service';

export default class FikaBuddyGroupComponent extends Component {
  @inject meService;

  get highlight() {
    const users = this.args.model.users;
    const currentUserId = this.meService.userId;

    return users.some(x => x.id === currentUserId);
  }
}
