using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour {

    // Reference to the first reel first position to add the other slots depending on the number of reels and figures.
    public GameObject reel1Slot1Reference;

    // The minimum amount of time a reel spins before stopping in milliseconds.
    private int minSpinTime = 2000;
    // The maximum amount of time a reel spins before stopping in milliseconds.
    private int maxSpinTime = 4000;
    // The short delay between one reel and the next start and stop spinning.
    private float delayBetweenReels = 0.2f;

    // Reel spinning controls.
    private float nextSpinFinish = 0f;
    private bool spinning = false;
    private float spinSpeed;
    // The speed at which the reel rotates. The higher the number the slower the speed.
    private float reelSpeedDivisor = 10f;
    private float spinDuration = 2f;
    private List<GameObject> reelControls = new List<GameObject>();

    // Sprite and graphical control.
    private float pixelsPerUnit = 100f;
    private Dictionary<Figure, Sprite> figureToSprite;
    // Order in layer for the figures so they are above the background but below the foreground.
    private int figureSortingOrder = 5;
    private Vector2 slotRectSize;
    private float initialMidPosition;

    // Winning credits.
    private WinChecker winChecker = new WinChecker();

    // Boxes to show wins.
    public GameObject line1Boxes;
    public GameObject line2Boxes;
    public GameObject line3Boxes;

    // Winning text.
    public Text winningText;

    private void Awake() {
        // Load sprites to prepare the figures in the reel.
        var bell = Resources.Load<Sprite>("Figures/1");
        var watermelon = Resources.Load<Sprite>("Figures/2");
        var grapes = Resources.Load<Sprite>("Figures/3");
        var plum = Resources.Load<Sprite>("Figures/4");
        var orange = Resources.Load<Sprite>("Figures/5");
        var lemon = Resources.Load<Sprite>("Figures/6");
        var cherry = Resources.Load<Sprite>("Figures/7");

        // Get what sprite needs each figure, so after loading reel configuration we can assign sprites.
        figureToSprite = new Dictionary<Figure, Sprite>() {
            { Figure.REIMU, bell },
            { Figure.CIRNO, watermelon },
            { Figure.MEILING, grapes },
            { Figure.SAKUYA, plum },
            { Figure.REMILIA, orange },
            { Figure.FLANDRE, lemon },
            { Figure.YOUMU, cherry }
        };

        // Size of the figure in the reel for reference.
        var spriteRenderer = reel1Slot1Reference.GetComponent<SpriteRenderer>();
        slotRectSize = spriteRenderer.sprite.rect.size;
        pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        spinSpeed = -(slotRectSize.y / (reelSpeedDivisor * pixelsPerUnit));
        initialMidPosition = reel1Slot1Reference.transform.position.y - slotRectSize.y / pixelsPerUnit;
    }

    private void Start() {
        // The first is prepared for reference in the canvas.
        var slotIndex = 0;
        var reelIndex = 0;
        var xIncrement = slotRectSize.x / pixelsPerUnit;
        var yIcnrement = slotRectSize.y / pixelsPerUnit;
        GameObject reelParent;
        GameObject figure;
        SpriteRenderer figureSpriteRenderer;
        float xPosition;
        float yPosition;

        foreach (var reelConfiguration in ReelConfiguration.reelList) {
            var figures = new List<Tuple<Figure, GameObject>>();
            // Cada gameobject con figure debería ir en una tupla con su figure
            reelParent = new GameObject("reel" + (reelIndex + 1));
            reelParent.transform.position = reel1Slot1Reference.transform.position;
            reelParent.AddComponent<ReelControl>();
            reelControls.Add(reelParent);

            foreach (var figureType in reelConfiguration) {
                // Create the Game Object that holds the figure sprite.
                figure = new GameObject("reel" + (reelIndex + 1) + "slot" + (slotIndex + 1));
                figureSpriteRenderer = figure.AddComponent<SpriteRenderer>();
                figure.transform.parent = reelParent.transform;
                figure.GetComponent<SpriteRenderer>().sprite = figureToSprite[figureType];
                // Position the figures with the initial reference.
                xPosition = reel1Slot1Reference.transform.position.x + xIncrement * reelIndex + (xIncrement / 14f) * reelIndex;
                yPosition = reel1Slot1Reference.transform.position.y - yIcnrement * slotIndex;
                figure.transform.position = new Vector2(xPosition, yPosition);
                figureSpriteRenderer.sortingOrder = figureSortingOrder;
                // Finally add the figure Game Object to the Reel which gets added to the Reel List.
                figures.Add(Tuple.Create(figureType, figure.gameObject));
                slotIndex++;
            }
            reelParent.GetComponent<ReelControl>().SetReelConfiguration(figures, initialMidPosition, slotRectSize.y / pixelsPerUnit);
            // Before the game can start, the last figure needs to be above the first for the upcoming transition.
            reelParent.GetComponent<ReelControl>().MoveLastFigureToFirst();
            slotIndex = 0;
            reelIndex++;
        }
    }

    public void Spin() {
        if (spinning) {
            return;
        }

        // Set the state to a new spin.
        spinning = true;
        winningText.text = "";
        HideAllWinLines();

        // Get a random spin duration.
        var rand = new System.Random();
        spinDuration = rand.Next(minSpinTime, maxSpinTime) / 1000f;

        // Set spin duration for each reel slightly apart.
        var start = Time.time;
        var spinFinish = start + spinDuration;
        foreach (var reelParent in reelControls) {
            reelParent.GetComponent<ReelControl>().Spin(start, spinFinish, spinSpeed);
            start += delayBetweenReels;
            spinFinish += delayBetweenReels;
        }

        // Here we know when all reels have stopped spinning.
        nextSpinFinish = spinFinish;
    }

    private void HideAllWinLines() {
        var boxes = new List<GameObject>();
        foreach (Transform box in line1Boxes.transform) {
            box.gameObject.GetComponent<WinBoxControl>().Hide();
        }
        foreach (Transform box in line2Boxes.transform) {
            box.gameObject.GetComponent<WinBoxControl>().Hide();
        }
        foreach (Transform box in line3Boxes.transform) {
            box.gameObject.GetComponent<WinBoxControl>().Hide();
        }
    }

    private void FixedUpdate() {
        if (spinning && Time.time >= nextSpinFinish) {
            spinning = false;
            var creditsWon = 0;

            // Get current combination to check prizes with in the top line.
            var combination = new List<Figure>();
            foreach (var reelParent in reelControls) {
                combination.Add(reelParent.GetComponent<ReelControl>().GetFirstFigure());
            }
            var winMatches = winChecker.MatchPrize(combination);
            if (winMatches.Count > 0) {
                // Only checking the first match on the line.
                var boxes = new List<GameObject>();
                foreach (Transform box in line1Boxes.transform) {
                    boxes.Add(box.gameObject);
                }
                boxes[0].GetComponent<WinBoxControl>().DrawStartContinuingBox();
                var match = winMatches[0];
                for (var i = match.start + 1; i < match.length - 1; i++) {
                    boxes[i].GetComponent<WinBoxControl>().DrawContinuation();
                }
                boxes[match.length - 1].GetComponent<WinBoxControl>().DrawEnd();
                creditsWon += match.credits;
            }

            // Now in the center.
            combination = new List<Figure>();
            foreach (var reelParent in reelControls) {
                combination.Add(reelParent.GetComponent<ReelControl>().GetCentralFigure());
            }
            winMatches = winChecker.MatchPrize(combination);
            if (winMatches.Count > 0) {
                // Only checking the first match on the line.
                var boxes = new List<GameObject>();
                foreach (Transform box in line2Boxes.transform) {
                    boxes.Add(box.gameObject);
                }
                boxes[0].GetComponent<WinBoxControl>().DrawStartContinuingBox();
                var match = winMatches[0];
                for (var i = match.start + 1; i < match.length - 1; i++) {
                    boxes[i].GetComponent<WinBoxControl>().DrawContinuation();
                }
                boxes[match.length - 1].GetComponent<WinBoxControl>().DrawEnd();
                creditsWon += match.credits;
            }

            // Finally the bottom line.
            combination = new List<Figure>();
            foreach (var reelParent in reelControls) {
                combination.Add(reelParent.GetComponent<ReelControl>().GetThirdFigure());
            }
            winMatches = winChecker.MatchPrize(combination);
            if (winMatches.Count > 0) {
                // Only checking the first match on the line.
                var boxes = new List<GameObject>();
                foreach (Transform box in line3Boxes.transform) {
                    boxes.Add(box.gameObject);
                }
                boxes[0].GetComponent<WinBoxControl>().DrawStartContinuingBox();
                var match = winMatches[0];
                for (var i = match.start + 1; i < match.length - 1; i++) {
                    boxes[i].GetComponent<WinBoxControl>().DrawContinuation();
                }
                boxes[match.length - 1].GetComponent<WinBoxControl>().DrawEnd();
                creditsWon += match.credits;
            }

            winningText.text = "Credits won: " + creditsWon;
        }
    }
}
