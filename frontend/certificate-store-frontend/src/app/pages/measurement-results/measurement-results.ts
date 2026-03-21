import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CertificateService } from '../../services/certificate.service';

@Component({
  selector: 'app-measurement-results',
  imports: [CommonModule],
  templateUrl: './measurement-results.html',
  styleUrl: './measurement-results.css'
})
export class MeasurementResultsComponent implements OnInit {

  results: any[] = [];
  pageNumber = 1;
  pageSize = 5;
  errorMessage = '';

  constructor(private certificateService: CertificateService) {}

  ngOnInit(): void {
    this.loadResults();
  }

  loadResults() {
    this.errorMessage = '';

    this.certificateService
      .getMeasurementResults(this.pageNumber, this.pageSize)
      .subscribe({
        next: (data) => {
          this.results = data;
        },
        error: () => {
          this.errorMessage = 'Failed to load measurement results from backend.';
          this.results = [];
        }
      });
  }

  nextPage() {
    this.pageNumber++;
    this.loadResults();
  }

  prevPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadResults();
    }
  }

  reload() {
    this.loadResults();
  }
}