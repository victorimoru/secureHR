namespace SecureHR.Core.Domains
{
    public record Fullname
    {
        public string Title { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public Fullname(string firstName, string lastName, string title = "")
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
            }

            FirstName = firstName;
            LastName = lastName;
            Title = title ?? ""; 
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                return $"{FirstName} {LastName}";
            }

            return $"{Title} {FirstName} {LastName}";
        }
    }
}
