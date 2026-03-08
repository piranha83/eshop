import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartComponent } from './cart.component';
import { OverDirective } from 'src/app/core/directives/over.directive';
import { ButtonModule } from 'src/app/shared/button/button.module';

@NgModule({
  declarations: [
    CartComponent,
    OverDirective
  ],
  exports: [CartComponent],
  imports: [
    ButtonModule,
    CommonModule
  ]
})
export class CartModule { }
