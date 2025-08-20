import { Component, EventEmitter, InjectionToken, Input, Output, inject } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { ToastrService } from 'ngx-toastr';
import { Product } from '../../../../shared/models/product.model';
import { environment } from '../../../../../environments/environment';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog } from '@angular/material/dialog';
import { ProductOptionComponent } from '../product-option/product-option.component';

const Operations = {
  EDIT: 'edit',
  DELETE: 'delete'
};

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [
    CommonModule,
    CurrencyPipe,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatTooltipModule,

  ],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.css'
})
export class ProductCardComponent {
  @Input({ required: true }) product!: Product;
  @Output() productUpdated = new EventEmitter<Product>();
  
  baseUrl = environment.baseUrl;
  imageUrl: string | null = null;
  private readonly toastrService = inject(ToastrService);

  constructor(
    private dialog: MatDialog
  ) { }
  
  imageError = false;

  ngOnInit() {
    if (!this.imageUrl) {
      this.imageUrl = `${this.baseUrl}${this.product.image}`;
      console.warn(`No image URL provided for product: ${this.product.name}`);
    }
  }

  onImageError(event: any): void {
    this.imageError = true;
    console.warn(`Failed to load image for product: ${this.product.name}`);
  }

  onAddToOrder(): void {
    this.toastrService.success(`${this.product.name} agregado al carrito`, 'Producto agregado');
    console.log('Agregar al carrito:', this.product);
  }

  onViewDetails(): void {
    this.toastrService.info(`Mostrando detalles de ${this.product.name}`, 'Detalles del producto');
    console.log('Ver detalles:', this.product);
  }

  showOptions(operation: string, product: Product) {
    let data: object = {
      operation: operation,
      disableClose: true
    }
    switch (operation) {
      case Operations.EDIT:
        data = {
          title: "Modificar Producto",
          product: product,
          ...data
        }
        break;
    }

    const dialogRef = this.dialog.open(ProductOptionComponent, {
      data: data
    });

    dialogRef.afterClosed().subscribe(() => {
      this.productUpdated.emit(product);
    });
  }

  get formattedPrice(): string {
    return new Intl.NumberFormat('es-US', {
      style: 'currency',
      currency: 'BOB',
      minimumFractionDigits: 2
    }).format(this.product.price);
  }
}