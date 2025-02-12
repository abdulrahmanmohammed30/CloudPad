namespace NoteTakingApp.Core.Exceptions;

public class NoteColumnNotSortable:Exception
{
    public NoteColumnNotSortable():base() {}
    public NoteColumnNotSortable(string message):base(message) {}
    public NoteColumnNotSortable(string message,Exception? innerException):base(message,innerException) {}
} 