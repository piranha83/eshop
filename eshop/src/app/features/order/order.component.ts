import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/features/service/order.service';
import { Schema, SchemaService } from 'src/app/features/service/schema.service';
import { Order } from './order.type';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styles: [``]
})
export class OrderComponent implements OnInit {
  schema: Schema;
  data: Order | undefined;
  delivery: boolean = true;

  constructor(
    readonly orderService: OrderService, 
    readonly schemaService: SchemaService) {
      console.log('order');
      this.data = this.orderService.get;
      this.schema = this.schemaService.forOrder(this.data);
  }

  ngOnInit(): void {
  }

  valid(name: string): boolean {
    const control = this.schema.form.get(name);
    return control?.disabled || true && control?.valid || false;
  }

  changeDelivery(e: any) {
    if(e.value != "athome") {
      this.delivery = false;
    }
    else {
      this.schema.form.get('checkout')?.setValue("paypal");
      this.delivery = true;
    }
  }

  onSubmit(): void {
    if(this.schema.form.invalid) {
      return;
    }
    this.orderService.create(this.schema.form.value);
  }
}
