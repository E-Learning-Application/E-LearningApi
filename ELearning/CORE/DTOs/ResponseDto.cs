namespace CORE.DTOs
{
    public class ResponseDto<T>
    {
        public T Data { get; set; } // if statuscode is success and the you don't want to attach any data, use object type
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
