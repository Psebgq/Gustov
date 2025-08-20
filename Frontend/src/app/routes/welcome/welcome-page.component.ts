import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './welcome-page.component.html',
  styleUrl: './welcome-page.component.css'
})
export class WelcomePageComponent {
  welcomeMessage = '¡Bienvenido a nuestro sistema de punto de venta!';
  
  currentTime = new Date();

  constructor(
    private router: Router
  ) {
  }

  navigateToMenu(): void {
    this.router.navigate(['/menu']);
  }

  navigateToPOS(): void {
    // La navegación se maneja por el routerLink en el template
    console.log('Navegando al POS...');
  }
}