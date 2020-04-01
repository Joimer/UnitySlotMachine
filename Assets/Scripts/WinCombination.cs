using System.Collections.Generic;

public class WinCombination {

    public List<Figure> figures { get; private set; }
    public int credits { get; private set; }
    public int figureCount { get; private set; }

    public WinCombination(List<Figure> consecutiveFigures, int credits) {
        figures = consecutiveFigures;
        figureCount = consecutiveFigures.Count;
        this.credits = credits;
    }
}
