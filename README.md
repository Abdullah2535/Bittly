Bittly is the RESTful URL shortener API built with ASP.NET Core and C#, centered on clean API design, resilience, and explicit error handling.

Key architectural components and implementation details of the project:

    Domain & Flow Control: Implemented the Result pattern to replace control-flow exceptions with explicit, typed success and error outcomes (Result<T>), paired with Fluent Validation pipeline behaviors to intercept invalid URLs and payloads early.

    Data Access & Persistence: Entity Framework Core with SQL Server, configuring entities for URL mappings, collision-resistant short-code generation (Base62 encoding/hashing), and click-tracking analytics, wrapped behind a Unit of Work and repository pattern.

    Security & Authorization: Secure endpoints using ASP.NET Core Identity and JWT (JSON Web Tokens) for bearer authentication, user registration, role/claim validation, and user-specific URL ownership.

    Observability & Diagnostics: Structured logging configured via Serilog, coupled with custom global error-handling middleware to sanitize and standardize outgoing RFC 7807 problem details.
