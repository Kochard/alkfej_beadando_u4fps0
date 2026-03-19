import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CertificateService } from '../../services/certificate.service';

@Component({
  selector: 'app-user-certificates',
  imports: [CommonModule],
  templateUrl: './user-certificates.html',
  styleUrl: './user-certificates.css'
})
export class UserCertificatesComponent implements OnInit {

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
      .getUserCertificates(this.pageNumber, this.pageSize)
      .subscribe({
        next: (data) => {
          this.certificates = data;
        },
        error: () => {
          this.errorMessage = 'Failed to load user certificates from backend.';
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