public class InvalidResourceIdException : Exception
{
    public InvalidResourceIdException() : base() { }
    public InvalidResourceIdException(string message) : base(message) { }
    public InvalidResourceIdException(string message, Exception? innerException) : base(message, innerException) { }
}
