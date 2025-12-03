import { CartItem } from "src/app/features/cart/cart.type";
import { Product } from "src/app/features/productcard/product.type";

export class PriceService {
    getTotalPrice(products: CartItem[] | null): number {
      let total = 0;
      for(let index in products) {
        let i = <number><unknown>index;
        total+=this.getPrice(products[i]);
      }
      return total;
    }

    getTotalDiscount(products:  CartItem[] | null): number {
      let total = 0;
      for(let index in products) {
        let i = <number><unknown>index;
        total+= (products[i].product.price - this.discountPrice(products[i].product)) * products[i].count;
      }
      return total;
    }

    discountPrice(val: Product | null): number {
      return val!= null ? val.price * (1 - val.discount) : 0;
    }

    getPrice(val: CartItem): number {
      return this.discountPrice(val.product) * val.count;
    }
}