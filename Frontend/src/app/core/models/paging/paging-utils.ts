import { PageRequest } from './page-request';

/**
 * Utility functions for creating common paging configurations.
 */

/**
 * Default paging (10 items per page).
 */
export const defaultPaging: PageRequest = new PageRequest(1, 10);

/**
 * Paging for fetching all items (1000 items).
 * Use for dropdowns and small datasets.
 */
export const allItemsPaging: PageRequest = new PageRequest(1, 1000);

/**
 * Large page size (100 items).
 */
export const largePaging: PageRequest = new PageRequest(1, 100);

/**
 * Small page size (5 items).
 */
export const smallPaging: PageRequest = new PageRequest(1, 5);

/**
 * Create a custom paging configuration.
 */
export function createPaging(page: number = 1, pageSize: number = 10): PageRequest {
  return new PageRequest(page, pageSize);
}
