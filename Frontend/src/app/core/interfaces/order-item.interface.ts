export interface OrderItem {
  id: number;
  saleId: number;
  categoryId: number;
  name: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
  categoryName?: string;
}

export interface CreateOrderItem {
  categoryId: number;
  name: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  isActive?: boolean;
}

export interface UpdateOrderItem {
  saleId: number;
  categoryId: number;
  name: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  isActive: boolean;
}