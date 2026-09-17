// src/app/core/components/base-list.component.ts

import {BaseComponent} from './base-component';

export abstract class BaseListComponent<TItem> extends BaseComponent{
  items: TItem[] = [];

  /**
   * The concrete data-loading implementation is left to the child component.
   */
  protected abstract loadData(): void;

  /**
   * Helper you can call from the child component's ngOnInit.
   */
  protected initList(): void {
    this.loadData();
  }
}
