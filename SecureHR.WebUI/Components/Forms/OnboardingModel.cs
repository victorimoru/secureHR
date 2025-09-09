using System.ComponentModel.DataAnnotations;

namespace SecureHR.WebUI.Components.Forms
{
    public class OnboardingModel
    {
        public EmployeeDetails Employee { get; set; } = new();
        public JobDetails Job { get; set; } = new();
        public DocumentDetails Documents { get; set; } = new();

        public OnboardingModel()
        {
            // Initialize with default values if necessary
        }
    }

    /// <summary>
    /// Basic personal and contact information for the employee.
    /// </summary>
    public class EmployeeDetails
    {
        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Personal email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? PersonalEmail { get; set; }
    }

    /// <summary>
    /// Details about the employee's role and compensation.
    /// </summary>
    public class JobDetails
    {
        [Required(ErrorMessage = "Job title is required.")]
        public string? JobTitle { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime? StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Please select a department.")]
        public string? Department { get; set; }

        [Required(ErrorMessage = "Please select a manager.")]
        public string? Manager { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(30000, 500000, ErrorMessage = "Salary must be between $30,000 and $500,000.")]
        public decimal? AnnualSalary { get; set; }

        public string Qualification { get; set; }

        public string ReasonForHire { get; set; }
        
        public string? EmploymentType { get; set; } = "Full-Time";
    }

    /// <summary>
    /// Placeholder for document upload information.
    /// </summary>
    public class DocumentDetails
    {
        public bool OfferLetterSigned { get; set; }
        public bool I9FormCompleted { get; set; }
        public bool W4FormCompleted { get; set; }
    }
}
