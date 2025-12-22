namespace Market.Application.Common.Exceptions;


/// <summary>
/// Represents an error that occurs when an authenticated user attempts
/// to perform an action for which they do not have sufficient permissions.
///
/// This exception is used for authorization failures where the user is
/// logged in, but the requested operation is restricted based on roles,
/// ownership, or access rules.
///
/// Examples:
/// - A non-admin user attempting to create or delete security questions
/// - A user trying to modify resources they do not own
/// </summary>

public sealed class MarketForbiddenException : Exception
{
    public int StatusCode {  get; set; }

    public MarketForbiddenException(int statusCode=403,string message= "You do not have permission to perform this action.") : base(message) 
    {
        StatusCode = statusCode;
    }
}
