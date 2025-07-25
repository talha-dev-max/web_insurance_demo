namespace medical_insurance_backend.DTOs
{
    /// <summary>
    /// Data Transfer Object for company response
    /// Contains company information returned to the client
    /// </summary>
    public class CompanyResponseDto
    {
        /// <summary>
        /// Company unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Commercial Registration Number (CR Number)
        /// </summary>
        public string CrNumber { get; set; } = string.Empty;

        /// <summary>
        /// Company name in English
        /// </summary>
        public string CompanyNameEn { get; set; } = string.Empty;

        /// <summary>
        /// Company name in Arabic
        /// </summary>
        public string CompanyNameAr { get; set; } = string.Empty;

        /// <summary>
        /// Company phone number
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// License start date for the insurance policy
        /// </summary>
        public DateTime LicenseStartDate { get; set; }

        /// <summary>
        /// Company classification for pricing purposes
        /// </summary>
        public string Classification { get; set; } = string.Empty;

        /// <summary>
        /// Record creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Record last update timestamp
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Standard API response wrapper for success cases
    /// </summary>
    /// <typeparam name="T">Type of data being returned</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Response data
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// HTTP status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Timestamp of the response
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Standard API response wrapper for error cases
    /// </summary>
    public class ApiErrorResponse
    {
        /// <summary>
        /// Indicates if the operation was successful (always false for errors)
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// List of validation errors (if applicable)
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// HTTP status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Timestamp of the error response
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}