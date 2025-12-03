import { Product } from "../productcard/product.type";

export interface CartItem { 
  count: number, 
  readonly product: Product
}

export class Cart {
  readonly items: CartItem[] = Cart.load();
  
  static load(): CartItem[] {
    const temp = localStorage.getItem('cart');
    let cartItems: CartItem[] = [];
    if(temp) {
        cartItems = <CartItem[]>JSON.parse(temp); 
    }
    return cartItems;
  }

  static save(value : CartItem[]) {
      localStorage.setItem('cart', JSON.stringify(value));
  }
}