using medical_insurance_backend.DTOs;
using medical_insurance_backend.Models;

namespace medical_insurance_backend.Services.Interfaces
{
    /// <summary>
    /// Interface for company-related business operations
    /// Defines contract for company service implementations
    /// Follows Interface Segregation Principle (ISP)
    /// </summary>
    public interface ICompanyService
    {
        /// <summary>
        /// Search for a company by CR Number
        /// </summary>
        /// <param name="crNumber">Commercial Registration Number</param>
        /// <returns>Company information if found, null otherwise</returns>
        Task<CompanyResponseDto?> GetCompanyByCrNumberAsync(string crNumber);

        /// <summary>
        /// Create a new company record
        /// </summary>
        /// <param name="companyDto">Company creation data</param>
        /// <returns>Created company information</returns>
        Task<CompanyResponseDto> CreateCompanyAsync(CompanyCreateRequestDto companyDto);

        /// <summary>
        /// Update an existing company record
        /// </summary>
        /// <param name="id">Company ID</param>
        /// <param name="companyDto">Updated company data</param>
        /// <returns>Updated company information</returns>
        Task<CompanyResponseDto?> UpdateCompanyAsync(int id, CompanyCreateRequestDto companyDto);

        /// <summary>
        /// Check if a company exists by CR Number
        /// </summary>
        /// <param name="crNumber">Commercial Registration Number</param>
        /// <returns>True if company exists, false otherwise</returns>
        Task<bool> CompanyExistsAsync(string crNumber);

        /// <summary>
        /// Get all companies with pagination
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>List of companies</returns>
        Task<IEnumerable<CompanyResponseDto>> GetAllCompaniesAsync(int pageNumber = 1, int pageSize = 10);
    }
}