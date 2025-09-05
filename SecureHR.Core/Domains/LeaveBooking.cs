namespace SecureHR.Core.Domains
{
    public class LeaveBooking
    {
        public Guid Id { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsCancelled { get; private set; }

        internal LeaveBooking(DateTime startDate, DateTime endDate)
        {
            if (startDate < DateTime.UtcNow.Date)
                throw new ArgumentException("Leave start date cannot be in the past.");
            if (endDate < startDate)
                throw new ArgumentException("Leave end date must be after the start date.");

            Id = Guid.NewGuid();
            StartDate = startDate;
            EndDate = endDate;
            IsCancelled = false;
        }

        internal void Cancel()
        {
            IsCancelled = true;
        }
    }
}
