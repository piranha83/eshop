import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PriceService } from 'src/app/features/service/price.service';
import { Product } from '../productcard/product.type';
import { CartItem } from './cart.type';

@Component({
  selector: 'app-cart',
  providers: [],
  template: `
  <app-button size="small" class="buy" color="warning" (click)="onToggleCart()">В корзине {{total}}</app-button>
  <div class="button cart" over (clickOut)="hideCart($event)" *ngIf="showCart">
    <a (click)="onClearCart()">Очистить корзину</a>
    <ul>
      <ng-template ngFor let-item [ngForOf]="products">
        <li>
        <app-button size="small" (click)="onRemoveFromCart(item)">X</app-button>
          <span>
            {{item.product?.name}} {{this.item.product.price | currency: 'RUB':'symbol-narrow' }} x {{this.item.count}} шт. = {{this.priceService.getPrice(this.item) | currency: 'RUB':'symbol-narrow' }} со скидкой
          </span>
        </li>
      </ng-template>
      <li class="footer">
        Скидка {{ this.priceService.getTotalDiscount(products) | currency: 'RUB':'symbol-narrow' }}
      </li>
      <li class="footer">
        К оплате {{ this.priceService.getTotalPrice(products) | currency: 'RUB':'symbol-narrow' }}
      </li>
    </ul>
    <div>
      <a (click)="onToggleCart()">Закрыть корзину</a>
      <app-button color="accent" (click)="onCheckout()">Оформить заказ</app-button>
    </div>
  </div>`,
  styles: [],
})
export class CartComponent implements OnInit {
  @Input() products: CartItem [] | null = [];
  @Input() total: number | null = 0;

  @Output() removeFromCart = new EventEmitter<Product>();
  @Output() clearCart = new EventEmitter();
  @Output() checkout = new EventEmitter();

  showCart: boolean = false;

  constructor(public priceService: PriceService)
  {
  }

  hideCart(e: any): void {
    if(e.target.tagName.toUpperCase() != 'BUTTON' && e.target.outerText.indexOf('В корзине')==-1){
      this.showCart = false;
    }
  }

  onToggleCart(): void {
    if(this.products?.length || 0 > 0) {
      this.showCart = !this.showCart;
    }
  }

  onCheckout(): void {
    this.checkout.emit();
    this.showCart = false;
  }

  onRemoveFromCart(val: CartItem): void {
    this.removeFromCart.emit(val.product);
    if(this.products?.length || 0 <= 1) {
      this.showCart = false;
    }
  }

  onClearCart(): void {
    this.clearCart.emit();
    this.showCart = false;
  }

  ngOnInit(): void {
  }

}
