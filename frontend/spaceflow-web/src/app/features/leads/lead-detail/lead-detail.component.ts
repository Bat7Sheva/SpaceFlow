import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { LeadClient } from '../models/lead.model';
import { LeadService } from '../services/lead.service';

@Component({
  selector: 'app-lead-detail',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './lead-detail.component.html',
  styleUrl: './lead-detail.component.scss'
})
export class LeadDetailComponent implements OnInit {
  private readonly leadService = inject(LeadService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);
  private readonly fb = inject(FormBuilder);

  readonly leadForm = this.fb.nonNullable.group({
    fullName: ['', [Validators.required, Validators.maxLength(200)]],
    phone: ['', [Validators.required, Validators.maxLength(50)]],
    email: ['', [Validators.email, Validators.maxLength(200)]],
    source: ['', [Validators.required, Validators.maxLength(100)]],
    status: ['', [Validators.required, Validators.maxLength(50)]],
    nextFollowUpAt: [''],
    notes: ['', [Validators.maxLength(2000)]]
  });

  readonly interactionForm = this.fb.nonNullable.group({
    channel: ['', [Validators.required, Validators.maxLength(50)]],
    summary: ['', [Validators.required, Validators.maxLength(1000)]],
    interactionAt: [''],
    nextFollowUpAt: ['']
  });

  leadId: number | null = null;
  loading = false;
  saving = false;
  addingInteraction = false;
  lead: LeadClient | null = null;

  ngOnInit(): void {
    const routeId = this.route.snapshot.paramMap.get('id');

    if (routeId && routeId !== 'new') {
      this.leadId = Number(routeId);
      this.loadLead(this.leadId);
      return;
    }

    this.interactionForm.disable();
  }

  saveLead(): void {
    if (this.leadForm.invalid || this.saving) {
      this.leadForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    const payload = {
      ...this.leadForm.getRawValue(),
      nextFollowUpAt: this.toNullableIso(this.leadForm.getRawValue().nextFollowUpAt),
      email: this.toNullableString(this.leadForm.getRawValue().email),
      notes: this.toNullableString(this.leadForm.getRawValue().notes)
    };

    if (this.leadId === null) {
      this.leadService.createLead(payload).subscribe({
        next: (response) => {
          this.saving = false;
          this.snackBar.open('ליד נוצר בהצלחה', 'סגירה', { duration: 2500 });
          this.router.navigate(['/leads', response.data.id]);
        },
        error: () => {
          this.saving = false;
          this.snackBar.open('שגיאה בשמירת ליד', 'סגירה', { duration: 3000 });
        }
      });
      return;
    }

    this.leadService.updateLead(this.leadId, payload).subscribe({
      next: (response) => {
        this.saving = false;
        this.lead = response.data;
        this.patchLeadForm(response.data);
        this.snackBar.open('הליד עודכן בהצלחה', 'סגירה', { duration: 2500 });
      },
      error: () => {
        this.saving = false;
        this.snackBar.open('שגיאה בעדכון ליד', 'סגירה', { duration: 3000 });
      }
    });
  }

  deleteLead(): void {
    if (this.leadId === null || this.saving) {
      return;
    }

    const shouldDelete = window.confirm('למחוק את הליד?');
    if (!shouldDelete) {
      return;
    }

    this.saving = true;
    this.leadService.deleteLead(this.leadId).subscribe({
      next: () => {
        this.saving = false;
        this.snackBar.open('הליד נמחק', 'סגירה', { duration: 2500 });
        this.router.navigate(['/leads']);
      },
      error: () => {
        this.saving = false;
        this.snackBar.open('שגיאה במחיקת ליד', 'סגירה', { duration: 3000 });
      }
    });
  }

  addInteraction(): void {
    if (this.leadId === null || this.interactionForm.invalid || this.addingInteraction) {
      this.interactionForm.markAllAsTouched();
      return;
    }

    this.addingInteraction = true;
    const payload = {
      ...this.interactionForm.getRawValue(),
      interactionAt:
        this.toNullableIso(this.interactionForm.getRawValue().interactionAt) ?? new Date().toISOString(),
      nextFollowUpAt: this.toNullableIso(this.interactionForm.getRawValue().nextFollowUpAt)
    };

    this.leadService.addInteraction(this.leadId, payload).subscribe({
      next: () => {
        this.addingInteraction = false;
        this.interactionForm.reset({
          channel: '',
          summary: '',
          interactionAt: '',
          nextFollowUpAt: ''
        });
        this.snackBar.open('אינטראקציה נוספה', 'סגירה', { duration: 2500 });
        this.loadLead(this.leadId!);
      },
      error: () => {
        this.addingInteraction = false;
        this.snackBar.open('שגיאה בהוספת אינטראקציה', 'סגירה', { duration: 3000 });
      }
    });
  }

  private loadLead(id: number): void {
    this.loading = true;

    this.leadService.getLeadById(id).subscribe({
      next: (response) => {
        this.loading = false;
        this.lead = response.data;
        this.patchLeadForm(response.data);
        this.interactionForm.enable();
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('שגיאה בטעינת ליד', 'סגירה', { duration: 3000 });
        this.router.navigate(['/leads']);
      }
    });
  }

  private patchLeadForm(lead: LeadClient): void {
    this.leadForm.patchValue({
      fullName: lead.fullName,
      phone: lead.phone,
      email: lead.email ?? '',
      source: lead.source,
      status: lead.status,
      nextFollowUpAt: this.toInputValue(lead.nextFollowUpAt),
      notes: lead.notes ?? ''
    });
  }

  private toInputValue(value: string | null | undefined): string {
    if (!value) {
      return '';
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '';
    }

    const local = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
    return local.toISOString().slice(0, 16);
  }

  private toNullableIso(value: string): string | null {
    if (!value?.trim()) {
      return null;
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return null;
    }

    return date.toISOString();
  }

  private toNullableString(value: string): string | null {
    const normalized = value?.trim();
    return normalized ? normalized : null;
  }
}
