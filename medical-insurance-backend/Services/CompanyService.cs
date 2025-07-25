using Microsoft.EntityFrameworkCore;
using medical_insurance_backend.Data;
using medical_insurance_backend.DTOs;
using medical_insurance_backend.Models;
using medical_insurance_backend.Services.Interfaces;

namespace medical_insurance_backend.Services
{
    /// <summary>
    /// Implementation of company-related business operations
    /// Follows Single Responsibility Principle (SRP) and Dependency Inversion Principle (DIP)
    /// </summary>
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompanyService> _logger;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        public CompanyService(ApplicationDbContext context, ILogger<CompanyService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Search for a company by CR Number
        /// </summary>
        /// <param name="crNumber">Commercial Registration Number</param>
        /// <returns>Company information if found, null otherwise</returns>
        public async Task<CompanyResponseDto?> GetCompanyByCrNumberAsync(string crNumber)
        {
            try
            {
                _logger.LogInformation("Searching for company with CR Number: {CrNumber}", crNumber);

                var company = await _context.Companies
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CrNumber == crNumber);

                if (company == null)
                {
                    _logger.LogWarning("Company not found with CR Number: {CrNumber}", crNumber);
                    return null;
                }

                _logger.LogInformation("Company found with CR Number: {CrNumber}", crNumber);
                return MapToResponseDto(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for company with CR Number: {CrNumber}", crNumber);
                throw;
            }
        }

        /// <summary>
        /// Create a new company record
        /// </summary>
        /// <param name="companyDto">Company creation data</param>
        /// <returns>Created company information</returns>
        public async Task<CompanyResponseDto> CreateCompanyAsync(CompanyCreateRequestDto companyDto)
        {
            try
            {
                _logger.LogInformation("Creating new company with CR Number: {CrNumber}", companyDto.CrNumber);

                // Check if company already exists
                var existingCompany = await _context.Companies
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CrNumber == companyDto.CrNumber);

                if (existingCompany != null)
                {
                    _logger.LogWarning("Company already exists with CR Number: {CrNumber}", companyDto.CrNumber);
                    throw new InvalidOperationException($"Company with CR Number {companyDto.CrNumber} already exists");
                }

                var company = MapToEntity(companyDto);
                
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Company created successfully with ID: {CompanyId}", company.Id);
                return MapToResponseDto(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating company with CR Number: {CrNumber}", companyDto.CrNumber);
                throw;
            }
        }

        /// <summary>
        /// Update an existing company record
        /// </summary>
        /// <param name="id">Company ID</param>
        /// <param name="companyDto">Updated company data</param>
        /// <returns>Updated company information</returns>
        public async Task<CompanyResponseDto?> UpdateCompanyAsync(int id, CompanyCreateRequestDto companyDto)
        {
            try
            {
                _logger.LogInformation("Updating company with ID: {CompanyId}", id);

                var company = await _context.Companies.FindAsync(id);
                if (company == null)
                {
                    _logger.LogWarning("Company not found with ID: {CompanyId}", id);
                    return null;
                }

                // Check if CR Number is being changed and if it conflicts with another company
                if (company.CrNumber != companyDto.CrNumber)
                {
                    var existingCompany = await _context.Companies
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CrNumber == companyDto.CrNumber && c.Id != id);

                    if (existingCompany != null)
                    {
                        _logger.LogWarning("Cannot update company. CR Number {CrNumber} already exists", companyDto.CrNumber);
                        throw new InvalidOperationException($"Company with CR Number {companyDto.CrNumber} already exists");
                    }
                }

                // Update company properties
                UpdateEntityFromDto(company, companyDto);
                
                await _context.SaveChangesAsync();

                _logger.LogInformation("Company updated successfully with ID: {CompanyId}", company.Id);
                return MapToResponseDto(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating company with ID: {CompanyId}", id);
                throw;
            }
        }

        /// <summary>
        /// Check if a company exists by CR Number
        /// </summary>
        /// <param name="crNumber">Commercial Registration Number</param>
        /// <returns>True if company exists, false otherwise</returns>
        public async Task<bool> CompanyExistsAsync(string crNumber)
        {
            try
            {
                _logger.LogInformation("Checking if company exists with CR Number: {CrNumber}", crNumber);

                var exists = await _context.Companies
                    .AsNoTracking()
                    .AnyAsync(c => c.CrNumber == crNumber);

                _logger.LogInformation("Company exists check result for CR Number {CrNumber}: {Exists}", crNumber, exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking company existence with CR Number: {CrNumber}", crNumber);
                throw;
            }
        }

        /// <summary>
        /// Get all companies with pagination
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>List of companies</returns>
        public async Task<IEnumerable<CompanyResponseDto>> GetAllCompaniesAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Retrieving companies - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

                var companies = await _context.Companies
                    .AsNoTracking()
                    .OrderBy(c => c.CompanyNameEn)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var responseDtos = companies.Select(MapToResponseDto).ToList();

                _logger.LogInformation("Retrieved {Count} companies", responseDtos.Count);
                return responseDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving companies");
                throw;
            }
        }

        /// <summary>
        /// Map Company entity to CompanyResponseDto
        /// </summary>
        /// <param name="company">Company entity</param>
        /// <returns>Company response DTO</returns>
        private static CompanyResponseDto MapToResponseDto(Company company)
        {
            return new CompanyResponseDto
            {
                Id = company.Id,
                CrNumber = company.CrNumber,
                CompanyNameEn = company.CompanyNameEn,
                CompanyNameAr = company.CompanyNameAr,
                PhoneNumber = company.PhoneNumber,
                LicenseStartDate = company.LicenseStartDate,
                Classification = company.Classification,
                CreatedAt = company.CreatedAt,
                UpdatedAt = company.UpdatedAt
            };
        }

        /// <summary>
        /// Map CompanyCreateRequestDto to Company entity
        /// </summary>
        /// <param name="dto">Company create request DTO</param>
        /// <returns>Company entity</returns>
        private static Company MapToEntity(CompanyCreateRequestDto dto)
        {
            return new Company
            {
                CrNumber = dto.CrNumber,
                CompanyNameEn = dto.CompanyNameEn,
                CompanyNameAr = dto.CompanyNameAr,
                PhoneNumber = dto.PhoneNumber,
                LicenseStartDate = dto.LicenseStartDate,
                Classification = dto.Classification
            };
        }

        /// <summary>
        /// Update Company entity from CompanyCreateRequestDto
        /// </summary>
        /// <param name="company">Company entity to update</param>
        /// <param name="dto">Company update data</param>
        private static void UpdateEntityFromDto(Company company, CompanyCreateRequestDto dto)
        {
            company.CrNumber = dto.CrNumber;
            company.CompanyNameEn = dto.CompanyNameEn;
            company.CompanyNameAr = dto.CompanyNameAr;
            company.PhoneNumber = dto.PhoneNumber;
            company.LicenseStartDate = dto.LicenseStartDate;
            company.Classification = dto.Classification;
        }
    }
}