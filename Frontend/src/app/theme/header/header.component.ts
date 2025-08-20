// src/app/theme/header/header.component.ts
import { Component, ViewEncapsulation, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatBadgeModule } from '@angular/material/badge';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Observable } from 'rxjs';
import { CartService } from '../../shared/services/cart.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
  host: {
    class: 'matero-header',
  },
  encapsulation: ViewEncapsulation.None,
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatBadgeModule,
    MatTooltipModule
  ],
})
export class HeaderComponent {
  restaurantName = 'Gustov';
  
  private readonly cartService = inject(CartService);
  private readonly router = inject(Router);
  
  // Observables del carrito
  totalItems$: Observable<number> = this.cartService.totalItems$;
  totalAmount$: Observable<number> = this.cartService.totalAmount$;

  constructor() {
    this.totalItems$.subscribe(totalItems => {
      if (totalItems > 0) {
        this.addCartButtonEffect();
      }
    });
  }
  goToSale(): void {
    this.router.navigate(['/sale']);
  }

  goHome(): void {
    this.router.navigate(['/']);
  }

  private addCartButtonEffect(): void {
    const cartButton = document.querySelector('.cart-button');
    if (cartButton) {
      cartButton.classList.add('has-items');
    }
  }
}