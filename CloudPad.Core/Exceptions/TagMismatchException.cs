namespace NoteTakingApp.Core.Exceptions;

public class TagMismatchException:Exception
{
    public TagMismatchException():base() {}
    public TagMismatchException(string message):base(message) {}
    public TagMismatchException(string message,Exception? innerException):base(message,innerException) {}
}