# API Services

## Šta su API servisi?

`api-services` sadrže **tanke servise** koji komuniciraju direktno sa backend API kontrolerima.

To su servisi:
* **1:1 sa backend kontrolerima** – jedan servis po kontroleru
* **bez poslovne logike** – samo HTTP pozivi
* **bez UI logike** – ne brinu se o forme, validacijama, notifikacijama
* **bez lokalnog state-a** – ne čuvaju podatke
* **bez transformacija** – vraćaju podatke onakve kakve backend vrati
* **samo poziv → DTO ulaz / DTO izlaz**

Ovo je **najniži FE sloj** koji priča s backendom.

---

## Arhitektura - Gdje ide šta?

```
Backend (C# Controllers)
         ↓
    API Servisi (1:1 mapping)
         ↓
    Komponente (direktno koriste API servise)
         ↓
    (opciono) Form Servisi (za reusable forme)
```

**Princip:** Komponente direktno koriste API servise. Ako ista forma postoji u više komponenti (npr. add + edit), izvlačimo je u **Form servis**.

---

## Struktura foldera

```
api-services/
├── auth/
│   ├── auth-api.model.ts
│   └── auth-api.service.ts
├── product-categories/
│   ├── product-categories-api.model.ts
│   └── product-categories-api.service.ts
├── products/
│   ├── products-api.models.ts
│   └── products-api.service.ts
└── readme.md
```

Svaki folder predstavlja jedan backend kontroler.

---

## Konvencije imenovanja

### Folderi
```
kebab-case, jednina ili množina prema backend kontroleru
Primjer: product-categories, products, auth
```

### Fajlovi
```
{entity}-api.model.ts    // DTOs i interfejsi
{entity}-api.service.ts  // HTTP servis
```

### Klase i interfejsi
```typescript
// REQUEST modeli (input za backend)
ListProductCategoriesRequest
CreateProductCommand
UpdateProductCommand
UpsertProductCategoryCommand

// RESPONSE modeli (output sa backenda)
ListProductCategoriesResponse
ProductCategoryListItem
ProductDetailDto

// SERVISI
ProductCategoriesApiService
ProductsApiService
AuthApiService
```

---

## Anatomija API servisa

### 1. Model fajl (`*-api.model.ts`)

Sadrži **samo TypeScript tipove** – DTOs koji se koriste za komunikaciju s backendom.

```typescript
import { BasePagedQuery } from '../../core/models/basePagedQuery';
import { PageResult } from '../../core/models/pageResult';

// REQUEST - šta šaljemo backendu
export class ListProductCategoriesRequest extends BasePagedQuery {
  search?: string | null;
  onlyEnabled?: boolean | null;
}

// RESPONSE ITEM - jedan objekat iz liste
export interface ProductCategoryListItem {
  id: number;
  name: string;
  isEnabled: boolean;
}

// RESPONSE - šta dobijamo nazad
export type ListProductCategoriesResponse = PageResult<ProductCategoryListItem>;

// COMMAND - za CREATE/UPDATE
export interface UpsertProductCategoryCommand {
  name?: string | null;
}
```

**Pravila:**
- Koristimo tipove koji odgovaraju backend DTOs
- `Request` sufiks za query parametre
- `Response` sufiks za odgovore
- `Command` sufiks za create/update payloade
- Nullable property definišemo kao `property?: type | null`

---

### 2. Service fajl (`*-api.service.ts`)

Sadrži **samo HTTP pozive** prema backend kontroleru.

```typescript
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ListProductCategoriesRequest,
  ListProductCategoriesResponse,
  ProductCategoryListItem,
  UpsertProductCategoryCommand
} from './product-categories-api.model';
import { buildHttpParams } from '../../core/models/buildHttpParams';

@Injectable({
  providedIn: 'root',
})
export class ProductCategoriesApiService {
  private readonly baseUrl = `${environment.apiUrl}/ProductCategories`;
  private http = inject(HttpClient);

  /**
   * Lista kategorija sa query parametrima.
   */
  list(request?: ListProductCategoriesRequest): Observable<ListProductCategoriesResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListProductCategoriesResponse>(this.baseUrl, { params });
  }

  /**
   * Dohvat jedne kategorije po ID-u.
   */
  getById(id: number): Observable<ProductCategoryListItem> {
    return this.http.get<ProductCategoryListItem>(`${this.baseUrl}/${id}`);
  }

  /**
   * Kreiranje nove kategorije.
   */
  create(payload: UpsertProductCategoryCommand): Observable<number> {
    return this.http.post<number>(this.baseUrl, payload);
  }

  /**
   * Izmjena postojeće kategorije.
   */
  update(id: number, payload: UpsertProductCategoryCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Brisanje kategorije.
   */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```

