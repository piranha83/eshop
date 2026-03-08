export interface Product {
    readonly id: number;
    readonly name: string;
    readonly description: string;
    readonly price: number;
    readonly inStock: boolean;
    readonly discount: number;
    rate: number;
    readonly img?: string;
};

export interface Paged<T> {
    readonly data: T[];
    total: number;
}

export interface ProductPaged extends Paged<Product> { 
}