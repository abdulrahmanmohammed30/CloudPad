namespace CloudPad.Core.Exceptions;

public class NoteColumnNotSearchable:Exception
{
    public NoteColumnNotSearchable():base() {}
    public NoteColumnNotSearchable(string message):base(message) {}
    public NoteColumnNotSearchable(string message,Exception? innerException):base(message,innerException) {}
} 