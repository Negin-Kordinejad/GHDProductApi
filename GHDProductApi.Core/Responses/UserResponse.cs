namespace GHDProductApi.Core.Responses
{
    public class UserResponse
    {

        public UserResponse()
        {
            ErrorMessages = new List<ResponseError>();
        }

        public bool IsSuccessful => !ErrorMessages.Any();

        public IList<ResponseError> ErrorMessages { get; set; }

        public void AddError(string errorCode, string errorMessage)
        {
            ErrorMessages.Add(new ResponseError(errorCode, errorMessage));
        }

    }
}
