import { createFeatureSelector, createSelector } from "@ngrx/store";
import { Catalog } from "..";

export const selectCatalog = createFeatureSelector<Catalog>('catalog');
export const selectCatalogProducts = createSelector(selectCatalog, state => state.products);
export const selectCatalogFilter = createSelector(selectCatalog, state => state.filter);
export const selectCatalogLoading = createSelector(selectCatalog, state => state.loading);
export const selectCatalogNext = createSelector(selectCatalog, state => state.products?.length != state.total);