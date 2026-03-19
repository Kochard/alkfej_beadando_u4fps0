import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CertificateService {

  private baseUrl = 'http://localhost:5202/api';

  constructor(private http: HttpClient) {}

  getRootCertificates(pageNumber: number, pageSize: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/RootCertificates?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  getUserCertificates(pageNumber: number, pageSize: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/UserCertificates?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
}