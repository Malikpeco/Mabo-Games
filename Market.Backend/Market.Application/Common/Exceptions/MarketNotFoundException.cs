namespace Market.Application.Common.Exceptions;


/// <summary>
/// Represents an error that occurs when a requested resource
/// cannot be found.
///
/// This exception is used when an entity does not exist, has been
/// deleted, or is not accessible in the current context.
///
/// Examples:
/// - Attempting to update or delete a resource with an invalid identifier
/// - Accessing a resource that belongs to another user
/// </summary>
public sealed class MarketNotFoundException : Exception
{
    public MarketNotFoundException(string message) : base(message) { }
}
