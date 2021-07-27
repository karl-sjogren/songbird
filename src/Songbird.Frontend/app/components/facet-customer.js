import Component from '@glimmer/component';
import { action } from '@ember/object';
import CustomerFilter from 'songbird/models/customer-filter';

export default class FacetCustomerComponent extends Component {
  customerFilters = [
    // TODO Move this to the database instead of hardcoding it
    new CustomerFilter('AcadeMedia', [
      'IIS APPPOOL\\Vam',
      'ACA-IT-Prod-VUX-VAM2-PROD',
      'ACA-IT-Prod-VUX-VAM2-STAGE',
      'ACA-IT-Prod-VUX-VAM2-DEV',
      'ACA-IT-Prod-VUX-PRODRAPP-PROD',
      'ACA-IT-Prod-VUX-PRODRAPP-STAGE',
      'ACA-IT-Prod-VUX-PRODRAPP-DEV',
      'ACA-IT-Prod-VUX-KURSSLUSSEN-PROD',
      'ACA-IT-Prod-VUX-KURSSLUSSEN-DEV'
    ]),
    new CustomerFilter('SkandiaMäklarna', [
      'IIS APPPOOL\\6676ee60-a120-4083-a7d9-0d07ca397da5',
      'IIS APPPOOL\\4315a266-5a20-4081-b718-273d91c4f5a4',
      'IIS APPPOOL\\87b17b45-1ced-4bd3-9121-27fffd309adc',
      'skandiaip-prod-api',
      'skandiaip-stage-api',
      'skandiaip-dev-api',
      'skandiaip-prod-functions',
      'skandiaip-stage-functions',
      'skandiaip-dev-functions'
    ]),
    new CustomerFilter('IKSU', [
      'IIS APPPOOL\\iksu-production-public-web',
      'IIS APPPOOL\\iksu-test-public-web',
      'IIS APPPOOL\\iksu_imagevault',
    ]),
    new CustomerFilter('Erik Olsson', [
      'ErikOlsson CDN V2 - Production',
      'ErikOlsson CDN V2 - Development',
      'IIS APPPOOL\\www.erikolsson.se',
      'NT AUTHORITY\\NETWORK SERVICE',
      'IIS APPPOOL\\test.eofastighet.se',
      'IIS APPPOOL\\restapi.eofastighet.se',
      'IIS APPPOOL\\test-restapi.eofastighet.se',
    ])
  ];

  get applicationFacet() {
    const facets = this.args.facets || [];
    return facets.find(x => x.name === 'Application') || { name: 'Application', items: [] };
  }

  get applicationFilter() {
    return this.args.filter?.application || [];
  }

  @action
  filter(customerFilter) {
    const filter = this.applicationFilter;

    if(filter.length > 0) {
      const currentFilter = filter.some(filter => customerFilter.applications.some(application => filter.toLowerCase() === application.toLowerCase()));

      this.args.removeFilter('application', filter);

      if(currentFilter) {
        return;
      }
    }

    this.args.setFilter('application', customerFilter.applications);
  }
}
