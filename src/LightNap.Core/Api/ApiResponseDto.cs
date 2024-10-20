namespace LightNap.Core.Api
{
    /// <summary>
    /// Represents the response of an API call.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class ApiResponseDto<T>
    {
        /// <summary>
        /// Gets or sets the result of the API call.
        /// </summary>
        public T? Result { get; set; }

        /// <summary>
        /// Gets or sets the type of the API response.
        /// </summary>
        public ApiResponseType Type { get; set; }

        /// <summary>
        /// Gets or sets the error messages associated with the API response.
        /// </summary>
        public List<string>? ErrorMessages { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseDto{T}"/> class.
        /// </summary>
        public ApiResponseDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseDto{T}"/> class with the specified response type.
        /// </summary>
        /// <param name="type">The response type.</param>
        private ApiResponseDto(ApiResponseType type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Creates a success API response with the specified result.
        /// </summary>
        /// <param name="result">The result of the API call.</param>
        /// <returns>An instance of <see cref="ApiResponseDto{T}"/> representing a success response.</returns>
        public static ApiResponseDto<T> CreateSuccess(T? result)
        {
            return new ApiResponseDto<T>(ApiResponseType.Success)
            {
                Result = result,
            };
        }

        /// <summary>
        /// Creates an error API response with the specified error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>An instance of <see cref="ApiResponseDto{T}"/> representing an error response.</returns>
        public static ApiResponseDto<T> CreateError(string error)
        {
            return ApiResponseDto<T>.CreateError([error]);
        }

        /// <summary>
        /// Creates an error API response with the specified error messages.
        /// </summary>
        /// <param name="errors">The error messages.</param>
        /// <returns>An instance of <see cref="ApiResponseDto{T}"/> representing an error response.</returns>
        public static ApiResponseDto<T> CreateError(IEnumerable<string> errors)
        {
            return new ApiResponseDto<T>(ApiResponseType.Error)
            {
                ErrorMessages = errors.ToList(),
            };
        }

        /// <summary>
        /// Creates an unexpected error API response with the specified error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>An instance of <see cref="ApiResponseDto{T}"/> representing an unexpected error response.</returns>
        public static ApiResponseDto<T> CreateUnexpectedError(string error)
        {
            return ApiResponseDto<T>.CreateUnexpectedError([error]);
        }

        /// <summary>
        /// Creates an unexpected error API response with the specified error messages.
        /// </summary>
        /// <param name="errors">The error messages.</param>
        /// <returns>An instance of <see cref="ApiResponseDto{T}"/> representing an unexpected error response.</returns>
        public static ApiResponseDto<T> CreateUnexpectedError(IEnumerable<string> errors)
        {
            return new ApiResponseDto<T>(ApiResponseType.UnexpectedError)
            {
                ErrorMessages = errors.ToList()
            };
        }
    }
}
