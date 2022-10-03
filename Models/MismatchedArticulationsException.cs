using System;

namespace ReBase
{
    public class MismatchedArticulationsException : Exception
    {
        public string Patterns { get; }
        public MismatchedArticulationsException() { }

        public MismatchedArticulationsException(string message)
            : base(message) { }

        public MismatchedArticulationsException(string message, Exception inner)
            : base(message, inner) { }

        public MismatchedArticulationsException(string message, int[] articulationsA, int[] articulationsB)
        : this(message)
        {
            Patterns = $"[{string.Join(", ", articulationsA)}] and [{string.Join(", ", articulationsB)}]";
        }
    }
}