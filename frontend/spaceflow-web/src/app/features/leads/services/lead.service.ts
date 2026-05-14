import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { ApiResponse } from '../../../models/api-response.model';
import {
  CreateInteractionRequest,
  CreateLeadClientRequest,
  Interaction,
  LeadClient,
  UpdateLeadClientRequest
} from '../models/lead.model';

@Injectable({
  providedIn: 'root'
})
export class LeadService {
  private readonly apiService = inject(ApiService);

  getLeads(search?: string, status?: string): Observable<ApiResponse<LeadClient[]>> {
    return this.apiService.get<ApiResponse<LeadClient[]>>('/api/leads', { search, status });
  }

  getTodayLeads(): Observable<ApiResponse<LeadClient[]>> {
    return this.apiService.get<ApiResponse<LeadClient[]>>('/api/leads/today');
  }

  getLeadById(id: number): Observable<ApiResponse<LeadClient>> {
    return this.apiService.get<ApiResponse<LeadClient>>(`/api/leads/${id}`);
  }

  createLead(payload: CreateLeadClientRequest): Observable<ApiResponse<LeadClient>> {
    return this.apiService.post<ApiResponse<LeadClient>>('/api/leads', payload);
  }

  updateLead(id: number, payload: UpdateLeadClientRequest): Observable<ApiResponse<LeadClient>> {
    return this.apiService.put<ApiResponse<LeadClient>>(`/api/leads/${id}`, payload);
  }

  deleteLead(id: number): Observable<ApiResponse<null>> {
    return this.apiService.delete<ApiResponse<null>>(`/api/leads/${id}`);
  }

  addInteraction(id: number, payload: CreateInteractionRequest): Observable<ApiResponse<Interaction>> {
    return this.apiService.post<ApiResponse<Interaction>>(`/api/leads/${id}/interactions`, payload);
  }
}
