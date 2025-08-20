import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { ToastrService } from 'ngx-toastr';

import { CategorySectionComponent } from './category-section/category-section.component';
import { CategoryService } from '../../../core/services/category.service';
import { Category } from '../../../core/interfaces/category.interface';
import { MatDialog } from '@angular/material/dialog';
import { ProductOptionComponent } from './product-option/product-option.component';
import { MatButtonModule } from "@angular/material/button";

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule,
    CategorySectionComponent,
    MatButtonModule
],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit {
  private readonly categoryService = inject(CategoryService);
  private readonly toastrService = inject(ToastrService);

  constructor(
    private dialog: MatDialog,
  ) { }

  isLoading = false;
  categories: Category[] = [];

  ngOnInit(): void {
    this.loadCategories();
  }

  trackByCategory(index: number, item: Category): number {
    return item.id;
  }

  addProduct(): void {
    if (this.categories.length === 0) {
      this.toastrService.warning('No hay categorías disponibles. Primero crea una categoría.', 'Sin Categorías');
      return;
    }

    const dialogData = {
      operation: 'create',
      title: 'Añadir Nuevo Producto',
      categories: this.categories,
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
          `Producto "${result.name}" creado correctamente`, 
          'Producto Creado'
        );
        
        this.refreshCategorySections();
      }
    });
  }

  private refreshCategorySections(): void {
    const currentCategories = [...this.categories];
    this.categories = [];
    
    setTimeout(() => {
      this.categories = currentCategories;
    }, 100);
  }

  private loadCategories(): void {
    this.isLoading = true;

    this.categoryService.findAll().subscribe({
      next: (categories: Category[]) => {
        this.categories = categories.filter(cat => cat.isActive).sort((a, b) => a.sortOrder - b.sortOrder);
        this.isLoading = false;

        if (this.categories.length > 0) {
          this.toastrService.success('Categorías cargadas correctamente');
        }
      },
      error: (error: any) => {
        this.handleError(error);
      }
    });
  }

  private handleError(error: any) {
    console.error('Error:', error);
    if (error.error && error.error.message) {
      this.toastrService.error(error.error.message);
    } else {
      this.toastrService.error('Error al procesar la solicitud');
    }
  }
}