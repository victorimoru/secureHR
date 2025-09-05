namespace SecureHR.Core.Domains
{
    // This is a Value Object. It has no identity, only value.
    public record ContactInfo(string Street, string City, string PostalCode, string PhoneNumber);
}
