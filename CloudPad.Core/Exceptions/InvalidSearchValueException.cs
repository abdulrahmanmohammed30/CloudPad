namespace CloudPad.Core.Exceptions;


public class InvalidSearchValueException:Exception
{
    public InvalidSearchValueException():base() {}
    public InvalidSearchValueException(string message):base(message) {}
    public InvalidSearchValueException(string message,Exception? innerException):base(message,innerException) {}
} 