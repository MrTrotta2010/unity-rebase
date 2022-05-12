using System;

public class MismatchedArticulationsExcpetion : Exception
{
    public string Patterns { get; }
    public MismatchedArticulationsExcpetion() { }

    public MismatchedArticulationsExcpetion(string message)
        : base(message) { }

    public MismatchedArticulationsExcpetion(string message, Exception inner)
        : base(message, inner) { }

    public MismatchedArticulationsExcpetion(string message, int[] articulationsA, int[] articulationsB)
    : this(message)
    {
        Patterns = $"[{string.Join(", ", articulationsA)}] and [{string.Join(", ", articulationsB)}]";
    }
}