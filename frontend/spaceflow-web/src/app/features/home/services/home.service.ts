import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { ApiResponse } from '../../../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private readonly apiService = inject(ApiService);

  getApiStatus(): Observable<ApiResponse<string>> {
    return this.apiService.get<ApiResponse<string>>('/api/Test'); 
  }
}
