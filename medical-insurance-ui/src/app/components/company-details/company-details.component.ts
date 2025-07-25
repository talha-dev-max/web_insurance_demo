import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CompanyService, CompanyCreateRequest, CompanyResponse } from '../../services/company.service';
import { TranslationService } from '../../services/translation.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

@Component({
  selector: 'app-company-details',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './company-details.component.html',
  styleUrl: './company-details.component.scss'
})
export class CompanyDetailsComponent implements OnInit {
  // Form data
  companyData: CompanyCreateRequest = {
    crNumber: '',
    companyNameEn: '',
    companyNameAr: '',
    phoneNumber: '',
    licenseStartDate: '',
    classification: 'أقل من 3 ملايين ريال'
  };

  // Component state
  isExisting: boolean = false;
  isLoading: boolean = false;
  message: string = '';
  messageType: 'success' | 'error' = 'error';

  // Classification options - will be populated from translation service
  classificationOptions: { value: string; label: string }[] = [];

  constructor(
    private companyService: CompanyService,
    private router: Router,
    public translationService: TranslationService
  ) {
    // Get data from navigation state
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras?.state || (typeof window !== 'undefined' ? window.history.state : null);

    if (state) {
      if (state.company) {
        // Existing company data
        this.isExisting = true;
        this.populateFormWithExistingData(state.company);
      } else if (state.crNumber) {
        // New company with CR number from home
        this.companyData.crNumber = state.crNumber;
      }
    }
  }

  ngOnInit(): void {
    // Initialize classification options from translation service
    this.classificationOptions = this.translationService.getClassificationOptions();
    
    // Set default date to today
    if (!this.companyData.licenseStartDate) {
      const today = new Date();
      this.companyData.licenseStartDate = today.toISOString().split('T')[0];
    }

  }

  /**
   * Populate form with existing company data
   * @param company Existing company data
   */
  private populateFormWithExistingData(company: CompanyResponse): void {
    this.companyData = {
      crNumber: company.crNumber,
      companyNameEn: company.companyNameEn,
      companyNameAr: company.companyNameAr,
      phoneNumber: company.phoneNumber,
      licenseStartDate: new Date(company.licenseStartDate).toISOString().split('T')[0],
      classification: company.classification
    };
  }

  /**
   * Handle form submission
   * @param event Form submission event
   */
  onSubmit(event: Event): void {
    event.preventDefault();
    this.clearMessage();

    // Validate form
    if (!this.validateForm()) {
      return;
    }

    this.isLoading = true;

    // Prepare data for API
    const submitData: CompanyCreateRequest = {
      ...this.companyData,
      licenseStartDate: this.companyService.formatDateForApi(this.companyData.licenseStartDate)
    };

    // Submit to API
    this.companyService.saveCompany(submitData).subscribe({
      next: (response: CompanyResponse) => {
        this.isLoading = false;
        this.showSuccessMessage(
          this.isExisting ? 'تم تحديث بيانات الشركة بنجاح!' : 'تم حفظ بيانات الشركة بنجاح!'
        );
        
        // Navigate back to home after 2 seconds
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 2000);
      },
      error: (error: any) => {
        this.isLoading = false;
        this.showErrorMessage(error.message || 'حدث خطأ أثناء حفظ البيانات');
      }
    });
  }

  /**
   * Handle CR number input changes
   */
  onCrNumberChange(): void {
    // Remove non-numeric characters and limit to 10 digits
    this.companyData.crNumber = this.companyData.crNumber.replace(/[^0-9]/g, '');
    if (this.companyData.crNumber.length > 10) {
      this.companyData.crNumber = this.companyData.crNumber.slice(0, 10);
    }
    this.clearMessage();
  }

  /**
   * Handle phone number input changes
   */
  onPhoneNumberChange(): void {
    // Remove non-numeric characters and limit to 10 digits
    this.companyData.phoneNumber = this.companyData.phoneNumber.replace(/[^0-9]/g, '');
    if (this.companyData.phoneNumber.length > 10) {
      this.companyData.phoneNumber = this.companyData.phoneNumber.slice(0, 10);
    }
    this.clearMessage();
  }

  /**
   * Handle English company name input changes
   */
  onCompanyNameEnChange(): void {
    // Allow English letters, spaces, numbers, and common company symbols
    this.companyData.companyNameEn = this.companyData.companyNameEn.replace(/[^a-zA-Z0-9\s&\-\.\(\)]/g, '');
    this.clearMessage();
  }

  /**
   * Handle Arabic company name input changes
   */
  onCompanyNameArChange(): void {
    // Allow Arabic letters, spaces, numbers, and common company symbols
    this.companyData.companyNameAr = this.companyData.companyNameAr.replace(/[^\u0600-\u06FF\s0-9&\-\.\(\)]/g, '');
    this.clearMessage();
  }

  /**
   * Navigate back to home
   */
  goBack(): void {
    this.router.navigate(['/']);
  }

  /**
   * Validate form data
   * @returns true if valid, false otherwise
   */
  private validateForm(): boolean {
    // Validate CR Number
    if (!this.companyService.isValidCrNumber(this.companyData.crNumber)) {
      this.showErrorMessage('رقم السجل التجاري يجب أن يكون 10 أرقام بالضبط');
      return false;
    }

    // Validate Company Name EN
    if (!this.companyData.companyNameEn || !this.companyData.companyNameEn.trim()) {
      this.showErrorMessage('اسم الشركة بالإنجليزية مطلوب');
      return false;
    }

    // Validate Company Name AR
    if (!this.companyData.companyNameAr || !this.companyData.companyNameAr.trim()) {
      this.showErrorMessage('اسم الشركة بالعربية مطلوب');
      return false;
    }

    // Validate Phone Number
    // if (!this.companyService.isValidPhoneNumber(this.companyData.phoneNumber)) {
    //   this.showErrorMessage('رقم الجوال يجب أن يبدأ بـ 05 ويكون 10 أرقام');
    //   return false;
    // }

    // Validate License Start Date
    if (!this.companyData.licenseStartDate) {
      this.showErrorMessage('تاريخ بداية الوثيقة مطلوب');
      return false;
    }

    // Validate Classification
    if (!this.companyData.classification) {
      this.showErrorMessage('إعدادات المنشأة مطلوبة');
      return false;
    }

    return true;
  }

  /**
   * Show success message
   * @param message Message to display
   */
  private showSuccessMessage(message: string): void {
    this.message = message;
    this.messageType = 'success';
  }

  /**
   * Show error message
   * @param message Message to display
   */
  private showErrorMessage(message: string): void {
    this.message = message;
    this.messageType = 'error';
  }

  /**
   * Clear current message
   */
  private clearMessage(): void {
    this.message = '';
  }
}
