namespace LightNap.Core.Api
{
    public class ApiResponseDto<T>
    {
        public T? Result { get; set; }
        public ApiResponseType Type { get; set; }
        public List<string>? ErrorMessages { get; set; }

        public ApiResponseDto() { }

        private ApiResponseDto(ApiResponseType type)
        {
            this.Type = type;
        }

        public static ApiResponseDto<T> CreateSuccess(T? result)
        {
            return new ApiResponseDto<T>(ApiResponseType.Success)
            {
                Result = result,
            };
        }

        public static ApiResponseDto<T> CreateNonSuccess(ApiResponseType responseType)
        {
            if (responseType == ApiResponseType.Success) { throw new ArgumentException($"{nameof(ApiResponseDto<T>.CreateNonSuccess)} cannot be used for successful responses"); }

            return new ApiResponseDto<T>(responseType);
        }

        public static ApiResponseDto<T> CreateError(string error)
        {
            return ApiResponseDto<T>.CreateError([error]);
        }

        public static ApiResponseDto<T> CreateError(IEnumerable<string> errors)
        {
            return new ApiResponseDto<T>(ApiResponseType.Error)
            {
                ErrorMessages = errors.ToList(),
            };
        }

        public static ApiResponseDto<T> CreateUnhandledError(string error)
        {
            return ApiResponseDto<T>.CreateUnhandledError([error]);
        }

        public static ApiResponseDto<T> CreateUnhandledError(IEnumerable<string> errors)
        {
            return new ApiResponseDto<T>(ApiResponseType.UnhandledError)
            {
                ErrorMessages = errors.ToList()
            };
        }
    }
}
