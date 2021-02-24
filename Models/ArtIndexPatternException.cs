using System;

public class ArtIndexPatternExcpetion : Exception
{
    public string Patterns { get; }
    public ArtIndexPatternExcpetion() { }

    public ArtIndexPatternExcpetion(string message)
        : base(message) { }

    public ArtIndexPatternExcpetion(string message, Exception inner)
        : base(message, inner) { }

    public ArtIndexPatternExcpetion(string message, string SessionPattern, string RegisterPattern)
    : this(message)
    {
        Patterns = SessionPattern + " and " + RegisterPattern;
    }
}