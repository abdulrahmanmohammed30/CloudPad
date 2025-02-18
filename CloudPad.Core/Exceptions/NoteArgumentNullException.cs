namespace NoteTakingApp.Core.Exceptions
{
    public class NoteArgumentNullException : Exception
    {
        public NoteArgumentNullException() : base() { }
        public NoteArgumentNullException(string message) : base(message) { }
        public NoteArgumentNullException(string message, Exception? innerException) : base(message, innerException) { }
    }
}


