import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CompanyService, CompanyResponse } from '../../services/company.service';
import { TranslationService } from '../../services/translation.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  crNumber: string = '';
  agreed: boolean = false;
  message: string = '';
  messageType: 'success' | 'error' = 'error';
  isLoading: boolean = false;
  showCreateButton: boolean = false;

  constructor(
    private companyService: CompanyService,
    private router: Router,
    public translationService: TranslationService
  ) {}

  /**
   * Handle form submission for CR number search
   * @param event Form submission event
   */
  onSubmit(event: Event): void {
    event.preventDefault();
    this.clearMessage();

    // Validate input
    if (!this.validateInput()) {
      return;
    }

    this.isLoading = true;
    console.log(this.crNumber, "===============")
    
    // Search for company
    this.companyService.searchCompany(this.crNumber).subscribe({
      next: (company: CompanyResponse | null) => {
        this.isLoading = false;
        
        if (company) {
          // Company found - navigate to company details with data
          this.showSuccessMessage(this.translationService.translate('messages.company.found'));
          setTimeout(() => {
            this.router.navigate(['/company-details'], { 
              state: { company, isExisting: true }
            });
          }, 1000);
        } else {
          // Company not found - show message and offer to create new
          this.showErrorMessage(this.translationService.translate('messages.company.not_found'));
          this.showCreateButton = true;
        }
      },
      error: (error: any) => {
        console.log(error)
        this.isLoading = false;
        this.showErrorMessage(error.message || this.translationService.translate('messages.search.error'));
      }
    });
  }

  /**
   * Handle input changes for CR number field
   * Restricts input to numbers only and limits to 10 digits
   */
  onCrNumberChange(): void {
    // Remove non-numeric characters
    this.crNumber = this.crNumber.replace(/[^0-9]/g, '');
    
    // Limit to 10 digits
    if (this.crNumber.length > 10) {
      this.crNumber = this.crNumber.slice(0, 10);
    }
    
    // Clear message when user starts typing
    this.clearMessage();
  }

  /**
   * Validate form input
   * @returns true if valid, false otherwise
   */
  private validateInput(): boolean {
    // Trim whitespace and ensure clean input
    this.crNumber = this.crNumber.trim();
    
    // Check if CR number is exactly 10 digits
    console.log(this.crNumber, "ssssssssssss")
    if (!this.companyService.isValidCrNumber(this.crNumber)) {
      console.log('Validation failed for CR:', this.crNumber, 'Length:', this.crNumber.length);
      this.showErrorMessage(this.translationService.translate('messages.validation.cr_required'));
      return false;
    }

    // Check if user agreed to terms
    if (!this.agreed) {
      this.showErrorMessage(this.translationService.translate('messages.validation.agreement_required'));
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
    this.showCreateButton = false;
  }

  /**
   * Navigate to create new company page
   */
  createNewCompany(): void {
    this.router.navigate(['/company-details'], { 
      state: { crNumber: this.crNumber, isExisting: false }
    });
  }
}
