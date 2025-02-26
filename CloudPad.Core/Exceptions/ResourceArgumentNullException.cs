namespace CloudPad.Core.Exceptions
{
    public class ResourceArgumentNullException : Exception
    {
        public ResourceArgumentNullException() : base() { }
        public ResourceArgumentNullException(string message) : base(message) { }
        public ResourceArgumentNullException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
