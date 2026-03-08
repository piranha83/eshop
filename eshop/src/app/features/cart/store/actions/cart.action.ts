import { createAction, props } from "@ngrx/store";
import { Product } from "src/app/features/productcard/product.type";

export const addProductToCartAction = createAction(
    '[Cart] add product', 
    props<{product: Product}>());

export const removeProductToCartAction = createAction(
    '[Cart] remove product', 
    props<{product: Product}>());

export const clearProductsInCartAction = createAction(
    '[Cart] clear product');