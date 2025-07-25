using System.ComponentModel.DataAnnotations;

namespace medical_insurance_backend.DTOs
{
    /// <summary>
    /// Data Transfer Object for company search request
    /// Used when client searches for company by CR Number
    /// </summary>
    public class CompanySearchRequestDto
    {
        /// <summary>
        /// Commercial Registration Number (CR Number)
        /// Must be exactly 10 digits
        /// </summary>
        [Required(ErrorMessage = "CR Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "CR Number must be exactly 10 digits")]
        public string CrNumber { get; set; } = string.Empty;
    }

    /// <summary>
    /// Data Transfer Object for company creation/update request
    /// Contains all company information from the form
    /// </summary>
    public class CompanyCreateRequestDto
    {
        /// <summary>
        /// Commercial Registration Number (CR Number)
        /// Must be exactly 10 digits
        /// </summary>
        [Required(ErrorMessage = "CR Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "CR Number must be exactly 10 digits")]
        public string CrNumber { get; set; } = string.Empty;

        /// <summary>
        /// Company name in English
        /// </summary>
        [Required(ErrorMessage = "Company name in English is required")]
        [StringLength(255, ErrorMessage = "Company name cannot exceed 255 characters")]
        public string CompanyNameEn { get; set; } = string.Empty;

        /// <summary>
        /// Company name in Arabic
        /// </summary>
        [Required(ErrorMessage = "Company name in Arabic is required")]
        [StringLength(255, ErrorMessage = "Company name cannot exceed 255 characters")]
        public string CompanyNameAr { get; set; } = string.Empty;

        /// <summary>
        /// Company phone number
        /// </summary>
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// License start date for the insurance policy
        /// </summary>
        [Required(ErrorMessage = "License start date is required")]
        public DateTime LicenseStartDate { get; set; }

        /// <summary>
        /// Company classification for pricing purposes
        /// </summary>
        [Required(ErrorMessage = "Classification is required")]
        public string Classification { get; set; } = string.Empty;
    }
}