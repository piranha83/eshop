import { createFeatureSelector, createSelector } from "@ngrx/store";
import { Cart } from "../../cart.type";

export const selectCards = createFeatureSelector<Cart>('cart');

export const selectProductsInCards = createSelector(
    selectCards,
    (state: Cart) => state.items
);

export const selectProductsInCardsCount = createSelector(
    selectCards,
    (state: Cart) => state.items.map(m => m.count).reduce((a, b) => a + b, 0)
);