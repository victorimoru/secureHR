namespace SecureHR.Core.Domains.EmployeeAggregate
{
    public class SalaryAdjustment
    {
        public Guid Id { get; private set; }
        public decimal NewSalary { get; private set; }
        public string Reason { get; private set; } 
        public DateTime EffectiveDate { get; private set; }
        internal SalaryAdjustment(decimal newSalary, string reason, DateTime effectiveDate)
        {
            if (newSalary <= 0) throw new ArgumentException("Salary must be positive.", nameof(newSalary));
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("A reason is required for a salary adjustment.", nameof(reason));

            Id = Guid.NewGuid();
            NewSalary = newSalary;
            Reason = reason;
            EffectiveDate = effectiveDate;
        }
    }
}
