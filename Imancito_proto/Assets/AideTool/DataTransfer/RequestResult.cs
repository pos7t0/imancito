namespace AideTool.DataTransfer
{
    public struct RequestResult
    {
        public bool IsOK { get; set; }
        public string Message { get; set; }
        public object Content { get; set; }

        public RequestResult(bool result, string msg, object content)
        {
            IsOK = result;
            Message = msg;
            Content = content;
        }

        public static RequestResult Ok() { return new RequestResult(true, "OK", null); }
        public static RequestResult Ok(object model) { return new RequestResult(true, "OK", model); }
        public static RequestResult Fail(string message) { return new RequestResult(false, message, null); }
    }
}
