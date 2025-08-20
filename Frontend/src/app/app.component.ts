import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./theme/header/header.component";

@Component({
  selector: 'app-root',
  template: `
  <app-header></app-header>
  <router-outlet />
  `,
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    HeaderComponent
]
})
export class AppComponent {
  title = 'Frontend';

}
