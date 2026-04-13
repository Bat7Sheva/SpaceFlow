import { Component, OnInit, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { HomeService } from './services/home.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatProgressSpinnerModule, MatSnackBarModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  private readonly homeService = inject(HomeService);
  private readonly snackBar = inject(MatSnackBar);

  statusMessage = '';
  loading = false;

  ngOnInit(): void {
    this.loadStatus();
  }

  loadStatus(): void {
    this.loading = true;

    this.homeService.getApiStatus().subscribe({
      next: (response) => {
        this.statusMessage = response.data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.statusMessage = 'Unable to reach API';
        this.snackBar.open('API request failed', 'Close', { duration: 3000 });
      }
    });
  }
}
