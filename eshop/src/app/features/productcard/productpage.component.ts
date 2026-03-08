import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { CatalogService } from 'src/app/features/service/catalog.service';
import { Product } from './product.type';

@Component({
  selector: 'app-productpage',
  providers: [],
  template: `<app-productcard [product]="this.product" (AddToCart)="onAddToCart($event)">
    <app-raiting [rate]="this.product?.rate"></app-raiting>
  </app-productcard>`,
  styles: []
})
export class ProductpageComponent implements OnInit, OnDestroy {
  product: any;
  destroy$ = new Subject<boolean>();

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly catalogService: CatalogService) {
      const id = +this.route.snapshot.params['id'];
      this.catalogService
        .get(id)
        .pipe(takeUntil(this.destroy$))
        .subscribe(product => {
          if(product != null)
            this.product = product;
          else
            this.router.navigate(['404']);
        });
  }

  ngOnInit(): void {
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  onAddToCart(product: Product): void {
  }
}
