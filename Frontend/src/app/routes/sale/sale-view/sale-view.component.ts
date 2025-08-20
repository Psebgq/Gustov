import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialogModule } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil, finalize } from 'rxjs/operators';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { ProductCardComponent } from './product-card/product-card.component';
import { CartService } from '../../../shared/services/cart.service';
import { SaleService } from '../../../core/services/sale.service';
import { CartItem } from '../../../shared/models/cart.model';
import { CreateOrderItem } from '../../../core/interfaces/order-item.interface';
import { CreateSale } from '../../../core/interfaces/sale.interface';

@Component({
  selector: 'app-sale-view',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ProductCardComponent
  ],
  templateUrl: './sale-view.component.html',
  styleUrl: './sale-view.component.css'
})
export class SaleViewComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();
  private readonly cartService = inject(CartService);
  private readonly saleService = inject(SaleService);
  private readonly toastrService = inject(ToastrService);

  cartItems: CartItem[] = [];
  totalItems: number = 0;
  totalAmount: number = 0;
  subtotal: number = 0;
  tipAmount: number = 0;
  cashReceived: number = 0;
  cashChange: number = 0;
  isProcessingSale: boolean = false;

  ngOnInit(): void {
    this.subscribeToCart();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private subscribeToCart(): void {
    this.cartService.cartItems$
      .pipe(takeUntil(this.destroy$))
      .subscribe(items => {
        this.cartItems = items;
        this.calculateTotals();
      });

    this.cartService.totalItems$
      .pipe(takeUntil(this.destroy$))
      .subscribe(total => {
        this.totalItems = total;
      });

    this.cartService.totalAmount$
      .pipe(takeUntil(this.destroy$))
      .subscribe(amount => {
        this.totalAmount = amount;
        this.calculateTotals();
      });
  }

  private calculateTotals(): void {
    this.subtotal = this.totalAmount;
    this.calculateTotal();
  }

  private calculateTotal(): void {
    const total = this.subtotal + this.tipAmount;
    this.cashChange = this.cashReceived > total ? this.cashReceived - total : 0;
  }

  onCashReceivedChange(): void {
    // Validar que el valor sea positivo
    if (this.cashReceived < 0) {
      this.cashReceived = 0;
    }
    this.calculateTotal();
  }

  onTipAmountChange(): void {
    // Validar que el valor sea positivo
    if (this.tipAmount < 0) {
      this.tipAmount = 0;
    }
    this.calculateTotal();
  }

  onQuantityChange(itemId: string, newQuantity: number): void {
    if (newQuantity <= 0) {
      this.onRemoveItem(itemId);
      return;
    }

    this.cartService.updateQuantity(itemId, newQuantity);
    this.toastrService.success('Cantidad actualizada', 'Carrito actualizado');
  }

  onRemoveItem(itemId: string): void {
    const item = this.cartItems.find(i => i.id === itemId);
    if (item) {
      this.cartService.removeItem(itemId);
      this.toastrService.success(
        `${item.product.name} eliminado del carrito`, 
        'Producto eliminado'
      );
    }
  }

  onClearCart(): void {
    if (this.cartItems.length === 0) {
      this.toastrService.info('El carrito ya está vacío', 'Carrito vacío');
      return;
    }

    this.cartService.clear();
    this.toastrService.success('Carrito vaciado', 'Carrito limpiado');
  }

  onProcessSale(): void {
    if (this.cartItems.length === 0) {
      this.toastrService.warning('No hay productos en el carrito', 'Carrito vacío');
      return;
    }

    if (this.cashReceived < (this.subtotal + this.tipAmount)) {
      this.toastrService.warning('El efectivo recibido es insuficiente', 'Efectivo insuficiente');
      return;
    }

    this.isProcessingSale = true;

    const orderItems: CreateOrderItem[] = this.cartItems.map(cartItem => ({
      saleId: 0, // Se asignará en el backend
      categoryId: cartItem.product.categoryId,
      name: cartItem.product.name,
      quantity: cartItem.quantity,
      unitPrice: cartItem.price,
      totalPrice: cartItem.subtotal,
      isActive: true
    }));

    const createSaleData: CreateSale = {
      subTotal: this.subtotal,
      tipAmount: this.tipAmount,
      total: this.subtotal + this.tipAmount,
      cashRecieved: this.cashReceived,
      cashChange: this.cashChange,
      orderItems: orderItems
    };

    this.saleService.create(createSaleData)
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => {
          this.isProcessingSale = false;
        })
      )
      .subscribe({
        next: (sale) => {
          this.toastrService.success(
            `Venta procesada exitosamente. ID: ${sale.id}`, 
            'Venta completada'
          );
          
          // Limpiar el carrito después de una venta exitosa
          this.cartService.clear();
          
          // Resetear valores de pago
          this.resetPaymentValues();
          
          console.log('Venta creada:', sale);
        },
        error: (error) => {
          console.error('Error al procesar la venta:', error);
          
          let errorMessage = 'Error al procesar la venta';
          
          if (error.status === 400) {
            errorMessage = 'Datos de venta inválidos';
          } else if (error.status === 500) {
            errorMessage = 'Error interno del servidor';
          }
          
          this.toastrService.error(errorMessage, 'Error en la venta');
        }
      });
  }

  private resetPaymentValues(): void {
    this.tipAmount = 0;
    this.cashReceived = 0;
    this.cashChange = 0;
  }

  get formattedSubtotal(): string {
    return this.formatCurrency(this.subtotal);
  }

  get formattedTotal(): string {
    return this.formatCurrency(this.subtotal + this.tipAmount);
  }

  get formattedTip(): string {
    return this.formatCurrency(this.tipAmount);
  }

  get formattedCashReceived(): string {
    return this.formatCurrency(this.cashReceived);
  }

  get formattedCashChange(): string {
    return this.formatCurrency(this.cashChange);
  }

  private formatCurrency(amount: number): string {
    return new Intl.NumberFormat('es-US', {
      style: 'currency',
      currency: 'BOB',
      minimumFractionDigits: 2
    }).format(amount);
  }

  get isEmpty(): boolean {
    return this.cartItems.length === 0;
  }

  get canProcessSale(): boolean {
    return this.cartItems.length > 0 && 
           this.cashReceived >= (this.subtotal + this.tipAmount) &&
           !this.isProcessingSale;
  }

  trackByItemId(index: number, item: CartItem): string {
    return item.id;
  }
}