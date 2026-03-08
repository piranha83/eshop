import { Product } from 'src/app/features/productcard/product.type';
import { FilterPipe } from './filter.pipe';

describe('FilterPipe', () => {
  let products: Product[] = [
    { id:1, price: 100 } as Product,
    { id:2, price: 200 } as Product,
    { id:3, price: 300 } as Product
  ];
  let pipe!: FilterPipe<Product>;

  beforeEach(() => {
    pipe = new FilterPipe<Product>();
    expect(pipe).toBeTruthy();
  });

  it('should be no filter wo filter params', () => {
    let result = pipe.transform([], item => item);
    expect(result).toEqual([]);
  });

  it('should be filter price > 100', () => {
    let result = pipe.transform(products, item => item.price > 100);
    expect(result.length).toBe(2);
    expect(result[0].id).toBe(2);
    expect(result[1].id).toBe(3);
  });
});
