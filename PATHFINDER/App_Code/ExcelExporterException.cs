using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    public class ExcelExporterException : Exception
    {
        public ExcelExporterException() : base() { }
        public ExcelExporterException(String message) : base(message) { }
        public ExcelExporterException(String message, Exception inner) : base(message, inner) { }
    }
}
