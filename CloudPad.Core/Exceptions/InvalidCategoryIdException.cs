using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPad.Core.Exceptions
{
    public class InvalidCategoryIdException : Exception
    {
        public InvalidCategoryIdException() : base() { }
        public InvalidCategoryIdException(string message) : base(message) { }
        public InvalidCategoryIdException(string message, Exception? innerException) : base(message, innerException) { }

    }
}
