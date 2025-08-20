import { Component, Input, OnInit, inject, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';

import { ProductCardComponent } from '../product-card/product-card.component';
import { ProductOptionComponent } from '../product-option/product-option.component';
import { Category } from '../../../../core/interfaces/category.interface';
import { ProductService } from '../../../../core/services/product.service';
import { Product } from '../../../../core/interfaces/product.interface';

@Component({
  selector: 'app-category-section',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatChipsModule,
    MatIconModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatTooltipModule,
    ProductCardComponent
  ],
  templateUrl: './category-section.component.html',
  styleUrl: './category-section.component.css'
})
export class CategorySectionComponent implements OnInit {
  @Input({ required: true }) category!: Category;
  @Output() categoryUpdated = new EventEmitter<Category>();

  private readonly productService = inject(ProductService);
  private readonly toastrService = inject(ToastrService);
  private readonly dialog = inject(MatDialog);

  isLoadingProducts = false;
  products: Product[] = [];

  ngOnInit(): void {
    this.loadProducts();
  }

  trackByProduct(index: number, product: Product): number {
    return product.id;
  }

  private loadProducts(): void {
    this.isLoadingProducts = true;

    this.productService.findByCategory(this.category.id).subscribe({
      next: (products: Product[]) => {
        this.products = products.filter(product => product.isActive).sort((a, b) => a.sortOrder - b.sortOrder);
        this.isLoadingProducts = false;

        if (this.products.length > 0) {
          console.log(`Productos cargados para ${this.category.name}:`, this.products.length);
        }
      },
      error: (error: any) => {
        this.handleError(error);
      }
    });
  }

  retryLoadProducts(): void {
    this.loadProducts();
  }

  // Método para añadir un nuevo producto
  showOption(operation: string): void {
    const dialogData = {
      operation: 'create',
      title: `Añadir Producto a ${this.category.name}`,
      category: this.category,
      disableClose: true
    };

    const dialogRef = this.dialog.open(ProductOptionComponent, {
      data: dialogData,
      width: '700px',
      maxWidth: '95vw',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.toastrService.success(
          `Producto "${result.name}" añadido a ${this.category.name}`, 
          'Producto Creado'
        );
        this.loadProducts(); // Recargar productos para mostrar el nuevo
        this.categoryUpdated.emit(this.category); // Notificar al componente padre
      }
    });
  }

  onProductUpdated(updatedProduct: Product): void {
    const index = this.products.findIndex(p => p.id === updatedProduct.id);
    if (index !== -1) {
      this.products[index] = updatedProduct;
      this.products.sort((a, b) => a.sortOrder - b.sortOrder);
    }
  }

  private handleError(error: any) {
    console.error('Error:', error);
    this.isLoadingProducts = false;
    
    if (error.error && error.error.message) {
      this.toastrService.error(`Error en ${this.category.name}: ${error.error.message}`);
    } else {
      this.toastrService.error(`Error al cargar productos de ${this.category.name}`);
    }
  }
}