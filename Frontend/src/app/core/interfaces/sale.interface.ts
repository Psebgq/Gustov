import { CreateOrderItem, OrderItem, UpdateOrderItem } from "./order-item.interface";

export interface Sale {
  id: number;
  subTotal: number;
  tipAmount: number;
  total: number;
  cashRecieved: number;
  cashChange: number;
  createdAt: string;
  orderItems: OrderItem[];
}

export interface CreateSale {
  subTotal: number;
  tipAmount?: number;
  total: number;
  cashRecieved: number;
  cashChange?: number;
  orderItems: CreateOrderItem[];
}

export interface UpdateSale {
  subTotal: number;
  tipAmount: number;
  total: number;
  cashRecieved: number;
  cashChange: number;
  orderItems: UpdateOrderItem[];
}