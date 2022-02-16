using System;
using System.Globalization;

namespace Infrastructure.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException() : base()
        {

        }

        public ServiceException(string message) : base(message)
        {

        }

        public ServiceException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
