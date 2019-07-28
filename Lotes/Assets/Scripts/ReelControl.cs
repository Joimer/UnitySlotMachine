using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReelControl : MonoBehaviour {

    private List<GameObject> figures;
    private float initialMidPosition;
    private bool spinning = false;
    private float startedSpinning = 0f;
    private float nextSpinStop = 0f;
    private float spinSpeed = 0f;
    private float reelUnitsMoved = 0f;
    private float figureVerticalSize = 0f;

    public void SetReelConfiguration(List<GameObject> figures, float initialMidPosition, float figureVerticalSize) {
        this.figures = figures;
        this.initialMidPosition = initialMidPosition;
        this.figureVerticalSize = figureVerticalSize;
    }

    public void Spin(float startTime, float endTime, float spinSpeed) {
        startedSpinning = startTime;
        nextSpinStop = endTime;
        spinning = true;
        this.spinSpeed = spinSpeed;
    }

    private void FixedUpdate() {
        if (spinning && Time.time >= startedSpinning) {
            var thirdFigure = figures[1];
            var posY = thirdFigure.transform.position.y;
            var posYAfter = thirdFigure.transform.position.y + spinSpeed;

            if (Time.time >= nextSpinStop && posY > initialMidPosition && posYAfter <= initialMidPosition) {
                // This makes the last update to move the figures to the exact same initial position.
                // The leftover is usually trivial, but it's better to be sure.
                MoveAllFigures(posY - initialMidPosition);
                spinning = false;
            } else {
                MoveAllFigures(spinSpeed);
                // Position the last figure above the first one so it rolls up smoothly.
                reelUnitsMoved += Math.Abs(spinSpeed);
                if (reelUnitsMoved > figureVerticalSize) {
                    reelUnitsMoved -= figureVerticalSize;
                    MoveLastFigureToFirst();
                }
            }
        }
    }

    private void MoveAllFigures(float verticalMovement) {
        foreach (var figure in figures) {
            figure.transform.Translate(new Vector2(0f, verticalMovement));
        }
    }

    public void MoveLastFigureToFirst() {
        var first = figures.First();
        var last = figures.Last();
        last.transform.position = new Vector2(first.transform.position.x, first.transform.position.y + figureVerticalSize);
        figures.Remove(last);
        figures.Insert(0, last);
    }
}
