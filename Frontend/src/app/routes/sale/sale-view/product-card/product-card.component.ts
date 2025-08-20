import { Component, Input, Output, EventEmitter, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { CartItem } from '../../../../shared/models/cart.model';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule
  ],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.css'
})
export class ProductCardComponent {
  @Input({ required: true }) cartItem!: CartItem;
  @Output() quantityChanged = new EventEmitter<{ itemId: string, quantity: number }>();
  @Output() removeItem = new EventEmitter<string>();

  private readonly toastrService = inject(ToastrService);
  
  baseUrl = environment.baseUrl;
  imageError = false;

  get imageUrl(): string {
    return `${this.baseUrl}${this.cartItem.product.image}`;
  }

  get formattedUnitPrice(): string {
    return this.formatCurrency(this.cartItem.price);
  }

  get formattedSubtotal(): string {
    return this.formatCurrency(this.cartItem.subtotal);
  }

  onImageError(event: any): void {
    this.imageError = true;
    console.warn(`Failed to load image for product: ${this.cartItem.product.name}`);
  }

  onIncreaseQuantity(): void {
    const newQuantity = this.cartItem.quantity + 1;
    this.quantityChanged.emit({ 
      itemId: this.cartItem.id, 
      quantity: newQuantity 
    });
  }

  onDecreaseQuantity(): void {
    if (this.cartItem.quantity > 1) {
      const newQuantity = this.cartItem.quantity - 1;
      this.quantityChanged.emit({ 
        itemId: this.cartItem.id, 
        quantity: newQuantity 
      });
    } else {
      this.onRemove();
    }
  }

  onQuantityInputChange(event: any): void {
    const value = parseInt(event.target.value, 10);
    
    if (isNaN(value) || value < 0) {
      // Reset to current quantity if invalid input
      event.target.value = this.cartItem.quantity;
      this.toastrService.warning('Cantidad inválida', 'Error');
      return;
    }

    if (value === 0) {
      this.onRemove();
      return;
    }

    this.quantityChanged.emit({ 
      itemId: this.cartItem.id, 
      quantity: value 
    });
  }

  onRemove(): void {
    this.removeItem.emit(this.cartItem.id);
  }

  private formatCurrency(amount: number): string {
    return new Intl.NumberFormat('es-US', {
      style: 'currency',
      currency: 'BOB',
      minimumFractionDigits: 2
    }).format(amount);
  }

  get canDecrease(): boolean {
    return this.cartItem.quantity > 1;
  }

  get maxQuantity(): number {
    return 999; // Puedes ajustar este límite según tus necesidades
  }
}