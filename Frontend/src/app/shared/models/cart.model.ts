import { Product } from "./product.model";

export interface CartItem {
  id: string;
  product: Product;
  quantity: number;
  price: number; 
  subtotal: number;
  addedAt: Date;
  notes?: string;
}