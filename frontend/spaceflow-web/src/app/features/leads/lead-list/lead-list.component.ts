import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Router, RouterLink } from '@angular/router';
import { debounceTime } from 'rxjs';
import { LeadClient } from '../models/lead.model';
import { LeadService } from '../services/lead.service';

@Component({
  selector: 'app-lead-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './lead-list.component.html',
  styleUrl: './lead-list.component.scss'
})
export class LeadListComponent implements OnInit {
  private readonly leadService = inject(LeadService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly router = inject(Router);

  readonly searchControl = new FormControl('', { nonNullable: true });
  readonly statusControl = new FormControl('', { nonNullable: true });

  loading = false;
  leads: LeadClient[] = [];
  filteredLeads: LeadClient[] = [];

  ngOnInit(): void {
    this.loadLeads();

    this.searchControl.valueChanges.pipe(debounceTime(250)).subscribe(() => {
      this.applyFilters();
    });

    this.statusControl.valueChanges.subscribe(() => {
      this.applyFilters();
    });
  }

  get availableStatuses(): string[] {
    const statuses = this.leads.map((lead) => lead.status).filter(Boolean);
    return Array.from(new Set(statuses)).sort((a, b) => a.localeCompare(b));
  }

  loadLeads(): void {
    this.loading = true;

    this.leadService.getLeads().subscribe({
      next: (response) => {
        this.leads = response.data ?? [];
        this.applyFilters();
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('שגיאה בטעינת לידים', 'סגירה', { duration: 3000 });
      }
    });
  }

  goToCreate(): void {
    this.router.navigate(['/leads', 'new']);
  }

  private applyFilters(): void {
    const search = this.searchControl.value.trim().toLowerCase();
    const status = this.statusControl.value.trim().toLowerCase();

    this.filteredLeads = this.leads.filter((lead) => {
      const matchesSearch =
        search.length === 0 ||
        lead.fullName.toLowerCase().includes(search) ||
        lead.phone.toLowerCase().includes(search) ||
        (lead.email ?? '').toLowerCase().includes(search);

      const matchesStatus = status.length === 0 || lead.status.toLowerCase() === status;
      return matchesSearch && matchesStatus;
    });
  }
}
