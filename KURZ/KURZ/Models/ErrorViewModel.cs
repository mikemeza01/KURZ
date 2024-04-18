namespace KURZ.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class Error
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
}