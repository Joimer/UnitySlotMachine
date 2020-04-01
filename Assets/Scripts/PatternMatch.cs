using System.Collections.Generic;

public class PatternMatch 
{
    public List<Figure> pattern { private set; get; }
    public int start { private set; get; }
    public int length { private set; get; }
    public int credits { private set; get; }

    public PatternMatch(List<Figure> pattern, int start, int length, int credits) {
        this.pattern = pattern;
        this.start = start;
        this.length = length;
        this.credits = credits;
    }
}
