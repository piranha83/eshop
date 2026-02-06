namespace Infrastructure.Core;

///<inheritdoc/>
public class ValidationException(IDictionary<string, string[]> errors) : Exception;