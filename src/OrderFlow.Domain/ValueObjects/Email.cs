using System.Text.RegularExpressions;
using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.ValueObjects;

public partial record Email
{
    public string Value { get; }
    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        var minLength = FieldLengths.EmailMinLength;
        var maxLength = FieldLengths.EmailMaxLength;

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(
                "Email.Empty",
                "Email cannot be null or empty."
            );
        }

        if (value.Length < minLength || value.Length > maxLength)
        {
            throw new DomainValidationException(
                "Email.InvalidLength",
                $"The email must be between {minLength} and {maxLength} characters long.",
                minLength,
                maxLength
            );
        }

        if (!IsValidEmail(value))
        {
            throw new DomainValidationException(
                "Email.InvalidFormat",
                "Invalid email format."
            );
        }

        return new Email(value);
    }

    private static bool IsValidEmail(string email) => MyRegex().IsMatch(email);

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex MyRegex();
}
