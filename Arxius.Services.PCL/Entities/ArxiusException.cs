using Arxius.DataAccess.PCL;
using System;

namespace Arxius.Services.PCL.Entities
{
    public class ArxiusException : Exception
    {
        public ArxiusException() { }
        public ArxiusException(ArxiusDataException e) : base(e.Message, e.InnerException) { }
    }
    public class ArxiusFileException : Exception
    {
        public ArxiusFileException(string message):base(message) { }
    }
}
