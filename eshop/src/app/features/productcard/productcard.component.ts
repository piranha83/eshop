import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PriceService } from 'src/app/features/service/price.service';
import { Product } from './product.type';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-productcard',
  providers: [],
  template: `
  <div class="app-productcard">
    <img data-testid="product-img" *ngIf="_product?.img" src="{{_img}}" alt=""/>
    <a data-testid="product-url" [routerLink]="['/catalog', _product?.id]">{{_product?.name}}</a>
    <span data-testid="price" class="line">{{_product?.price | currency: 'RUB':'symbol-narrow' }}</span>
    <span data-testid="discount">{{ _product?.discount | percent }}</span>
    <p data-testid="description">{{_product?.description}}</p>
    <h3 data-testid="discount-price">
      {{this.priceService.discountPrice(_product) | currency: 'RUB':'symbol-narrow' }}
    </h3>
    <ng-content select="<app-raiting>"></ng-content>
    <ng-content select="<app-button>"></ng-content>
  <div>
  `,
  styles: [],
})
export class ProductcardComponent implements OnInit {
  _product: Product | null = null;
  _img: string | null = null;

  @Input() set product(val : Product) {
    this._product = val;
    if (val != null)
    {
      this._img = `${environment.url}img/${val?.img}`;
    }
  }

  @Output() AddToCart = new EventEmitter<Product>();

  constructor(public priceService: PriceService)
  {
  }

  ngOnInit(): void {
  }

  onRate(rate: number): void {
    if(this._product)
      this._product.rate = rate;
  }

  onClick(): void {
    if(this._product)
      this.AddToCart.emit(this._product);
  }

}
