using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NoteTakingApp.Core.Exceptions
{
    internal class InvalidNoteException : Exception
    {
        public InvalidNoteException() : base() { }
        public InvalidNoteException(string message) : base(message) { }
        public InvalidNoteException(string message, Exception? innerException) : base(message, innerException) { }

    }
}
