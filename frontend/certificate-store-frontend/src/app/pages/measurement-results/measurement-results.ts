import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { CertificateService } from '../../services/certificate.service';

@Component({
  selector: 'app-measurement-results',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './measurement-results.html',
  styleUrl: './measurement-results.css'
})
export class MeasurementResultsComponent implements OnInit {
  results: any[] = [];
  getStatus(result: any): string {
    if (result.measuredValue >= result.lowerLimit && result.measuredValue <= result.upperLimit) {
      return 'PASS';
    }
    return 'NG';
  }
  pageNumber = 1;
  pageSize = 1;
  totalPages = 1;
  totalCount = 0;
  errorMessage = '';
  isLoading = false;

  constructor(
    private certificateService: CertificateService,
    private ngZone: NgZone,
    private cdr: ChangeDetectorRef,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const pageParam = this.route.snapshot.queryParamMap.get('page');
    this.pageNumber = pageParam ? Number(pageParam) : 1;
    
    this.loadResults();
  }

  loadResults() {
    this.errorMessage = '';
    this.isLoading = true;
    this.cdr.detectChanges();

    this.certificateService
      .getMeasurementResults(this.pageNumber, this.pageSize)
      .subscribe({
        next: (data) => {
          console.log('Measurement list data:', data);

          this.ngZone.run(() => {
            this.results = data.items ?? [];
            this.pageNumber = data.pageNumber ?? 1;
            this.pageSize = data.pageSize ?? 1;
            this.totalCount = data.totalCount ?? 0;
            this.totalPages = data.totalPages ?? 1;
            this.isLoading = false;
            this.errorMessage = '';
            this.cdr.detectChanges();
          });
        },
        error: (error) => {
          console.error('Measurement list error:', error);

          this.ngZone.run(() => {
            this.errorMessage = 'Failed to load measurement results from backend.';
            this.results = [];
            this.isLoading = false;
            this.cdr.detectChanges();
          });
        }
      });
  }

  nextPage() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadResults();
    }
  }

  prevPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadResults();
    }
  }

  firstPage() {
    if (this.pageNumber !== 1) {
      this.pageNumber = 1;
      this.loadResults();
    }
  }

  lastPage() {
    if (this.pageNumber !== this.totalPages) {
      this.pageNumber = this.totalPages;
      this.loadResults();
    }
  }

  reload() {
    this.loadResults();
  }

  deleteResult(id: string) {
    const confirmed = window.confirm('Are you sure you want to delete this measurement result?');

    if (!confirmed) {
      return;
    }

    this.certificateService.deleteMeasurementResult(id).subscribe({
      next: () => {
        this.ngZone.run(() => {
          if (this.pageNumber > 1 && this.results.length === 1) {
            this.pageNumber--;
          }

          this.loadResults();
        });
      },
      error: (error) => {
        console.error('Delete error:', error);

        this.ngZone.run(() => {
          this.errorMessage = 'Failed to delete measurement result.';
          this.cdr.detectChanges();
        });
      }
    });
  }
}