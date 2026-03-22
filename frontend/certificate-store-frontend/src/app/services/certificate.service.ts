import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CertificateService {
  private baseUrl = '/api/MeasurementResults';

  constructor(private http: HttpClient) {}

  getMeasurementResults(pageNumber: number, pageSize: number): Observable<any> {
    return this.http.get(`${this.baseUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  getMeasurementResultById(id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/${id}`);
  }

  createMeasurementResult(data: any): Observable<any> {
    return this.http.post(this.baseUrl, data);
  }

  updateMeasurementResult(id: string, data: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, data);
  }

  deleteMeasurementResult(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}