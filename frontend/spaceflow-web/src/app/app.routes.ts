import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'leads',
    loadComponent: () => import('./features/leads/lead-list/lead-list.component').then((m) => m.LeadListComponent)
  },
  {
    path: 'leads/:id',
    loadComponent: () =>
      import('./features/leads/lead-detail/lead-detail.component').then((m) => m.LeadDetailComponent)
  },
  {
    path: 'today',
    loadComponent: () => import('./features/leads/today/today.component').then((m) => m.TodayComponent)
  },
  { path: '', redirectTo: 'leads', pathMatch: 'full' },
  { path: '**', redirectTo: 'leads' }
];
