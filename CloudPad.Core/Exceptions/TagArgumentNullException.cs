namespace NoteTakingApp.Core.Exceptions
{
    public class TagArgumentNullException : Exception
    {
        public TagArgumentNullException() : base() { }
        public TagArgumentNullException(string message) : base(message) { }
        public TagArgumentNullException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
