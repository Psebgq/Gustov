import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil, forkJoin } from 'rxjs';
import { SaleService } from '../../../core/services/sale.service';
import { OrderItemService } from '../../../core/services/order-item.service';
import { Sale } from '../../../core/interfaces/sale.interface';


interface FilterOptions {
  dateFrom: Date | null;
  dateTo: Date | null;
  minAmount: number | null;
  maxAmount: number | null;
  searchText: string;
}

@Component({
  selector: 'app-sale-report',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    FormsModule
  ],
  templateUrl: './sale-report.component.html',
  styleUrl: './sale-report.component.css'
})
export class SaleReportComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();
  private readonly saleService = inject(SaleService);
  private readonly orderItemService = inject(OrderItemService);
  private readonly toastrService = inject(ToastrService);

  isLoading = true;
  allSales: Sale[] = [];
  filteredSales: Sale[] = [];
  
  // Filtros
  filters: FilterOptions = {
    dateFrom: null,
    dateTo: null,
    minAmount: null,
    maxAmount: null,
    searchText: ''
  };

  // Estadísticas calculadas
  totalSales = 0;
  totalRevenue = 0;
  averageSaleAmount = 0;
  
  // Columnas para la tabla
  displayedColumns: string[] = ['id', 'date', 'items', 'subtotal', 'tip', 'total', 'cash', 'change', 'actions'];

  ngOnInit(): void {
    this.loadSalesData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadSalesData(): void {
    this.isLoading = true;

    this.saleService.findAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (sales) => {
          this.allSales = sales.sort((a, b) => 
            new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
          );
          // Inicialmente mostrar todas las ventas
          this.filteredSales = [...this.allSales];
          this.calculateStatistics();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error al cargar datos de ventas:', error);
          this.toastrService.error('Error al cargar los datos de ventas', 'Error');
          this.isLoading = false;
        }
      });
  }

  applyFilters(): void {
    this.filteredSales = this.allSales.filter(sale => {
      // Filtro por fecha
      if (this.filters.dateFrom) {
        const saleDate = new Date(sale.createdAt);
        if (saleDate < this.filters.dateFrom) return false;
      }
      
      if (this.filters.dateTo) {
        const saleDate = new Date(sale.createdAt);
        const dateTo = new Date(this.filters.dateTo);
        dateTo.setHours(23, 59, 59, 999); // Incluir todo el día
        if (saleDate > dateTo) return false;
      }

      // Filtro por monto mínimo
      if (this.filters.minAmount !== null && sale.total < this.filters.minAmount) {
        return false;
      }

      // Filtro por monto máximo
      if (this.filters.maxAmount !== null && sale.total > this.filters.maxAmount) {
        return false;
      }

      // Filtro por texto (busca en ID o items)
      if (this.filters.searchText) {
        const searchLower = this.filters.searchText.toLowerCase();
        const saleId = sale.id.toString();
        const itemNames = sale.orderItems?.map(item => item.name.toLowerCase()).join(' ') || '';
        
        if (!saleId.includes(searchLower) && !itemNames.includes(searchLower)) {
          return false;
        }
      }

      return true;
    });

    this.calculateStatistics();
  }

  onSearchFilters(): void {
    this.applyFilters();
    this.toastrService.info('Filtros aplicados', 'Búsqueda');
  }

  private calculateStatistics(): void {
    this.totalSales = this.filteredSales.length;
    this.totalRevenue = this.filteredSales.reduce((sum, sale) => sum + sale.total, 0);
    this.averageSaleAmount = this.totalSales > 0 ? this.totalRevenue / this.totalSales : 0;
  }

  onRefreshReport(): void {
    this.loadSalesData();
    this.toastrService.info('Reporte actualizado', 'Actualización');
  }

  onClearFilters(): void {
    this.filters = {
      dateFrom: null,
      dateTo: null,
      minAmount: null,
      maxAmount: null,
      searchText: ''
    };
    // Mostrar todas las ventas cuando se limpian los filtros
    this.filteredSales = [...this.allSales];
    this.calculateStatistics();
    this.toastrService.info('Filtros limpiados', 'Filtros');
  }

  onViewSaleDetails(sale: Sale): void {
    console.log('Ver detalles de venta:', sale);
    this.toastrService.info(`Mostrando detalles de venta #${sale.id}`, 'Detalles');
  }

  getTotalItems(sale: Sale): number {
    return sale.orderItems?.reduce((sum, item) => sum + item.quantity, 0) || 0;
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('es-US', {
      style: 'currency',
      currency: 'BOB',
      minimumFractionDigits: 2
    }).format(amount);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  formatDateTime(dateString: string): string {
    return new Date(dateString).toLocaleString('es-ES', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  get hasData(): boolean {
    return this.filteredSales.length > 0;
  }

  get hasActiveFilters(): boolean {
    return this.filters.dateFrom !== null ||
           this.filters.dateTo !== null ||
           this.filters.minAmount !== null ||
           this.filters.maxAmount !== null ||
           this.filters.searchText.trim() !== '';
  }
}