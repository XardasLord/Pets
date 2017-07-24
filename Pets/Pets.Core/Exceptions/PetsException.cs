using System;

namespace Pets.Core.Exceptions
{
    public class PetsException : Exception
    {
        public string Code { get; }

        protected PetsException()
        {
        }

        public PetsException(string code)
        {
            Code = code;
        }

        public PetsException(string message, params object[] args) : this(string.Empty, message, args)
        {
        }

        public PetsException(string code, string message, params object[] args) : this(null, code, message, args)
        {
        }

        public PetsException(Exception innerException, string message, params object[] args) 
            : this(innerException, string.Empty, message, args)
        {
        }

        public PetsException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
