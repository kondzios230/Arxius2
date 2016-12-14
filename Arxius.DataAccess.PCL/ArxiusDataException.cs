using System;

namespace Arxius.DataAccess.PCL
{
    public class ArxiusDataException : Exception
    {
        public ArxiusDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
