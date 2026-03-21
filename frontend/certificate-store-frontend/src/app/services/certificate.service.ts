import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CertificateService {

  private baseUrl = 'http://localhost:5202/api/MeasurementResults';

  constructor(private http: HttpClient) {}

  getMeasurementResults(pageNumber: number, pageSize: number): Observable<any> {
    return this.http.get(`${this.baseUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
}