import { HttpParams } from '@angular/common/http';

/**
 * Pretvara objekat u Angular HttpParams, sa podrškom za ugnježdene objekte.
 *
 * Primjeri:
 *  { paging: { page: 1, pageSize: 20 } } → ?paging.page=1&paging.pageSize=20
 *  { ids: [1, 2, 3] }                     → ?ids=1&ids=2&ids=3
 *  { search: null }                       → (preskače se)
 */
export function buildHttpParams(obj: Record<string, any>, prefix: string = ''): HttpParams {
  let params = new HttpParams();

  if (obj === undefined || obj === null) {
    return params;
  }

  Object.entries(obj).forEach(([key, value]) => {
    // Konstruiši puni ključ (npr. "paging.page")
    const fullKey = prefix ? `${prefix}.${key}` : key;

    // 1) null / undefined → preskoči
    if (value === null || value === undefined) {
      return;
    }

    // 2) prazni stringovi → preskoči
    if (typeof value === 'string' && value.trim() === '') {
      return;
    }

    // 3) niz → dodaj svaki element posebno
    if (Array.isArray(value)) {
      value.forEach(val => {
        if (val !== null && val !== undefined) {
          params = params.append(fullKey, String(val));
        }
      });
      return;
    }

    // 4) objekti → rekurzivno spljošti
    if (typeof value === 'object' && !(value instanceof Date)) {
      const nestedParams = buildHttpParams(value, fullKey);
      nestedParams.keys().forEach(nestedKey => {
        nestedParams.getAll(nestedKey)?.forEach(nestedValue => {
          params = params.append(nestedKey, nestedValue);
        });
      });
      return;
    }

    // 5) Date → pretvori u ISO string
    if (value instanceof Date) {
      params = params.set(fullKey, value.toISOString());
      return;
    }

    // 6) sve ostalo → kao string
    params = params.set(fullKey, String(value));
  });

  return params;
}
