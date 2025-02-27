namespace CloudPad.Core.Exceptions;

public class InvalidateEmailRequestException:Exception
{
    public InvalidateEmailRequestException() : base() { }
    public InvalidateEmailRequestException(string message) : base(message) { }
    public InvalidateEmailRequestException(string message, Exception? innerException) : base(message, innerException) { }

}