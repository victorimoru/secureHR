namespace SecureHR.Shared.Models
{
    public class SecurityDetails
    {
        public SecurityLogType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public enum SecurityLogType
    {
        SuccessfulLogin,
        MFAChallenge,
        ViewedDocument,
        ExportedReport
    }
}
