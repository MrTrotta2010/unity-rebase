using System;

namespace ReBase
{
    public class RepeatedArticulationException : Exception
    {
        public string Patterns { get; }
        public RepeatedArticulationException() { }

        public RepeatedArticulationException(string message)
            : base(message) { }

        public RepeatedArticulationException(string message, Exception inner)
            : base(message, inner) { }

        public RepeatedArticulationException(string message, int[] articulations)
        : this(message)
        {
            Patterns = $"[{string.Join(", ", articulations)}]";
        }
    }
}