namespace LibraryManagement.Application.Common;

internal sealed class ValidationCollector
{
    private readonly Dictionary<string, List<string>> _errors = new(StringComparer.OrdinalIgnoreCase);

    public void Add(string field, string message)
    {
        if (!_errors.TryGetValue(field, out var messages))
        {
            messages = [];
            _errors[field] = messages;
        }

        messages.Add(message);
    }

    public void ThrowIfAny()
    {
        if (_errors.Count == 0)
        {
            return;
        }

        throw new ApplicationValidationException(
            _errors.ToDictionary(error => error.Key, error => error.Value.ToArray()));
    }
}
