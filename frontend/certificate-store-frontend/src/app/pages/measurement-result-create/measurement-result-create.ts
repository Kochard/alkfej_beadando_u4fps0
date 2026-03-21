import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CertificateService } from '../../services/certificate.service';

@Component({
  selector: 'app-measurement-result-create',
  imports: [CommonModule, FormsModule],
  templateUrl: './measurement-result-create.html',
  styleUrl: './measurement-result-create.css'
})
export class MeasurementResultCreateComponent {
  formData = {
    partName: '',
    serialNumber: '',
    measurementType: '',
    measuredValue: 0,
    unit: '',
    lowerLimit: 0,
    upperLimit: 0,
    status: '',
    measuredBy: '',
    measuredAt: '',
    notes: ''
  };

  errorMessage = '';
  successMessage = '';

  constructor(
    private certificateService: CertificateService,
    private router: Router
  ) {}

  createResult() {
    this.errorMessage = '';
    this.successMessage = '';

    const payload = {
      ...this.formData,
      measuredAt: this.formData.measuredAt
        ? new Date(this.formData.measuredAt).toISOString()
        : new Date().toISOString()
    };

    console.log('Create payload:', payload);

    this.certificateService.createMeasurementResult(payload).subscribe({
      next: (response) => {
        console.log('Create success:', response);
        this.successMessage = 'Measurement result created successfully.';
        this.router.navigate(['/measurement-results']);
      },
      error: (error) => {
        console.error('Create error:', error);
        this.errorMessage = 'Failed to create measurement result.';
      }
    });
  }
}