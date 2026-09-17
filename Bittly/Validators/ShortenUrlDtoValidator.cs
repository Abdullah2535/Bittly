using Bittly.Dtos;
using FluentValidation;
using System;
using System.Globalization;

namespace Bittly.Validators
{
    public class ShortenUrlDtoValidator : AbstractValidator<ShortenUrlDto>
    {
        public ShortenUrlDtoValidator()
        {
            // 1. Validate the Long URL
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("The URL is required.")
                .Must(BeAValidUrl).WithMessage("The URL format is invalid. Must be HTTP or HTTPS.");

            // 2. Validate the Custom Alias (ShortUrl)
            RuleFor(x => x.ShortUrl)
                .MaximumLength(6).WithMessage("The custom alias cannot exceed 6 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.ShortUrl)); 


            // 3. Expiration Date Rules
            RuleFor(x => x.ExpirationDate)
                .Must(BeAValidDateFormat)
                .WithMessage("Invalid date format. Please use exactly YYYY-MM-DD.")
                .DependentRules(() =>
                {
                    // This only runs if BeAValidDateFormat returns true
                    RuleFor(x => x.ExpirationDate)
                        .Must(BeAFutureDate)
                        .WithMessage("Expiration date must be in the future.");
                })
                .When(x => !string.IsNullOrWhiteSpace(x.ExpirationDate)); // Skip if null/empty
        }

        //  Validates format without throwing exceptions
        private bool BeAValidDateFormat(string? date)
        {
            return DateOnly.TryParseExact(
                date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        // Validates future timeline
        private bool BeAFutureDate(string? date)
        {
            var parsedDate = DateOnly.ParseExact(date!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endOfDay = parsedDate.ToDateTime(new TimeOnly(23, 59, 59), DateTimeKind.Utc);

            return endOfDay > DateTime.UtcNow;
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri? outUri)
                   && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}