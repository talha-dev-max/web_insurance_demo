import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/home/home.component').then(m => m.HomeComponent)
  },
  {
    path: 'company-details',
    loadComponent: () => import('./components/company-details/company-details.component').then(m => m.CompanyDetailsComponent)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
