using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_insurance_backend.Models
{
    /// <summary>
    /// Company entity model representing medical insurance company information
    /// </summary>
    [Table("Companies")]
    public class Company
    {
        /// <summary>
        /// Primary key identifier for the company
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Commercial Registration Number (CR Number) - unique identifier
        /// </summary>
        [Required]
        [StringLength(10, MinimumLength = 10)]
        [Column("CRNumber")]
        public string CrNumber { get; set; } = string.Empty;

        /// <summary>
        /// Company name in English
        /// </summary>
        [Required]
        [StringLength(255)]
        [Column("CompanyNameEn")]
        public string CompanyNameEn { get; set; } = string.Empty;

        /// <summary>
        /// Company name in Arabic
        /// </summary>
        [Required]
        [StringLength(255)]
        [Column("CompanyNameAr")]
        public string CompanyNameAr { get; set; } = string.Empty;

        /// <summary>
        /// Company phone number
        /// </summary>
        [Required]
        [StringLength(10, MinimumLength = 10)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// License start date for the insurance policy
        /// </summary>
        [Required]
        [Column("LicenseStartDate")]
        public DateTime LicenseStartDate { get; set; }

        /// <summary>
        /// Company classification for pricing purposes
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Classification { get; set; } = string.Empty;

        /// <summary>
        /// Record creation timestamp
        /// </summary>
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Record last update timestamp
        /// </summary>
        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property for company policies
        /// </summary>
        public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();
    }
}