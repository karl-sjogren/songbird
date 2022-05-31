import Component from '@glimmer/component';
import { tracked } from '@glimmer/tracking';
import { set, action } from '@ember/object';
import { next } from '@ember/runloop';
import { inject } from '@ember/service';
import queryHelper from 'qs';
import FilterObject from 'songbird/models/filter-object';
import tidyShallowObject from 'songbird/utils/tidy-shallow-object';
import { getCurrentQueryString } from 'songbird/utils/query-string';

const VK_RETURN = 13;

export default class LogSearchComponent extends Component {
  @inject intl;
  @inject('logSearchService') service;

  @tracked loading = false;
  @tracked results = [];
  @tracked facets = [];
  @tracked totalCount = 0;
  @tracked showAllColumns = false;
  @tracked excludedColumns = false;

  @tracked query = {};
  @tracked filter = new FilterObject();
  @tracked settings = {
    pageIndex: 0,
    pageSize: 100,
    sortField: '@timestamp'
  };

  constructor() {
    super(...arguments);

    this.parseQueryString();

    this.fetch();
  }

  get queryFields() {
    return ['query'];
  }

  get filterFields() {
    return ['application', 'level', 'dateRange'];
  }

  parseQueryString() {
    let queryString = getCurrentQueryString();

    for(let key of this.queryFields) {
      if(!!queryString[key]) {
        set(this, 'query.' + key, queryString[key]);
      }
    }

    for(let key of this.filterFields) {
      if(!!queryString[key]) {
        set(this, 'filter.' + key, queryString[key]);
      }
    }

    if(!!queryString.pageIndex) {
      set(this, 'settings.pageIndex', parseInt(queryString.pageIndex));
    }

    if(!!queryString.pageSize) {
      set(this, 'settings.pageSize', parseInt(queryString.pageSize));
    }

    if(!!queryString.sortField) {
      set(this, 'settings.sortField', queryString.sortField);
    }

    if(!!queryString.sortAscending) {
      set(this, 'settings.sortAscending', queryString.sortAscending === 'true');
    }
  }

  updateQueryString() {
    let query = {
      ...this.query,
      ...this.settings
    };

    // Can't use spred operator on classes so we do this manually
    for(let key of Object.getOwnPropertyNames(FilterObject.prototype)) {
      const value = this.filter[key];
      if(Array.isArray(value)) {
        query[key] = value;
      }
    }

    tidyShallowObject(query);

    let queryString = queryHelper.stringify(query);
    window.history.replaceState(queryString, document.title, `?${queryString}`);
  }

  enableFullWidth() {
    const mainWrapper = document.querySelector('.main-wrapper');
    mainWrapper.classList.add('full-width');
  }

  disableFullWidth() {
    const mainWrapper = document.querySelector('.main-wrapper');
    mainWrapper.classList.remove('full-width');
  }

  fetch() {
    if(this.loading) {
      return;
    }

    next(this, () => {
      this.loading = true;
      this.updateQueryString();

      let results = this.service
        .getResults(this.query, this.filter, this.settings)
        .then(result => {
          this.results.clear();
          this.results.pushObjects(result.items);
          this.totalCount = result.total;
        });

      let facets = this.service
        .getFacets(this.query, this.filter, this.settings)
        .then(facets => {
          this.facets.clear();
          this.facets.pushObjects(facets);
        });

      Promise.all([results, facets])
        .then(() => {
          this.loading = false;
        });
    });
  }

  @action
  onQueryKeyDown(e) {
    if(e.keyCode === VK_RETURN) {
      this.fetchFromStart();
    }
  }

  @action
  fetchFromStart() {
    set(this, 'settings.pageIndex', 0);
    this.fetch();
  }

  @action
  setFilter(filter, value) {
    if(!this.filter[filter]) {
      this.filter[filter] = [];
    }

    if(Array.isArray(value)) {
      value.forEach(x => {
        if(this.filter[filter].indexOf(x) >= 0) {
          return;
        }

        this.filter[filter].pushObject(x);
      });
    } else {
      if(this.filter[filter].indexOf(value) >= 0) {
        return;
      }

      this.filter[filter].pushObject(value);
    }

    set(this, 'settings.pageIndex', 0);
    //this.filter = this.filter;
    this.fetch();
  }

  @action
  removeFilter(filter, value) {
    if(!this.filter[filter]) {
      throw new Error(`Filter ${filter} does not exist on FilterObject.`);
    }

    if(Array.isArray(value)) {
      [...value].forEach(x => {
        const index = this.filter[filter].indexOf(x);
        if(index >= 0) {
          this.filter[filter].removeAt(index);
        }
      });
    } else {
      if(this.filter[filter].indexOf(value) === -1) {
        return;
      }

      this.filter[filter].removeAt(this.filter[filter].indexOf(value));
    }

    set(this, 'settings.pageIndex', 0);
    this.fetch();
  }

  @action
  gotoPageIndex(index) {
    set(this, 'settings.pageIndex', index);
    this.fetch();
  }

  @action
  changePageSize(pageSize) {
    set(this, 'settings.pageIndex', 0);
    set(this, 'settings.pageSize', pageSize);
    this.fetch();
  }

  @action
  sort(newField) {
    const sortField = this.settings.sortField;
    const sortAscending = this.settings.sortAscending;
    if(sortField === newField) {
      if(!sortAscending) {
        set(this, 'settings.sortAscending', true);
      } else {
        set(this, 'settings.sortField', null);
        set(this, 'settings.sortAscending', false);
      }
    } else {
      set(this, 'settings.sortField', newField);
      set(this, 'settings.sortAscending', false);
    }

    set(this, 'settings.pageIndex', 0);
    this.fetch();
  }
}
