import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FavoriteService } from 'src/app/features/service/favorite.service';

@Component({
  selector: 'app-favorite',
  template: `
  <span class="material-icons app-raiting" [ngClass]="isFavorite" (click)="onToggleFavoriteClick()">
    favorite
  </span>`,
  styles: [],
})
export class FavoriteComponent implements OnInit, OnDestroy {
  @Input() product!: any;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private readonly favoriteService: FavoriteService) {
    /*this.favoriteService.products$
      .pipe(takeUntil(this.destroy$))
      .subscribe(items => console.log(items));*/
  }

  ngOnInit(): void {
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  onToggleFavoriteClick(): void {
    this.favoriteService.toggleFavoriteProduct(this.product);
  }

  get isFavorite(): string {
    return this.favoriteService.isFavoriteProduct(this.product) ? 'active' : '';
  }
}
