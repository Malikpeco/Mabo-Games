namespace Market.Application.Common.Exceptions;

public sealed class MarketForbiddenException : Exception
{
    public int StatusCode {  get; set; }

    public MarketForbiddenException(int statusCode=403,string message= "You do not have permission to perform this action.") : base(message) 
    {
        StatusCode = statusCode;
    }
}
