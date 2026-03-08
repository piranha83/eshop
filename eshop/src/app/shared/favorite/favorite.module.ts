import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FavoriteComponent } from './favorite.component';
import { FavoriteService } from 'src/app/features/service/favorite.service';

@NgModule({
  declarations: [FavoriteComponent],
  exports: [FavoriteComponent],
  imports: [
    CommonModule,
  ],
  providers: [
    FavoriteService,
  ]
})
export class FavoriteModule { }
