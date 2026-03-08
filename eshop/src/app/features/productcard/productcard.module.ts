import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductcardComponent } from './productcard.component';
import { ProductpageComponent } from './productpage.component';
import { RouterModule } from '@angular/router';
import { RaitingModule } from 'src/app/shared/raiting/raiting.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    ProductcardComponent, 
    ProductpageComponent
  ],
  exports: [
    ProductcardComponent, 
    ProductpageComponent
  ],
  imports: [
	RouterModule,
    CommonModule,
    RaitingModule,
    SharedModule
  ]
})
export class ProductcardModule { }
