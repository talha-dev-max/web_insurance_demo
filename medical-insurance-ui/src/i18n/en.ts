export const EN_TRANSLATIONS = {
  // Header
  'header.medical_insurance': 'Medical Insurance',
  
  // Home page
  'home.search.placeholder': 'Enter Commercial Registration Number',
  'home.search.agreement': 'I agree to grant Shahin the right to inquire',
  'home.search.button': 'Search',
  'home.search.searching': 'Searching...',
  'home.features.compare': 'Compare',
  'home.features.secure': 'Secure',
  'home.features.print': 'Print your document',
  'home.create_new_company': 'Create New Company',
  
  // Messages
  'messages.company.found': 'Company found successfully!',
  'messages.company.not_found': 'Company not found. You can add new data.',
  'messages.search.error': 'An error occurred during search',
  'messages.validation.cr_required': 'Please enter a 10-digit number only.',
  'messages.validation.agreement_required': 'Please agree to grant Shahin the right to inquire.',
  'messages.save.success': 'Company information saved successfully!',
  'messages.save.error': 'An error occurred while saving',
  
  // Company Details
  'company.details.title': 'Company Information',
  'company.details.cr_number': 'Commercial Registration Number',
  'company.details.company_name_en': 'Company Name (English)',
  'company.details.company_name_ar': 'Company Name (Arabic)',
  'company.details.phone': 'Phone Number',
  'company.details.license_date': 'License Start Date',
  'company.details.classification': 'Classification',
  'company.details.save': 'Save',
  'company.details.saving': 'Saving...',
  'company.details.back': 'Back to Search',
  
  // Placeholders
  'placeholder.cr_number': '0123456789',
  'placeholder.company_name_en': 'Enter company name in English',
  'placeholder.company_name_ar': 'Enter company name in Arabic',
  'placeholder.phone': 'Enter mobile number (10 digits)',
  
  // Classification options
  'classification.less_than_3m': 'Less than 3 million SAR',
  'classification.more_than_3m': 'More than 3 million SAR',
  
  // Validation messages
  'validation.required': 'This field is required',
  'validation.cr_format': 'CR number must be exactly 10 digits',
  'validation.phone_format': 'Phone number must start with 05 and be 10 digits',
  'validation.name_min_length': 'Name must be at least 2 characters',
  
  // Navigation
  'nav.home': 'Home',
  'nav.language': 'العربية',
  
  // Footer
  'footer.copyright': 'All rights reserved © 2024 Shahin',
  'footer.contact_us': 'Contact Us',
  'footer.privacy_policy': 'Privacy Policy',
  'footer.terms_conditions': 'Terms & Conditions',
  'footer.about_us': 'About Us',
  'footer.follow_us': 'Follow us on social media',
  
  // Loading
  'loading.default': 'Loading...',
  'loading.searching': 'Searching...',
  'loading.saving': 'Saving...'
};

export type TranslationKey = keyof typeof EN_TRANSLATIONS;