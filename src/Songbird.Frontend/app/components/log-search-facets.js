import Component from '@glimmer/component';
import { tracked } from '@glimmer/tracking';
import { set, action } from '@ember/object';
import { inject } from '@ember/service';
import localeComparer from 'songbird/utils/locale-comparer';

export default class LogSearchFacetsComponent extends Component {
  @inject intl;

  @tracked facetContainerOpen = false;

  get sortedFacets() {
    const facets = this.args.facets;
    const facetType = this.args.facetType;

    return facets.sort((a, b) => {
      let aName = this.intl.t(facetType + '.fields.' + a.name);
      let bName = this.intl.t(facetType + '.fields.' + b.name);

      return localeComparer(aName, bName);
    });
  }

  get selectedItems() {
    const facets = this.args.facets;
    const filter = this.args.filter;

    let result = [];

    if(!filter) {
      return result;
    }

    for(let key of Object.getOwnPropertyNames(filter)) {
      let filterValues = filter[key];
      for(let value of filterValues) {
        let facet = facets.find(f => f.name === key);
        if(!facet) {
          continue;
        }
        let type = facet.type;

        let name = value;
        let count = -1;

        if(type === 'terms') {
          let facetItem = facet.items.find(i => i.value === value);
          if(!!facetItem) {
            name = facetItem.name;
            count = facetItem.count;
          }
        }

        result.push({ name: name, count: count, filter: key, value: value, type: type });
      }
    }

    return result;
  }

  @action
  toggleFacet(facet) {
    const facets = this.args.facets;

    facets
      .filter(f => f !== facet)
      .forEach(f => set(f, 'open', false));

    set(facet, 'open', !facet.open);
  }

  @action
  toggleFacetContainer() {
    this.facetContainerOpen = !this.facetContainerOpen;
  }
}
