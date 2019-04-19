using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitor
{
    public class ProcessingException : Exception
    {
        public ProcessingException(string message) : base(message) { }
    }

    public class FileSizeProcessingException : ProcessingException
    {
        public FileSizeProcessingException(string message) : base(message) { }
    }
}
