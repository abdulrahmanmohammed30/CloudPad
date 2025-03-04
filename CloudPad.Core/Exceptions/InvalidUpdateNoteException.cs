namespace CloudPad.Core.Exceptions;

public class InvalidUpdateNoteException:Exception
{
    public InvalidUpdateNoteException() : base() { }
    public InvalidUpdateNoteException(string message) : base(message) { }
    public InvalidUpdateNoteException(string message, Exception? innerException) : base(message, innerException) { }
}