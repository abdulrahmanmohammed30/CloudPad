using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Exceptions
{
    public class InvalidTagIdException : Exception
    {
        public InvalidTagIdException() : base() { }
        public InvalidTagIdException(string message) : base(message) { }
        public InvalidTagIdException(string message, Exception? innerException) : base(message, innerException) { }

    }
}
