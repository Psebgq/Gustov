import { HttpClient } from "@angular/common/http";
import { CreateOrderItem, OrderItem, UpdateOrderItem } from "../interfaces/order-item.interface";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class OrderItemService {
  root: string = 'order-item';

  constructor(
    private readonly http: HttpClient
  ) { }

  findOne(orderItemId: number) {
    return this.http.get<OrderItem>(`/api/${this.root}/${orderItemId}`);
  }

  findAll() {
    return this.http.get<OrderItem[]>(`/api/${this.root}`);
  }

  findBySale(saleId: number) {
    return this.http.get<OrderItem[]>(`/api/${this.root}/sale/${saleId}`);
  }

  create(createOrderItemDto: CreateOrderItem) {
    return this.http.post<OrderItem>(`/api/${this.root}`, createOrderItemDto);
  }

  update(orderItemId: number, updateOrderItemDto: UpdateOrderItem) {
    return this.http.put<OrderItem>(`/api/${this.root}/${orderItemId}`, updateOrderItemDto);
  }
}