import Component from '@glimmer/component';
import { tracked } from '@glimmer/tracking';
import { action } from '@ember/object';

export default class FacetCustomerComponent extends Component {
  @tracked productionSelected = false;
  @tracked stagingSelected = false;
  @tracked developmentSelected = false;

  get hasEnvironmentFilter() {
    return this.productionSelected || this.stagingSelected || this.developmentSelected;
  }

  get selectedEnvironments() {
    const environments = [];

    if(this.productionSelected) {
      environments.push('Production');
    }

    if(this.stagingSelected) {
      environments.push('Staging');
    }

    if(this.developmentSelected) {
      environments.push('Development');
    }

    return environments;
  }

  get customerFilters() {
    return this.args.applicationFacets || [];
  }

  get applicationFilters() {
    return this.customerFilters.map(x => x.applications).flat();
  }

  get applicationFacet() {
    const facets = this.args.facets || [];
    return facets.find(x => x.name === 'Application') || { name: 'Application', items: [] };
  }

  get applicationFilter() {
    return this.args.filter?.application || [];
  }

  get productionCount() {
    return this.getCountByEnvironment('Production');
  }

  get stagingCount() {
    return this.getCountByEnvironment('Staging');
  }

  get developmentCount() {
    return this.getCountByEnvironment('Development');
  }

  getCountByEnvironment(type) {
    const applicationFacet = this.applicationFacet;
    const applicationFilters = this.applicationFilters;
    const filteredValues = applicationFilters.filter(x => x.environment === type).map(x => x.filterValue);

    return applicationFacet
      .items
      .filter(x => filteredValues.some(value => value === x.value))
      .reduce((acc, current) => acc + current.count, 0);
  }

  get currentCustomerFilter() {
    const customerFilters = this.customerFilters;
    const applicationFilter = this.applicationFilter;

    return customerFilters
      .find(customerFilter => customerFilter.applications
        .some(filter => applicationFilter
          .some(application => application.toLowerCase() === filter.filterValue.toLowerCase())));
  }

  @action
  selectEnvironment(environment) {
    switch (environment) {
      case 'Production':
        this.productionSelected = !this.productionSelected;
        break;
      case 'Staging':
        this.stagingSelected = !this.stagingSelected;
        break;
      case 'Development':
        this.developmentSelected = !this.developmentSelected;
        break;
      default:
        throw new Error(`Unsupported environment: ${environment}`);
    }

    this.filter(this.currentCustomerFilter, true);
  }

  @action
  filter(customerFilter, forceAdd = false) {
    const filter = this.applicationFilter;
    console.log(forceAdd);
    if(filter.length > 0) {
      const currentFilter = this.currentCustomerFilter;

      this.args.removeFilter('application', filter);

      if(currentFilter && !forceAdd) {
        return;
      }
    }

    if(!customerFilter) {
      return;
    }

    let applicationFilters = customerFilter.applications;
    if(this.hasEnvironmentFilter) {
      applicationFilters = applicationFilters.filter(x => this.selectedEnvironments.some(environment => x.environment === environment));
    }

    this.args.setFilter('application', applicationFilters.map(x => x.filterValue));
  }
}
