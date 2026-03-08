import { Order } from "src/app/features/order/order.type";

export class OrderService {
    create(order: Order): void {
      localStorage.setItem('order', JSON.stringify(order));
    }

    get get(): Order | undefined {
      const orderData = localStorage.getItem('order');
      return orderData 
        ? JSON.parse(orderData) as Order 
        : undefined;
    }

    checkout(): void {

    }
}