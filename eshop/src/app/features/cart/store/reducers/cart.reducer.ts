import { createReducer, on } from "@ngrx/store";
import { Cart } from "../../cart.type";
import { addProductToCartAction, clearProductsInCartAction, removeProductToCartAction } from "../actions/cart.action"

export const initialCardState: Cart =
{
    items: Cart.load()
}

export const cartReducer = createReducer(
    initialCardState,
    on(addProductToCartAction, (state: Cart, { product }) => {
        let cart = { items: state.items.filter(m => m.product.id != product.id) };
        let index = state.items.findIndex(m => m.product.id === product.id);
        let count = index > -1
            ? state.items[index]?.count ?? 0
            : 0;
        cart.items.splice(index, 0, { count: ++count, product });
        Cart.save(cart.items);
        return (cart);
    }),
    on(removeProductToCartAction, (state: Cart, { product }) => {
        let cart = {
            items: state.items.filter(m => m.product.id != product.id)
        }
        Cart.save(cart.items);
        return (cart);
    }),
    on(clearProductsInCartAction, () => {
        let cart = {
            items: []
        }
        Cart.save(cart.items);
        return (cart);
    }),
);