**Pravila:**
- `providedIn: 'root'` – singleton servis
- `inject()` umjesto `constructor()` dependency injection
- `baseUrl` property za endpoint
- Metode vraćaju `Observable<T>`
- Bez `.subscribe()` – to radi komponenta/state servis
- Bez `try/catch` – error handling je na višem nivou
- Bez transformacija podataka

---

## CRUD konvencije

| Operacija | HTTP metod | Backend endpoint | Metoda servisa |
|-----------|-----------|------------------|----------------|
| **Lista** | GET | `/ProductCategories` | `list(request?)` |
| **Dohvat po ID** | GET | `/ProductCategories/{id}` | `getById(id)` |
| **Kreiranje** | POST | `/ProductCategories` | `create(payload)` |
| **Izmjena** | PUT | `/ProductCategories/{id}` | `update(id, payload)` |
| **Brisanje** | DELETE | `/ProductCategories/{id}` | `delete(id)` |

---

## ✅ Dobri primjeri

```typescript
// ✅ Čist API poziv
list(request?: ListProductsRequest): Observable<ListProductsResponse> {
  const params = request ? buildHttpParams(request) : undefined;
  return this.http.get<ListProductsResponse>(this.baseUrl, { params });
}

// ✅ Jednostavan endpoint
getById(id: number): Observable<ProductDetailDto> {
  return this.http.get<ProductDetailDto>(`${this.baseUrl}/${id}`);
}

// ✅ Explicit tipizacija
create(payload: CreateProductCommand): Observable<number> {
  return this.http.post<number>(this.baseUrl, payload);
}
```

---

## ❌ Loši primjeri

```typescript
// ❌ NE - poslovna logika u API servisu
list(request?: ListProductsRequest): Observable<Product[]> {
  return this.http.get<Product[]>(this.baseUrl).pipe(
    map(products => products.filter(p => p.isActive)), // ❌ filtriranje
    tap(products => console.log('Loaded products'))    // ❌ logging
  );
}

// ❌ NE - UI logika u API servisu
create(payload: CreateProductCommand): Observable<number> {
  return this.http.post<number>(this.baseUrl, payload).pipe(
    tap(() => this.toastr.success('Product created!')) // ❌ notifikacija
  );
}

// ❌ NE - lokalni state
private products: Product[] = []; // ❌ čuvanje podataka

list(): Observable<Product[]> {
  if (this.products.length > 0) { // ❌ caching
    return of(this.products);
  }
  return this.http.get<Product[]>(this.baseUrl);
}

// ❌ NE - transformacija podataka
getById(id: number): Observable<ProductViewModel> {
  return this.http.get<ProductDto>(`${this.baseUrl}/${id}`).pipe(
    map(dto => this.mapToViewModel(dto)) // ❌ mapping
  );
}
```

---

## Gdje ide šta?

| Odgovornost | Gdje? |
|-------------|-------|
| HTTP poziv prema backendu | ✅ API servis |
| Tipizacija (DTOs) | ✅ API model |
| Poslovna logika | ❌ Komponenta |
| UI logika | ❌ Komponenta |
| Validacija formi | ❌ Reactive Forms (komponenta) |
| Error handling | ❌ Komponenta / Interceptor |
| Loading state | ❌ Komponenta (BaseComponent) |
| Transformacija podataka | ❌ Komponenta (ako je jednostavna) |
| Caching | ❌ Komponenta (ako treba) |
| Notifikacije | ❌ Komponenta |
| **Kreiranje formi** | ✅ **Form servis (ako se reusuje)** |

---

---

## Kako koristiti API servise?

API servise koristimo **DIREKTNO u komponentama**.

```typescript
// ✅ DOBRO - direktno u komponenti
export class ProductListComponent extends BaseListPagedComponent<
  ListProductsQueryDto,
  ListProductsRequest
> {
  private api = inject(ProductsApiService);
  
  ngOnInit() {
    this.initList();
  }
  
  protected override loadPagedData(): void {
    this.startLoading();
    
    this.api.list(this.request).subscribe({
      next: (result) => {
        this.handlePageResult(result);
        this.stopLoading();
      },
      error: (err) => this.stopLoading(err.message)
    });
  }
}
```

**Zato što:**
- ✅ Kod je jednostavan i jasan
- ✅ Sve na jednom mjestu - lakše za učenje
- ✅ Nema nepotrebnih apstrakcija
- ✅ Prirodan flow podataka
```

---

## Query parametri

Koristimo helper funkciju `buildHttpParams` za query string parametire:

```typescript
list(request?: ListProductsRequest): Observable<ListProductsResponse> {
  const params = request ? buildHttpParams(request) : undefined;
  return this.http.get<ListProductsResponse>(this.baseUrl, { params });
}
```

Ova funkcija automatski:
- Pretvara objekt u `HttpParams`
- Preskače `null` i `undefined` vrijednosti
- Enkoduje specijalne karaktere

---
