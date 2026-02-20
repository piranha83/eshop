import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { map, Observable, of } from "rxjs";
import { environment } from "src/environments/environment";
import { Paged, Product } from "src/app/features/productcard/product.type";
import { Filter } from "../toggle/toggle.type";

export class HttpDataService {
  readonly headers = new HttpHeaders().set("Content-Type", "application/json");
  readonly url: string = environment.url + 'product/';

  constructor(readonly http: HttpClient) {}

  public getById<T>(id: number): Observable<T> {
    if (id < 0) throw new Error('Argument id must be gather -1');

    return this.http.get<T>(this.url + id);
  }

  public get<T>(filter: Filter): Observable<Paged<T>> {
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

export class DataService extends HttpDataService {
  private data: any;

  constructor() { super(null!); }

  public setData(val: Product[]): void { this.data = val; }

  public override get<T>(filter: Filter): Observable<T> {
    var res: Product[] = this.data;
    let index = (filter.page - 1) * filter.size;
    if(filter.term != '')
      res = res.filter(x => x.name == filter.term);
    if(filter.sort == 'PriceAsc')
      res = res.sort((a, b) => a.price - b.price);
    if(filter.sort == 'PriceDesc')
      res = res.sort((a, b) => b.price - a.price);
    if(filter.toggle == 'HasDiscount')
      res = res.filter(x => x.discount != 0);
    if(filter.toggle == 'InStock')
      res = res.filter(x => x.inStock);
    if(filter.page > 0 && filter.size > 0)
      res = res.slice(index, index + filter.size);
    return of<any>({ data: res, total: this.data.length });
  }

  public override getById<T>(id: number): Observable<T> {
    if (id < 0) throw new Error('Argument id must be gather -1');
    var res: Product[] = this.data;
    return of<any>(res.filter(x => x.id == id)[0]);
  }
}

