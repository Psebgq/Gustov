import { Routes } from '@angular/router';
import { WelcomePageComponent } from './routes/welcome/welcome-page.component';

export const routes: Routes = [
    {
        path: '',
        component: WelcomePageComponent
    },
    { path: 'menu', loadChildren: () => import('./routes/menu/menu.routes').then(m => m.routes) },
    { path: 'sale', loadChildren: () => import('./routes/sale/sale.route').then(m => m.routes) }
];
