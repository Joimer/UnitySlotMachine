using System.Collections.Generic;
using UnityEngine;

public class WinChecker {

    public List<WinCombination> winningPatterns = new List<WinCombination>();

    public WinChecker() {
        // Load first the configured win combinations.
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.BELL, Figure.BELL, Figure.BELL, Figure.BELL }, 100));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.BELL, Figure.BELL, Figure.BELL }, 75));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.BELL, Figure.BELL }, 70));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.PLUM, Figure.PLUM, Figure.PLUM, Figure.PLUM }, 40));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.PLUM, Figure.PLUM, Figure.PLUM }, 20));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.PLUM, Figure.PLUM }, 10));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.CHERRY, Figure.CHERRY, Figure.CHERRY, Figure.CHERRY }, 10));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.CHERRY, Figure.CHERRY, Figure.CHERRY }, 5));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.CHERRY, Figure.CHERRY, }, 2));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.WATERMELON, Figure.WATERMELON, Figure.WATERMELON, Figure.WATERMELON }, 60));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.WATERMELON, Figure.WATERMELON, Figure.WATERMELON }, 30));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.WATERMELON, Figure.WATERMELON }, 20));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.ORANGE, Figure.ORANGE, Figure.ORANGE, Figure.ORANGE }, 30));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.ORANGE, Figure.ORANGE, Figure.ORANGE }, 15));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.ORANGE, Figure.ORANGE }, 10));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.GRAPES, Figure.GRAPES, Figure.GRAPES, Figure.GRAPES }, 50));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.GRAPES, Figure.GRAPES, Figure.GRAPES }, 20));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.GRAPES, Figure.GRAPES }, 10));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.LEMON, Figure.LEMON, Figure.LEMON, Figure.LEMON }, 20));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.LEMON, Figure.LEMON, Figure.LEMON }, 10));
        winningPatterns.Add(new WinCombination(new List<Figure>() { Figure.LEMON, Figure.LEMON }, 5));

        // After all the winning patterns have been added, we are gonna order it by number of figures.
        // This forces the matcher to find from the most to the least figures without having to keep track of matched patterns and comparing their length / credits.
        winningPatterns.Sort((win1, win2) => win1.figureCount < win2.figureCount ? 1 : -1);
    }

    public List<PatternMatch> MatchPrize(List<Figure> pattern) {
        var found = new List<PatternMatch>();
        foreach (var winCombo in winningPatterns) {
            // Pattern always should be bigger than the win combination.
            int matches = 0;
            int winComboIndex = 0;
            for (var i = 0; i < winCombo.figureCount || i < pattern.Count; i++) {
                if (winCombo.figures[winComboIndex].Equals(pattern[i])) {
                    matches++;
                } else {
                    matches = 0;
                    winComboIndex = 0;
                }
                if (matches == winCombo.figureCount) {
                    found.Add(new PatternMatch(i, winCombo.figureCount, winCombo.credits));
                }
            }
        }

        return found;
    }
}
