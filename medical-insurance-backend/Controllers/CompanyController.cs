using Microsoft.AspNetCore.Mvc;
using medical_insurance_backend.DTOs;
using medical_insurance_backend.Services.Interfaces;

namespace medical_insurance_backend.Controllers
{
    /// <summary>
    /// API Controller for company-related operations
    /// Handles HTTP requests for company management
    /// Follows REST principles and proper error handling
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="companyService">Company service interface</param>
        /// <param name="logger">Logger instance</param>
        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Search for a company by CR Number
        /// </summary>
        /// <param name="request">Search request containing CR Number</param>
        /// <returns>Company information if found</returns>
        /// <response code="200">Company found successfully</response>
        /// <response code="404">Company not found</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("search")]
        [ProducesResponseType(typeof(ApiResponse<CompanyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchCompany([FromBody] CompanySearchRequestDto request)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Invalid model state for company search: {Errors}", string.Join(", ", errors));

                    return BadRequest(new ApiErrorResponse
                    {
                        Message = "Invalid input data",
                        Errors = errors,
                        StatusCode = 400
                    });
                }

                _logger.LogInformation("Searching for company with CR Number: {CrNumber}", request.CrNumber);

                var company = await _companyService.GetCompanyByCrNumberAsync(request.CrNumber);

                if (company == null)
                {
                    _logger.LogWarning("Company not found with CR Number: {CrNumber}", request.CrNumber);
                    
                    return NotFound(new ApiErrorResponse
                    {
                        Message = $"Company with CR Number {request.CrNumber} not found",
                        StatusCode = 404
                    });
                }

                _logger.LogInformation("Company found successfully with CR Number: {CrNumber}", request.CrNumber);

                return Ok(new ApiResponse<CompanyResponseDto>
                {
                    Success = true,
                    Message = "Company found successfully",
                    Data = company,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for company with CR Number: {CrNumber}", request?.CrNumber);

                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "An internal server error occurred",
                    StatusCode = 500
                });
            }
        }

        /// <summary>
        /// Create or update a company record
        /// </summary>
        /// <param name="request">Company creation/update request</param>
        /// <returns>Created or updated company information</returns>
        /// <response code="200">Company updated successfully</response>
        /// <response code="201">Company created successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="409">Company already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("save")]
        [ProducesResponseType(typeof(ApiResponse<CompanyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CompanyResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveCompany([FromBody] CompanyCreateRequestDto request)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Invalid model state for company save: {Errors}", string.Join(", ", errors));

                    return BadRequest(new ApiErrorResponse
                    {
                        Message = "Invalid input data",
                        Errors = errors,
                        StatusCode = 400
                    });
                }

                _logger.LogInformation("Saving company with CR Number: {CrNumber}", request.CrNumber);

                // Check if company exists
                var existingCompany = await _companyService.GetCompanyByCrNumberAsync(request.CrNumber);

                if (existingCompany != null)
                {
                    // Update existing company
                    var updatedCompany = await _companyService.UpdateCompanyAsync(existingCompany.Id, request);
                    
                    _logger.LogInformation("Company updated successfully with CR Number: {CrNumber}", request.CrNumber);

                    return Ok(new ApiResponse<CompanyResponseDto>
                    {
                        Success = true,
                        Message = "Company updated successfully",
                        Data = updatedCompany,
                        StatusCode = 200
                    });
                }
                else
                {
                    // Create new company
                    var newCompany = await _companyService.CreateCompanyAsync(request);
                    
                    _logger.LogInformation("Company created successfully with CR Number: {CrNumber}", request.CrNumber);

                    return StatusCode(201, new ApiResponse<CompanyResponseDto>
                    {
                        Success = true,
                        Message = "Company created successfully",
                        Data = newCompany,
                        StatusCode = 201
                    });
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business logic error while saving company: {CrNumber}", request?.CrNumber);

                return Conflict(new ApiErrorResponse
                {
                    Message = ex.Message,
                    StatusCode = 409
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving company with CR Number: {CrNumber}", request?.CrNumber);

                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "An internal server error occurred",
                    StatusCode = 500
                });
            }
        }

        /// <summary>
        /// Get all companies with pagination
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10, max: 100)</param>
        /// <returns>List of companies</returns>
        /// <response code="200">Companies retrieved successfully</response>
        /// <response code="400">Invalid pagination parameters</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CompanyResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCompanies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate pagination parameters
                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                {
                    _logger.LogWarning("Invalid pagination parameters: PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);

                    return BadRequest(new ApiErrorResponse
                    {
                        Message = "Invalid pagination parameters. Page number must be >= 1 and page size must be between 1 and 100",
                        StatusCode = 400
                    });
                }

                _logger.LogInformation("Retrieving companies with pagination: PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);

                var companies = await _companyService.GetAllCompaniesAsync(pageNumber, pageSize);

                _logger.LogInformation("Retrieved {Count} companies", companies.Count());

                return Ok(new ApiResponse<IEnumerable<CompanyResponseDto>>
                {
                    Success = true,
                    Message = "Companies retrieved successfully",
                    Data = companies,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving companies");

                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "An internal server error occurred",
                    StatusCode = 500
                });
            }
        }

        /// <summary>
        /// Check if a company exists by CR Number
        /// </summary>
        /// <param name="crNumber">Commercial Registration Number</param>
        /// <returns>Boolean indicating if company exists</returns>
        /// <response code="200">Check completed successfully</response>
        /// <response code="400">Invalid CR Number format</response>
        /// <response code="500">Internal server error</response>
        [HttpHead("{crNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CompanyExists(string crNumber)
        {
            try
            {
                // Validate CR Number format
                if (string.IsNullOrWhiteSpace(crNumber) || !System.Text.RegularExpressions.Regex.IsMatch(crNumber, @"^\d{10}$"))
                {
                    _logger.LogWarning("Invalid CR Number format: {CrNumber}", crNumber);

                    return BadRequest(new ApiErrorResponse
                    {
                        Message = "CR Number must be exactly 10 digits",
                        StatusCode = 400
                    });
                }

                _logger.LogInformation("Checking if company exists with CR Number: {CrNumber}", crNumber);

                var exists = await _companyService.CompanyExistsAsync(crNumber);

                // Use HTTP status codes to indicate existence
                // 200 OK = exists, 404 Not Found = doesn't exist
                return exists ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking company existence with CR Number: {CrNumber}", crNumber);

                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "An internal server error occurred",
                    StatusCode = 500
                });
            }
        }
    }
}