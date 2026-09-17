import { HttpParams } from '@angular/common/http';

/**
 * Converts an object into Angular HttpParams, with support for nested objects.
 *
 * Examples:
 *  { paging: { page: 1, pageSize: 20 } } → ?paging.page=1&paging.pageSize=20
 *  { ids: [1, 2, 3] }                     → ?ids=1&ids=2&ids=3
 *  { search: null }                       → (skipped)
 */
export function buildHttpParams(obj: Record<string, any>, prefix: string = ''): HttpParams {
  let params = new HttpParams();

  if (obj === undefined || obj === null) {
    return params;
  }

  Object.entries(obj).forEach(([key, value]) => {
    // Build the full key (e.g. "paging.page")
    const fullKey = prefix ? `${prefix}.${key}` : key;

    // 1) null / undefined → skip
    if (value === null || value === undefined) {
      return;
    }

    // 2) empty strings → skip
    if (typeof value === 'string' && value.trim() === '') {
      return;
    }

    // 3) array → append each element separately
    if (Array.isArray(value)) {
      value.forEach(val => {
        if (val !== null && val !== undefined) {
          params = params.append(fullKey, String(val));
        }
      });
      return;
    }

    // 4) objects → flatten recursively
    if (typeof value === 'object' && !(value instanceof Date)) {
      const nestedParams = buildHttpParams(value, fullKey);
      nestedParams.keys().forEach(nestedKey => {
        nestedParams.getAll(nestedKey)?.forEach(nestedValue => {
          params = params.append(nestedKey, nestedValue);
        });
      });
      return;
    }

    // 5) Date → convert to ISO string
    if (value instanceof Date) {
      params = params.set(fullKey, value.toISOString());
      return;
    }

    // 6) everything else → as a string
    params = params.set(fullKey, String(value));
  });

  return params;
}
