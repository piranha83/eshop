import { TestBed } from '@angular/core/testing';
import { Product } from 'src/app/features/productcard/product.type';

import { FavoriteService } from './favorite.service';

describe('FavoriteService', () => {
  let service: FavoriteService | null;
  
  let product1: Product = {
    id: 1,
    name: 'Product1'
  } as Product;
  let product2 = { id: 2 } as Product;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ 
        FavoriteService
      ]
    });
    service = TestBed.inject(FavoriteService);
  });

  afterEach(()=> {
    service = null;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should be add favorite product', () => {
    service?.addProduct(product1);
    service?.products$.subscribe(products => {
      expect(products.length).toBe(1);
      expect(products[0].product).toEqual(product1);
    });
  });

  it('should be remove product', () => {
    service?.removeProduct(product1);
    service?.removeProduct({ id: 2 } as Product);
    service?.totalProducts$.subscribe(total => {
      expect(total).toBe(0);
    });
  });
  
  it('should be toggle product', () => {
    service?.toggleFavoriteProduct(product1);
    service?.toggleFavoriteProduct(product2);
    service?.toggleFavoriteProduct(product1);
    
    service?.products$.subscribe(products => {
      expect(products.length).toBe(1);
      expect(products[0].product).toEqual(product2);
    });
  });
});
