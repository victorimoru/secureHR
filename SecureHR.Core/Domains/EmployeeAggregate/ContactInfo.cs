using System.Net.Mail;

namespace SecureHR.Core.Domains.EmployeeAggregate
{
    public record ContactInfo
    {
        public string Street { get; }
        public string City { get; }
        public string PostalCode { get; }
        public string PhoneNumber { get; }
        public string Email { get; }

        public ContactInfo(string street, string city, string postalCode, string phoneNumber, string email)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street is required.", nameof(street));

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City is required.", nameof(city));

            if (string.IsNullOrWhiteSpace(postalCode))
                throw new ArgumentException("Postal code is required.", nameof(postalCode));

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number is required.", nameof(phoneNumber));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format.", nameof(email));

            Street = street;
            City = city;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
