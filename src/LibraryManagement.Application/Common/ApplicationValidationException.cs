namespace LibraryManagement.Application.Common;

public sealed class ApplicationValidationException : Exception
{
    public ApplicationValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public IDictionary<string, string[]> Errors { get; }
}
