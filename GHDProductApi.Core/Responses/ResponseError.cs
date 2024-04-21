namespace GHDProductApi.Core.Responses
{
    public class ResponseError
    {
        public ResponseError() { }

        public ResponseError(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }


        public static IList<ResponseError> UnexpectedError()
        {
            return new List<ResponseError>
            {
                new()
                {
                    ErrorCode = ResponseErrorCodeConstants.UnexpectedError,
                    ErrorMessage = "An unexpected error occurred."
                }
            };
        }
    }
}
