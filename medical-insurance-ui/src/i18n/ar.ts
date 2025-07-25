import { EN_TRANSLATIONS, TranslationKey } from './en';

export const AR_TRANSLATIONS: Record<TranslationKey, string> = {
  // Header
  'header.medical_insurance': 'التأمين الطبي',
  
  // Home page
  'home.search.placeholder': 'أدخل رقم السجل التجاري',
  'home.search.agreement': 'أوافق على منح شاهين حق الاستعلام',
  'home.search.button': 'بحث',
  'home.search.searching': 'جاري البحث...',
  'home.features.compare': 'قارن',
  'home.features.secure': 'أمن',
  'home.features.print': 'اطبع وثيقتك',
  'home.create_new_company': 'إنشاء شركة جديدة',
  
  // Messages
  'messages.company.found': 'تم العثور على الشركة بنجاح!',
  'messages.company.not_found': 'لم يتم العثور على الشركة. يمكنك إضافة بيانات جديدة.',
  'messages.search.error': 'حدث خطأ أثناء البحث',
  'messages.validation.cr_required': 'الرجاء إدخال رقم مكون من 10 أرقام فقط.',
  'messages.validation.agreement_required': 'يرجى الموافقة على منح شاهين حق الاستعلام.',
  'messages.save.success': 'تم حفظ بيانات الشركة بنجاح!',
  'messages.save.error': 'حدث خطأ أثناء الحفظ',
  
  // Company Details
  'company.details.title': 'بيانات الشركة',
  'company.details.cr_number': 'رقم السجل التجاري',
  'company.details.company_name_en': 'اسم الشركة (إنجليزي)',
  'company.details.company_name_ar': 'اسم الشركة (عربي)',
  'company.details.phone': 'رقم الهاتف',
  'company.details.license_date': 'تاريخ بداية الترخيص',
  'company.details.classification': 'التصنيف',
  'company.details.save': 'حفظ',
  'company.details.saving': 'جاري الحفظ...',
  'company.details.back': 'العودة للبحث',
  
  // Placeholders
  'placeholder.cr_number': '0123456789',
  'placeholder.company_name_en': 'ادخل اسم الشركة بالانجليزي',
  'placeholder.company_name_ar': 'ادخل اسم الشركة بالعربي',
  'placeholder.phone': 'أدخل رقم الجوال (10 أرقام)',
  
  // Classification options
  'classification.less_than_3m': 'أقل من 3 ملايين ريال',
  'classification.more_than_3m': 'اكثر من 3 ملايين ريال',
  
  // Validation messages
  'validation.required': 'هذا الحقل مطلوب',
  'validation.cr_format': 'رقم السجل التجاري يجب أن يكون 10 أرقام بالضبط',
  'validation.phone_format': 'رقم الهاتف يجب أن يبدأ بـ 05 ويكون 10 أرقام',
  'validation.name_min_length': 'الاسم يجب أن يكون على الأقل حرفين',
  
  // Navigation
  'nav.home': 'الرئيسية',
  'nav.language': 'English',
  
  // Footer
  'footer.copyright': 'جميع الحقوق محفوظة © 2024 شاهين',
  'footer.contact_us': 'تواصل معنا',
  'footer.privacy_policy': 'سياسة الخصوصية',
  'footer.terms_conditions': 'الشروط والأحكام',
  'footer.about_us': 'من نحن',
  'footer.follow_us': 'تابعنا عبر شبكات التواصل الاجتماعية',
  
  // Loading
  'loading.default': 'جاري التحميل...',
  'loading.searching': 'جاري البحث...',
  'loading.saving': 'جاري الحفظ...'
};