import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CertificateService } from '../../services/certificate.service';

@Component({
  selector: 'app-root-certificates',
  imports: [CommonModule],
  templateUrl: './root-certificates.html',
  styleUrl: './root-certificates.css'
})
export class RootCertificatesComponent implements OnInit {

  certificates: any[] = [];
  pageNumber = 1;
  pageSize = 5;
  errorMessage = '';

  constructor(private certificateService: CertificateService) {}

  ngOnInit(): void {
    this.loadCertificates();
  }

  loadCertificates() {
    this.errorMessage = '';

    this.certificateService
      .getRootCertificates(this.pageNumber, this.pageSize)
      .subscribe({
        next: (data) => {
          this.certificates = data;
        },
        error: () => {
          this.errorMessage = 'Failed to load root certificates from backend.';
          this.certificates = [];
        }
      });
  }

  nextPage() {
    this.pageNumber++;
    this.loadCertificates();
  }

  prevPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadCertificates();
    }
  }

  reload() {
    this.loadCertificates();
  }
}