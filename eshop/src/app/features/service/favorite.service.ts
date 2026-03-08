import { Injectable } from '@angular/core';
import { BehaviorSubject, filter, from, map, Observable, switchMap } from 'rxjs';
import { Product } from 'src/app/features/productcard/product.type';

export interface FavoriteState {
  favorites: FavoriteItem[];
} 

export interface FavoriteItem {
  product: Product;
}

@Injectable()
export class FavoriteService {

  private _state: FavoriteState = {
    favorites: [] as FavoriteItem[]
  }
  
  private state$ = new BehaviorSubject<FavoriteState>(this._state);

  public products$ = this.state$.pipe(map(state => state.favorites));

  public totalProducts$ = this.products$.pipe(map(state => state.length)); 

  constructor(){
    console.log('Favorite');
  }

  protected update() {
    this.state$.next({...this._state});
  }

  addProduct(product: Product): void {
    this._state = {
      ...this._state,
      favorites: [ ...this._state.favorites, { product } as FavoriteItem ]
    };
    this.update();
  }

  removeProduct(product: Product): void {
    this._state = {
      ...this._state,
      favorites: this._state.favorites.filter(item => item.product.id != product.id)
    };
    this.update();
  }

  isFavoriteProduct(product: Product): boolean {
    return this._state.favorites.some(item => item.product.id == product.id);
  }

  toggleFavoriteProduct(product: Product): void {
    if(this.isFavoriteProduct(product))
      this.removeProduct(product);
    else 
      this.addProduct(product);
  }

  protected getProduct(product: Product): Observable<Product> {
    return this.products$.pipe(
      switchMap(items => from(items)),
      filter(item => item.product.id == product.id),
      map(item => item.product)
    );
  }
}

