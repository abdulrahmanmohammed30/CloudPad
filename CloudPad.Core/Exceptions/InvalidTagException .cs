using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Exceptions
{
    public class InvalidTagException:Exception
    {
        public InvalidTagException() : base() { }
        public InvalidTagException(string message) : base(message) { }
        public InvalidTagException(string message, Exception? innerException) : base(message, innerException) { }

    }
}
