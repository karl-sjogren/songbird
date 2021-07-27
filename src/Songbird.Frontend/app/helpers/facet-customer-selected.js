import { helper } from '@ember/component/helper';

export default helper(function facetCustomerSelected([customerFilter, currentFilters]) {
  if(!customerFilter || !currentFilters) {
    return false;
  }

  return currentFilters
    .some(filter => customerFilter.applications.some(application => filter.toLowerCase() === application.toLowerCase()));
});
