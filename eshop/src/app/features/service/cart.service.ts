import { CartItem } from "src/app/features/cart/cart.type";
import { Product } from "src/app/features/productcard/product.type";

export class CartService {
    private _inCart: CartItem[] = [];

    addProduct(product: Product): void {
      let productinCart = this._inCart.find(function(m){ return m.product === product; });
      if(!productinCart) {
        productinCart = { count: 0, product };
        this._inCart.push(productinCart);
      }
      productinCart.count++;
    }

    removeProduct(product: Product): void {
      this._inCart = this._inCart.filter(function(m){ return m.product != product; }); 
    }

    getCart(): CartItem[] {
      return this._inCart;
    }

    getTotal(): number {
      console.log('1');
      let total = 0;
      for(let i in this._inCart) {
        total+=this._inCart[i].count;
      }
      return total;
    }

    clear(): void {
      this._inCart = [];
    }

    checkout(): void {
      
    }
}