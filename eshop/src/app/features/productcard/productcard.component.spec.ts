import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PriceService } from 'src/app/features/service/price.service';
import { RouterTestingModule } from '@angular/router/testing';
import { ProductcardComponent } from './productcard.component';
import { Product } from './product.type';

const queryBy = <T>(fixture: ComponentFixture<T>, selector: string) => 
  fixture.debugElement.nativeElement.querySelector(`[data-testid="${selector}"]`);

describe('ProductcardComponent', () => {
  let component: ProductcardComponent;
  let fixture: ComponentFixture<ProductcardComponent>;
  
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ RouterTestingModule.withRoutes([])],
      declarations: [ ProductcardComponent ],
      providers: [ PriceService ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductcardComponent);
    component = fixture.componentInstance;
    component.product = {
      id: 1,
      name: 'Product1',
      img: 'Product1.gif',
      description: 'description',
      price: 100,
      discount: 0.05,
      rate: 1
    } as Product;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should be render product', () => {
    expect(queryBy(fixture, 'product-img').src).toContain('/img/Product1.gif');
    expect(queryBy(fixture, 'product-url').textContent).toContain('Product1');
    expect(queryBy(fixture, 'product-url').href).toContain('/catalog/1');
    expect(queryBy(fixture, 'price').textContent).toContain('₽100.00');
    expect(queryBy(fixture, 'discount').textContent).toContain('5%');
    expect(queryBy(fixture, 'description').textContent).toContain('description');
    expect(queryBy(fixture, 'discount-price').textContent).toContain('₽95.00');
  });

  it('should be rate product', () => {
    component.onRate(2);
    expect(component._product?.rate).toBe(2);
  });

  it('should be raise add to cart event', () => {
    /*component.AddToCart.subscribe((product: Product) => {
      expect(product.id).toBe(1);
    });
    component.onClick();*/
    spyOn(component.AddToCart, 'emit');
    component.onClick();
    expect(component.AddToCart.emit).toHaveBeenCalledWith(component._product || undefined);
  });
});
