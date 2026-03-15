// src/app/core/components/base-list.component.ts

import {BaseComponent} from './base-component';

export abstract class BaseListComponent<TItem> extends BaseComponent{
  items: TItem[] = [];

  /**
   * Konkretnu implementaciju punjenja podataka ostavljamo djeci.
   */
  protected abstract loadData(): void;

  /**
   * Helper koji možeš zvati iz ngOnInit dječije komponente.
   */
  protected initList(): void {
    this.loadData();
  }
}
