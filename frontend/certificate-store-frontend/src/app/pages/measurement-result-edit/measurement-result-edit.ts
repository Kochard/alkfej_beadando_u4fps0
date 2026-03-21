import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CertificateService } from '../../services/certificate.service';

@Component({
  selector: 'app-measurement-result-edit',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './measurement-result-edit.html',
  styleUrl: './measurement-result-edit.css'
})
export class MeasurementResultEditComponent implements OnInit {
  id = '';
  returnPage = 1;
  isReady = false;

  formData: any = {
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

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private certificateService: CertificateService,
    private ngZone: NgZone,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const pageParam = this.route.snapshot.queryParamMap.get('page');
    this.returnPage = pageParam ? Number(pageParam) : 1;

    const id = this.route.snapshot.paramMap.get('id');
    
    if (id) {
      this.id = id;

      this.certificateService.getMeasurementResultById(id).subscribe({
        next: (data) => {
          this.ngZone.run(() => {
            this.formData = {
              partName: data.partName ?? '',
              serialNumber: data.serialNumber ?? '',
              measurementType: data.measurementType ?? '',
              measuredValue: data.measuredValue ?? 0,
              unit: data.unit ?? '',
              lowerLimit: data.lowerLimit ?? 0,
              upperLimit: data.upperLimit ?? 0,
              status: data.status ?? '',
              measuredBy: data.measuredBy ?? '',
              measuredAt: data.measuredAt
                ? new Date(data.measuredAt).toISOString().slice(0, 16)
                : '',
              notes: data.notes ?? ''
            };

            this.isReady = true;
            this.cdr.detectChanges();
          });
        },
        error: () => {
          this.errorMessage = 'Failed to load measurement result for editing.';
          this.isReady = true;
          this.cdr.detectChanges();
        }
      });
    } else {
      this.errorMessage = 'Missing measurement result ID.';
      this.isReady = true;
    }
  }

  updateResult() {
    this.errorMessage = '';

    const payload = {
      ...this.formData,
      measuredAt: this.formData.measuredAt
        ? new Date(this.formData.measuredAt).toISOString()
        : new Date().toISOString()
    };

    this.certificateService.updateMeasurementResult(this.id, payload).subscribe({
      next: () => {
        this.router.navigate(['/measurement-results'], {
          queryParams: { page: this.returnPage }
        });
      },
      error: () => {
        this.errorMessage = 'Failed to update measurement result.';
      }
    });
  }

  cancel() {
    this.router.navigate(['/measurement-results'], {
      queryParams: { page: this.returnPage }
    });
  }
}