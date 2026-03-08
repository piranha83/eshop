import { createAction, props } from "@ngrx/store";
import { ProductPaged } from "src/app/features/productcard/product.type";
import { Filter } from "src/app/features/toggle/toggle.type";

export const enterCatalogAction = createAction(
    '[Catalog] enter',
    props<{ filter: Filter }>())

export const loadedCatalogAction = createAction(
    '[Catalog Api] loaded', 
    props<{products: ProductPaged}>());

export const errorCatalogAction = createAction(
    '[Catalog Api] error', 
    props<{error: any}>());
