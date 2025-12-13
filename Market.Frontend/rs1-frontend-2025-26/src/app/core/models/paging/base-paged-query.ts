import {PageRequest} from './page-request';

// pratimo klasu c# BasePagedQuery.cs
export class BasePagedQuery {
  paging: PageRequest;

  constructor() {
    this.paging = new PageRequest();
  }
}
