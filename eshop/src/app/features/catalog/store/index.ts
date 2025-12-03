import { Product } from "src/app/features/productcard/product.type";
import { Filter } from "src/app/features/toggle/toggle.type";

export interface Catalog
{
    products: Product[] | null,
    loading: boolean,
    filter: Filter,
    done: boolean,
    total: number
}