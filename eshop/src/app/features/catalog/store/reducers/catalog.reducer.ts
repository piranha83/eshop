import { createReducer, on } from "@ngrx/store";
import { enterCatalogAction, errorCatalogAction, loadedCatalogAction } from "../actions/catalog.action"
import { Catalog } from "..";
import { Filter } from "src/app/features/toggle/toggle.type";

export const initialCatalogState: Catalog =
{
    products: null,
    loading: false,
    done: false,
    filter: {
        sort: 'Default',
        term: '',
        toggle: 'ShowAll',
        page: 1,
        size: 3,
    } as Filter,
    total: 0,
};

export const catalogReducer = createReducer(
    initialCatalogState,
    on(enterCatalogAction, ( state, { filter } ) => ({
        ...state,
        loading: true,
        filter: filter,
    })),
    on(loadedCatalogAction, ( state, { products }) => ({ 
        ... state,  
        loading: false,
        done: products.data.length == 0,
        products: state.filter.page > 1 /*загрузить еще...*/
            ? [...state.products || [], ...products?.data || [] ] 
            : products.data,
        total: products.total,
    })),
    on(errorCatalogAction, ( state ) => ({ 
        ...state, 
        loading: false
    })),
);

