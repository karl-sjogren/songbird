import queryHelper from 'qs';

export let getCleanedLocationSearch = function() {
  let search = window.location.search;
  if(!search) {
    return '';
  }

  if(search[0] === '?') {
    search = search.substr(1);
  }

  return search.trim();
};

export let getCurrentQueryString = function() {
  let locationSearch = getCleanedLocationSearch();
  let queryString = queryHelper.parse(locationSearch);
  return queryString;
};