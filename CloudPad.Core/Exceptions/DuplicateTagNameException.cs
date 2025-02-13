namespace NoteTakingApp.Core.Exceptions;

public class DuplicateTagNameException:Exception
{
    public DuplicateTagNameException():base() {}
    public DuplicateTagNameException(string message):base(message) {}
    public DuplicateTagNameException(string message,Exception? innerException):base(message,innerException) {}

}