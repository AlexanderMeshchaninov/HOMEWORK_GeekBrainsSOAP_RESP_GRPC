namespace ClinicService.Core
{
    public interface IOperationResult
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
