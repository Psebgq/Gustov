import { Injectable, inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { StorageService } from './storage.service';
import { CartItem } from '../../shared/models/cart.model';
import { Product } from '../../core/interfaces/product.interface';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly storageService = inject(StorageService);
  private readonly CART_STORAGE_KEY = 'cart_items';
  
  private cartItemsSubject = new BehaviorSubject<CartItem[]>([]);
  public cartItems$ = this.cartItemsSubject.asObservable();
  
  private totalItemsSubject = new BehaviorSubject<number>(0);
  public totalItems$ = this.totalItemsSubject.asObservable();
  
  private totalAmountSubject = new BehaviorSubject<number>(0);
  public totalAmount$ = this.totalAmountSubject.asObservable();

  constructor() {
    this.loadFromStorage();
  }

  addProduct(product: Product, quantity: number = 1): void {
    const items = this.getItems();
    const existingItem = items.find(item => item.product.id === product.id);

    if (existingItem) {
      existingItem.quantity += quantity;
      existingItem.subtotal = existingItem.quantity * existingItem.price;
    } else {
      const newItem: CartItem = {
        id: this.generateId(),
        product: product,
        quantity: quantity,
        price: product.price,
        subtotal: quantity * product.price,
        addedAt: new Date()
      };
      items.push(newItem);
    }

    this.updateCart(items);
  }

  updateQuantity(itemId: string, quantity: number): void {
    if (quantity <= 0) {
      this.removeItem(itemId);
      return;
    }

    const items = this.getItems();
    const item = items.find(i => i.id === itemId);
    
    if (item) {
      item.quantity = quantity;
      item.subtotal = quantity * item.price;
      this.updateCart(items);
    }
  }

  removeItem(itemId: string): void {
    const items = this.getItems().filter(item => item.id !== itemId);
    this.updateCart(items);
  }

  clear(): void {
    this.updateCart([]);
  }

  getItems(): CartItem[] {
    return this.cartItemsSubject.value;
  }

  getTotalItems(): number {
    return this.totalItemsSubject.value;
  }

  getTotalAmount(): number {
    return this.totalAmountSubject.value;
  }

  hasProduct(productId: number): boolean {
    return this.getItems().some(item => item.product.id === productId);
  }

  getProductQuantity(productId: number): number {
    const item = this.getItems().find(item => item.product.id === productId);
    return item ? item.quantity : 0;
  }
  
  private updateCart(items: CartItem[]): void {
    const totalItems = items.reduce((sum, item) => sum + item.quantity, 0);
    const totalAmount = items.reduce((sum, item) => sum + item.subtotal, 0);
    
    this.cartItemsSubject.next(items);
    this.totalItemsSubject.next(totalItems);
    this.totalAmountSubject.next(totalAmount);
    
    this.saveToStorage(items);
  }

  private saveToStorage(items: CartItem[]): void {
    this.storageService.setItem(this.CART_STORAGE_KEY, items);
  }

  private loadFromStorage(): void {
    const items = this.storageService.getItem<CartItem[]>(this.CART_STORAGE_KEY) || [];
    this.updateCart(items);
  }

  private generateId(): string {
    return `${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
  }
}