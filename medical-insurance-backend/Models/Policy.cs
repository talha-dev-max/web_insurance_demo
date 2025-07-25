using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_insurance_backend.Models
{
    /// <summary>
    /// Policy entity model representing medical insurance policies
    /// </summary>
    [Table("Policies")]
    public class Policy
    {
        /// <summary>
        /// Primary key identifier for the policy
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to Company
        /// </summary>
        [Required]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        /// <summary>
        /// Policy number (unique identifier)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string PolicyNumber { get; set; } = string.Empty;

        /// <summary>
        /// Policy start date
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Policy end date
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Policy premium amount
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PremiumAmount { get; set; }

        /// <summary>
        /// Policy status (Active, Inactive, Expired)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

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
        /// Navigation property to Company
        /// </summary>
        public virtual Company Company { get; set; } = null!;
    }
}