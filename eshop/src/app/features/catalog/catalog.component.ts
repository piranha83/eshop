import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CartService } from 'src/app/features/service/cart.service';
import { CatalogService } from 'src/app/features/service/catalog.service';
import { debounceTime, distinctUntilChanged, Subject, takeUntil } from 'rxjs';
import { select, Store } from '@ngrx/store';
import { Catalog } from '../catalog/store/index';
import { selectCatalogFilter, selectCatalogProducts, selectCatalogLoading, selectCatalogNext } from './store/selectors/catalog.selector';
import { enterCatalogAction } from './store/actions/catalog.action';
import { initialCatalogState } from './store/reducers/catalog.reducer';
import { addProductToCartAction, clearProductsInCartAction, removeProductToCartAction } from '../cart/store/actions/cart.action';
import { selectProductsInCards, selectProductsInCardsCount } from '../cart/store/selectors/cart.selector'
import { Filter, parseToggleValueType, SortValueType, ToggleValueType } from 'src/app/features/toggle/toggle.type';
import { Product } from '../productcard/product.type';

@Component({
  selector: 'app-catalog',
  template: `
  <ng-container *ngIf="filter$ | async as filter">
    <ng-container *ngIf="loading$ | async">
      Загрузка...
    </ng-container>
    <app-cart [products]="inCart$ | async" [total]="total$ | async" (removeFromCart)="onRemoveFromCart($event)" (clearCart)="onClearCart()" (checkout)="onCheckout()">
    </app-cart>
    <app-toggle [sort]="filter.sort" (changed)="onToggleChanged(filter, $event)" (sortChanged)="onSortChanged(filter, $event)">
      <app-button class="toggle" data="ShowAll">«Показать все»</app-button>
      <app-button class="toggle" data="InStock">«В наличии»</app-button>
      <app-button class="toggle" data="HasDiscount">«Со скидкой»</app-button>
    </app-toggle>
    <input placeholder="поиск" class="search button" (keyup)="onSearch(filter,$event)"/>
    <div class="center">
      <ng-template ngFor let-item [ngForOf]="this.products$ | async">
        <app-productcard [product]="item">
          <app-favorite [product]="item"></app-favorite>
          <app-button [isDisabled]="!item.inStock" (click)="onAddToCart(item)">В корзину</app-button>
        </app-productcard>
      </ng-template>
      <ng-template *ngIf="this.products$ | async as product">
        <ng-container *ngIf="product.length === 0">
          Не найдено.
        </ng-container>
      </ng-template>
    </div>
    <div class="center">
      <app-button *ngIf="next$ | async " color="primary" (click)="onLoad(filter)">Загрузить еще</app-button>
    </div>
  </ng-container>
  `,
  styles: [],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CatalogComponent implements OnDestroy { 
  products$ = this.store.pipe(select(selectCatalogProducts));
  filter$ = this.store.pipe(select(selectCatalogFilter));
  loading$ = this.store.pipe(select(selectCatalogLoading));
  next$ = this.store.pipe(select(selectCatalogNext));

  inCart$ = this.store.pipe(select(selectProductsInCards));
  total$ = this.store.pipe(select(selectProductsInCardsCount));

  term$ = new Subject<Filter>();
  destroy$: Subject<boolean> = new Subject<boolean>();
  
  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    public readonly catalogService: CatalogService,
    private readonly cartService: CartService,
    readonly store: Store<Catalog>
    )
  {
    this.route.queryParams.subscribe((queryParams: any) => {
      let temp = { ...initialCatalogState,
        ... {
          toggle: parseToggleValueType(queryParams.toggle),
          sort: queryParams.sort || 'Default',
        } as Filter 
      };
      this.loadCatalog(temp.filter);
      this.term$
        .pipe(debounceTime(3000))
        .pipe(distinctUntilChanged())
        .pipe(takeUntil(this.destroy$))
        .subscribe(filter => this.loadCatalog(filter));
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  onSearch(filter: Filter, e: any): void {
    this.term$.next({...filter, page: 1, term: e.target.value});
  }

  onAddToCart(product: Product): void {
    this.store.dispatch(addProductToCartAction({ product }));
  }

  onRemoveFromCart(product: Product): void {
    this.store.dispatch(removeProductToCartAction({ product }));
  }

  onClearCart(): void {
    this.store.dispatch(clearProductsInCartAction());
  }

  onCheckout(): void {
    this.cartService.checkout();
    this.router.navigate(['order']);
  }

  onToggleChanged(filter: Filter, toggleValue: string): void {
    let toggle = toggleValue as ToggleValueType;
    this.loadCatalog({ ...filter, page: 1, toggle });
  }

  onSortChanged(filter: Filter, sort: SortValueType): void {
    this.loadCatalog({ ...filter, page: 1, sort });
  }

  onLoad(filter: Filter): void {
    this.loadCatalog({ ...filter, page: filter.page + 1 });
  }

  onRate(rate: number, product: Product): void {
    product.rate = rate;
  }

  loadCatalog(filter: Filter){
    this.store.dispatch(enterCatalogAction({ filter }));
  }
}
