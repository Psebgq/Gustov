import { Product } from "../../core/interfaces/product.interface";

export interface CartItem {
  id: string;
  product: Product;
  quantity: number;
  price: number; 
  subtotal: number;
  addedAt: Date;
  notes?: string;
}