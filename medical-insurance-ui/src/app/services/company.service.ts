import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

// Interface for API response wrapper
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  statusCode: number;
  timestamp: string;
}

// Interface for API error response
export interface ApiErrorResponse {
  success: boolean;
  message: string;
  errors: string[];
  statusCode: number;
  timestamp: string;
}

// Interface for company search request
export interface CompanySearchRequest {
  crNumber: string;
}

// Interface for company create/update request
export interface CompanyCreateRequest {
  crNumber: string;
  companyNameEn: string;
  companyNameAr: string;
  phoneNumber: string;
  licenseStartDate: string; // ISO date string
  classification: string;
}

// Interface for company response data
export interface CompanyResponse {
  id: number;
  crNumber: string;
  companyNameEn: string;
  companyNameAr: string;
  phoneNumber: string;
  licenseStartDate: string;
  classification: string;
  createdAt: string;
  updatedAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private readonly apiUrl = 'http://localhost:5011/api/company';

  constructor(private http: HttpClient) { }

  /**
   * Search for a company by CR Number
   * @param crNumber Commercial Registration Number (10 digits)
   * @returns Observable with company data or null if not found
   */
  searchCompany(crNumber: string): Observable<CompanyResponse | null> {
    const request: CompanySearchRequest = { crNumber };
    
    return this.http.post<ApiResponse<CompanyResponse>>(`${this.apiUrl}/search`, request)
      .pipe(
        map(response => response.data),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            // Company not found - return null instead of throwing error
            return [null];
          }
          return this.handleError(error);
        })
      );
  }

  /**
   * Save (create or update) company information
   * @param companyData Company information to save
   * @returns Observable with saved company data
   */
  saveCompany(companyData: CompanyCreateRequest): Observable<CompanyResponse> {
    return this.http.post<ApiResponse<CompanyResponse>>(`${this.apiUrl}/save`, companyData)
      .pipe(
        map(response => response.data),
        catchError(this.handleError)
      );
  }

  /**
   * Get all companies with pagination
   * @param pageNumber Page number (default: 1)
   * @param pageSize Items per page (default: 10)
   * @returns Observable with list of companies
   */
  getAllCompanies(pageNumber: number = 1, pageSize: number = 10): Observable<CompanyResponse[]> {
    const params = { pageNumber: pageNumber.toString(), pageSize: pageSize.toString() };
    
    return this.http.get<ApiResponse<CompanyResponse[]>>(this.apiUrl, { params })
      .pipe(
        map(response => response.data || []),
        catchError(this.handleError)
      );
  }

  /**
   * Check if a company exists by CR Number
   * @param crNumber Commercial Registration Number
   * @returns Observable with boolean indicating existence
   */
  companyExists(crNumber: string): Observable<boolean> {
    return this.http.head(`${this.apiUrl}/${crNumber}`, { observe: 'response' })
      .pipe(
        map(response => response.status === 200),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            return [false];
          }
          return this.handleError(error);
        })
      );
  }

  /**
   * Validate CR Number format
   * @param crNumber CR Number to validate
   * @returns true if valid, false otherwise
   */
  isValidCrNumber(crNumber: string): boolean {
    const crPattern = /^\d{10}$/;
    return crPattern.test(crNumber);
  }

  /**
   * Validate phone number format (Saudi format)
   * @param phoneNumber Phone number to validate
   * @returns true if valid, false otherwise
   */
  isValidPhoneNumber(phoneNumber: string): boolean {
    const phonePattern = /^05\d{8}$/;
    return phonePattern.test(phoneNumber);
  }

  /**
   * Format date for API submission
   * @param date Date object or date string
   * @returns ISO date string
   */
  formatDateForApi(date: Date | string): string {
    if (typeof date === 'string') {
      return new Date(date).toISOString();
    }
    return date.toISOString();
  }

  /**
   * Handle HTTP errors
   * @param error HTTP error response
   * @returns Observable that throws formatted error
   */
  private handleError = (error: HttpErrorResponse): Observable<never> => {
    let errorMessage = 'An unknown error occurred';
    let errorDetails: string[] = [];

    if (error.error) {
      // API error response
      if (error.error.message) {
        errorMessage = error.error.message;
      }
      if (error.error.errors && Array.isArray(error.error.errors)) {
        errorDetails = error.error.errors;
      }
    } else if (error.message) {
      // HTTP error
      errorMessage = error.message;
    }

    // Log error for debugging
    console.error('API Error:', {
      status: error.status,
      message: errorMessage,
      details: errorDetails,
      url: error.url
    });

    // Return user-friendly error message
    const userError = {
      message: errorMessage,
      details: errorDetails,
      status: error.status
    };

    return throwError(() => userError);
  };
}