namespace Bittly.Configurations
{
    public enum ErrorType
    {
        Failure = 0,      // General error (500 Internal Server Error)
        Validation = 1,   // Bad input (400 Bad Request)
        NotFound = 2,     // Resource missing (404 Not Found)
        Conflict = 3,     // Business rule violation, e.g., Alias taken (409 Conflict)
        Unauthorized = 4  // Authentication/Authorization issues (401/403)
    }

    public record Error(string Code, string Description, ErrorType Type)
    {
        // A constant representing the absence of an error
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

        // Static factory methods for cleaner instantiation

        public static Error Failure(string code, string description) =>
            new(code, description, ErrorType.Failure);

        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorType.NotFound);

        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorType.Conflict);

        public static Error Validation(string code, string description) =>
            new(code, description, ErrorType.Validation);

        public static Error Unauthorized(string code, string description) =>
            new(code, description, ErrorType.Unauthorized);
    }
}