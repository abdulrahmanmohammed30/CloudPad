namespace CloudPad.Core.Exceptions;

public class InvalidCreateNoteException:Exception
{
    public InvalidCreateNoteException() : base() { }
    public InvalidCreateNoteException(string message) : base(message) { }
    public InvalidCreateNoteException(string message, Exception? innerException) : base(message, innerException) { }
}