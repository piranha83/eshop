import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductpageComponent } from '../productcard/productpage.component';
import { CatalogpageComponent } from './catalogpage.component';

export const catalog: Routes = [
  { path: '', component: CatalogpageComponent },
  { path: ':id', component: ProductpageComponent },
];

@NgModule({
  declarations: [],
  exports: [
    RouterModule
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(catalog) 
  ]
})
export class CatalogRoutingModule { }