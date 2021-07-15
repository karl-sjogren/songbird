import Component from '@glimmer/component';
import { action } from '@ember/object';
import queryHelper from 'qs';
import { getCurrentQueryString } from 'songbird/utils/query-string';

export default class ListPagerComponent extends Component {
  get pageIndex() {
    if(typeof this.args.pageIndex === 'number') {
      return this.args.pageIndex;
    }

    return 0;
  }

  get pageSize() {
    if(typeof this.args.pageSize === 'number') {
      return this.args.pageSize;
    }

    return 20;
  }

  get totalCount() {
    if(typeof this.args.totalCount === 'number') {
      return this.args.totalCount;
    }

    return 0;
  }

  get shouldUpdateQuery() {
    if(typeof this.args.shouldUpdateQuery === 'boolean') {
      return this.args.shouldUpdateQuery;
    }

    return true;
  }

  get quicklyAccessibleNumbersPerDirection() {
    if(typeof this.args.quicklyAccessibleNumbersPerDirection === 'number') {
      return this.args.quicklyAccessibleNumbersPerDirection;
    }

    return 3;
  }

  get pages() {
    let pages = Math.ceil(this.totalCount / this.pageSize);

    if(pages === 0 || isNaN(pages) || pages === Infinity) {
      return [];
    }

    return new Array(pages)
      .fill()
      .map((_, pageIndex) => ({
        pageIndex: pageIndex,
        pageNumber: pageIndex + 1,
        current: parseInt(pageIndex) === parseInt(this.pageIndex)
      }));
  }

  get limitedPages() {
    const pageIndex = parseInt(this.pageIndex);
    const quicklyAccessibleNumbersPerDirection = parseInt(this.quicklyAccessibleNumbersPerDirection);

    let lowerIndexBound = pageIndex - quicklyAccessibleNumbersPerDirection;
    if(lowerIndexBound < 0) {
      lowerIndexBound === 0;
    }

    const lastPageIndex = parseInt(this.lastPageIndex);
    let upperIndexBound = pageIndex + quicklyAccessibleNumbersPerDirection;
    if(upperIndexBound > lastPageIndex) {
      upperIndexBound = lastPageIndex;
    }

    const pages = this.pages;
    return pages.filter(page => page.pageIndex >= lowerIndexBound && page.pageIndex <= upperIndexBound);
  }

  get lastPageIndex() {
    const pages = this.pages;

    if(pages.length === 0) {
      return 0;
    }

    const maxPageIndex = pages[pages.length - 1].pageIndex;
    return maxPageIndex;
  }

  get canGoNext() {
    const pageIndex = this.pageIndex;
    const pages = this.pages;

    if(pages.length === 0) {
      return false;
    }

    const maxPageIndex = this.lastPageIndex;
    return pageIndex < maxPageIndex;
  }

  get canGoBack() {
    return this.pageIndex > 0;
  }

  updateQueryValue(pageIndex) {
    const query = getCurrentQueryString();
    query.pageIndex = pageIndex;

    const queryString = queryHelper.stringify(query);
    window.history.replaceState(this.pageIndex, document.title, `?${queryString}`);
  }

  @action
  nextPage() {
    if(!this.canGoNext) {
      return;
    }

    this.args.gotoPageIndex(this.pageIndex + 1);
    this.updateQueryValue(this.pageIndex + 1);
  }

  @action
  previousPage() {
    if(!this.canGoBack) {
      return;
    }

    this.args.gotoPageIndex(this.pageIndex - 1);
    this.updateQueryValue(this.pageIndex - 1);
  }

  @action
  gotoPageIndex(pageIndex) {
    this.args.gotoPageIndex(pageIndex);
    this.updateQueryValue(pageIndex);
  }
}
