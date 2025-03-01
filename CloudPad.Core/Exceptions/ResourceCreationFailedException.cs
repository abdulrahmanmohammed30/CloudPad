namespace CloudPad.Core.Exceptions;

public class ResourceCreationFailedException:Exception
{
    public ResourceCreationFailedException() : base() { }
    public ResourceCreationFailedException(string message) : base(message) { }
    public ResourceCreationFailedException(string message, Exception? innerException) : base(message, innerException) { }

}