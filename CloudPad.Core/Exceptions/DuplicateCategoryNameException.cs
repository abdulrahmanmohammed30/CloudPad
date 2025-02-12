namespace NoteTakingApp.Core.Exceptions;

public class DuplicateCategoryNameException:Exception
{
    public DuplicateCategoryNameException():base() {}
    public DuplicateCategoryNameException(string message):base(message) {}
    public DuplicateCategoryNameException(string message,Exception? innerException):base(message,innerException) {}

}