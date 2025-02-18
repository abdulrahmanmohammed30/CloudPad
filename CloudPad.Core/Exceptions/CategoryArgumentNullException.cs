namespace NoteTakingApp.Core.Exceptions
{
    public class CategoryArgumentNullException : Exception
    {
        public CategoryArgumentNullException() : base() { }
        public CategoryArgumentNullException(string message) : base(message) { }
        public CategoryArgumentNullException(string message, Exception? innerException) : base(message, innerException) { }
    }
}

