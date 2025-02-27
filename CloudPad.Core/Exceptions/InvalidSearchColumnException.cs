namespace CloudPad.Core.Exceptions;

public class InvalidSearchColumnException:Exception
{
    public InvalidSearchColumnException():base() {}
    public InvalidSearchColumnException(string message):base(message) {}
    public InvalidSearchColumnException(string message,Exception? innerException):base(message,innerException) {}
} 