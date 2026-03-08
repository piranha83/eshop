import { Cart } from "../cart.type";
import { selectProductsInCards } from "./selectors/cart.selector";

export interface AppState
{
    cart: Cart
}

export { selectProductsInCards };