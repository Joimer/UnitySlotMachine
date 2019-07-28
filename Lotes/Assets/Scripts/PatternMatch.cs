using UnityEngine;

public class PatternMatch 
{
    public int start { private set; get; }
    public int length { private set; get; }
    public float credits { private set; get; }

    public PatternMatch(int start, int length, float credits) {
        this.start = start;
        this.length = length;
        this.credits = credits;
    }
}
