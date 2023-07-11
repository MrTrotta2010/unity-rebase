using System;

namespace ReBase
{
    public class MissingAttributeException : Exception
    {
        public string Patterns { get; }
        public MissingAttributeException() { }

        public MissingAttributeException(string message)
            : base(message) { }

        public MissingAttributeException(string message, Exception inner)
            : base(message, inner) { }

        public MissingAttributeException(string message, string attribute)
        : this(message)
        {
            Patterns = attribute;
        }
    }
}