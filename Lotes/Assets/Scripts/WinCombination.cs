using System.Collections.Generic;

public class WinCombination {

    public List<Figure> figures { get; private set; }
    public float credits { get; private set; }
    public int figureCount { get; private set; }

    public WinCombination(List<Figure> consecutiveFigures, float credits) {
        this.figures = consecutiveFigures;
        figureCount = consecutiveFigures.Count;
    }
}
