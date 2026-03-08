import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderComponent } from './order.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { OrderService } from 'src/app/features/service/order.service';
import { SchemaService } from 'src/app/features/service/schema.service';
import { FormModule } from 'src/app/shared/forms/forms.module';

@NgModule({
  exports: [OrderComponent],
  declarations: [
    OrderComponent
  ],
  imports: [
    FormsModule,
    FormModule,
    ReactiveFormsModule,
    CommonModule
  ],
  providers: [
    SchemaService,
    OrderService,
  ]
})
export class OrderModule { }
