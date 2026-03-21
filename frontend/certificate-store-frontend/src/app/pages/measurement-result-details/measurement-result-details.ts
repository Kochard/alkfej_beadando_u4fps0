import { Component, OnInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CertificateService } from '../../services/certificate.service';

@Component({
  selector: 'app-measurement-result-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './measurement-result-details.html',
  styleUrl: './measurement-result-details.css'
})
export class MeasurementResultDetailsComponent implements OnInit {

  result: any = null;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private certificateService: CertificateService,
    private ngZone: NgZone,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.certificateService.getMeasurementResultById(id).subscribe({
        next: (data) => {
          console.log('DETAIL DATA:', data);

          this.ngZone.run(() => {
            this.result = data;
            this.cdr.detectChanges();
          });
        },
        error: (err) => {
          console.error('DETAIL ERROR:', err);

          this.ngZone.run(() => {
            this.errorMessage = 'Failed to load measurement result details.';
            this.cdr.detectChanges();
          });
        }
      });
    }
  }
}