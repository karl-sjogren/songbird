import Controller from '@ember/controller';

export default class IndexController extends Controller {
  get currentFikaBuddies() {
    const currentUserId = this.me.userId;
    const buddies = this.fikaBuddies;

    const myMatch = buddies.matches.find(match => match.users.some(user => user.id === currentUserId));
    if(!myMatch) {
      return 'Inga fika-kompisar denna vecka';
    }

    return myMatch.users.filter(x => x.id !== currentUserId);
  }
}
