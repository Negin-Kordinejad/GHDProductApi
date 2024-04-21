namespace GHDProductApi.Core.Responses
{
    public class Response<T> : UserResponse
    {
        public T Data { get; set; }
    }
}
