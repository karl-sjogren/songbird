import Component from '@glimmer/component';
import { action } from '@ember/object';

export default class FacetLevelComponent extends Component {
  get levelFacet() {
    const facets = this.args.facets || [];
    return facets.find(x => x.name === 'Level') || { name: 'Level', items: [] };
  }

  get levelFilter() {
    return this.args.filter?.level || [];
  }

  get fatalCount() {
    const levelFacet = this.levelFacet;
    const type = 'fatal';

    return levelFacet
      .items
      .filter(x => x.name.toLowerCase().startsWith(type))
      .reduce((acc, current) => acc + current.count, 0);
  }

  get errorCount() {
    const levelFacet = this.levelFacet;
    const type = 'error';

    return levelFacet
      .items
      .filter(x => x.name.toLowerCase().startsWith(type))
      .reduce((acc, current) => acc + current.count, 0);
  }

  get warningCount() {
    const levelFacet = this.levelFacet;
    const type = 'warn';

    return levelFacet
      .items
      .filter(x => x.name.toLowerCase().startsWith(type))
      .reduce((acc, current) => acc + current.count, 0);
  }

  get fatalSelected() {
    const filter = this.levelFilter;

    return filter.some(x => x.toLowerCase().startsWith('fatal'));
  }

  get errorSelected() {
    const filter = this.levelFilter;

    return filter.some(x => x.toLowerCase().startsWith('error'));
  }

  get warningSelected() {
    const filter = this.levelFilter;

    return filter.some(x => x.toLowerCase().startsWith('warn'));
  }

  @action
  filter(type) {
    const levelFacet = this.levelFacet;
    const filter = this.levelFilter;

    if(filter.length > 0) {
      this.args.removeFilter('level', filter);
      return;
    }

    const filterValues = levelFacet
      .items
      .filter(x => x.name.toLowerCase().startsWith(type.toLowerCase()))
      .map(x => x.value);

    this.args.setFilter('level', filterValues);
  }
}
