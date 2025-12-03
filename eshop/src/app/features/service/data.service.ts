import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { map, Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Paged } from "src/app/features/productcard/product.type";
import { Filter, SortValueType } from "../toggle/toggle.type";

export class DataService {
  private data: any;

  setData<T>(val: T): void {
    this.data = val;
  }

  get<T>(filter: Filter): T {
    return this.data;
  }

  public getById<T>(id: number): T {
    return this.data[0];
  }
}

export class HttpDataService {
  readonly headers = new HttpHeaders().set("Content-Type", "application/json");
  readonly url: string = environment.url + 'products/';

  constructor(readonly http: HttpClient) {}

  public getById<T>(id: number): Observable<T> {
    if (id < 0) throw new Error('Argument id must be gather -1');

    return this.http.get<T>(this.url + id);
  }

  get<T>(filter: Filter): Observable<Paged<T>> {
    if (filter.page <= 0) throw new Error('Argument page must be gather 0');
    if (filter.size <= 0) throw new Error('Argument size must be gather 0');

    let param = new HttpParams()
      .set('_page', filter.page)
      .set('_limit', filter.size);
    if(filter.term != '')
      param = param.set('name_like', filter.term);
    if(filter.sort == 'PriceAsc')
      param = param.set('_sort', 'price').set('_order', 'asc');
    if(filter.sort == 'PriceDesc')
      param = param.set('_sort', 'price').set('_order', 'desc');
    if(filter.toggle == 'HasDiscount')
      param = param.set('discount_ne', '0');
    if(filter.toggle == 'InStock')
      param = param.set('inStock', 'true');

    return this.http.get<T[]>(this.url, {
      headers: this.headers,
      params: param,
      observe: 'response'
    })
    .pipe(map(response => {
      if(response.body != null)
      {
        const total = parseInt(response.headers.get('x-total-count') || '0');
        return { data: response.body, total: total };
      }
      throw new Error('Argument body must be set');
    }));
  }
}
