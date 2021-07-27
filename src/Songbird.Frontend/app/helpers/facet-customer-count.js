import { helper } from '@ember/component/helper';

export default helper(function facetCustomerCount([filter, facets]) {
  if(!filter || !facets) {
    return -1;
  }

  return facets
    .items
    .filter(facet => filter.applications.some(application => facet.value.toLowerCase() === application.toLowerCase()))
    .reduce((acc, current) => acc + current.count, 0);
});
