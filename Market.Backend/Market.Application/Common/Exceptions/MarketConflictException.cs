namespace Market.Application.Common.Exceptions;


/// <summary>
/// Represents an error that occurs when a request cannot be completed
/// due to a conflict with the current state of the system.
///
/// This exception is used for business rule violations where the
/// requested operation is valid in principle, but cannot be executed
/// because it would result in an inconsistent or invalid state.
///
/// Examples:
/// - Attempting to add a security question that is already registered
/// - Registering more items than the allowed maximum
/// - Performing an operation that contradicts existing data
/// </summary>


public sealed class MarketConflictException : Exception
{
    public MarketConflictException(string message) : base(message) { }
}
