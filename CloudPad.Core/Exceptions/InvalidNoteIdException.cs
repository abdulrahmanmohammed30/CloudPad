using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPad.Core.Exceptions
{
    public class InvalidNoteIdException:Exception
    {
        public InvalidNoteIdException() : base() { }
        public InvalidNoteIdException(string message) : base(message) { }
        public InvalidNoteIdException(string message, Exception? innerException) : base(message, innerException) { }

    }
}
