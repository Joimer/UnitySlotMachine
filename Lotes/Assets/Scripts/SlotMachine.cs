using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotMachine : MonoBehaviour {

    // Reference to the first reel first position to add the other slots depending on the number of reels and figures.
    public GameObject reel1Slot1Reference;

    // The minimum amount of time a reel spins before stopping in milliseconds.
    private int minSpinTime = 2000;
    // The maximum amount of time a reel spins before stopping in milliseconds.
    private int maxSpinTime = 4000;
    // The short delay between one reel and the next start and stop spinning.
    private float delayBetweenReels = 0.2f;
    private float nextSpinFinish = 0f;
    private bool spinning = false;
    // Game reels.
    //private List<Reel> reels = new List<Reel>();

    // Order in layer for the figures so they are above the background but below the foreground.
    private int figureSortingOrder = 5;
    // Number of reels in play.
    private int reelAmount = 5;

    private Vector2 slotRectSize;

    private Dictionary<Figure, Sprite> figureToSprite;

    private GameObject lastSlotReel1Ref;

    private List<List<GameObject>> reelsGameObject = new List<List<GameObject>>();

    private List<List<Figure>> reelsList = new List<List<Figure>>();

    private float maxMoveSpeed;

    // The speed at which the reel rotates. The higher the number the slower the speed.
    private float reelSpeedDivisor = 10f;

    private float pixelsPerUnit = 100f;

    private float reelUnitsMoved = 0f;

    private float spinDuration = 2f;

    private void Awake() {
        // Load sprites to prepare the figures in the reel.
        var bell = Resources.Load<Sprite>("Figures/1");
        var watermelon = Resources.Load<Sprite>("Figures/2");
        var grapes = Resources.Load<Sprite>("Figures/3");
        var plum = Resources.Load<Sprite>("Figures/4");
        var orange = Resources.Load<Sprite>("Figures/5");
        var lemon = Resources.Load<Sprite>("Figures/6");
        var cherry = Resources.Load<Sprite>("Figures/7");

        figureToSprite = new Dictionary<Figure, Sprite>() {
            { Figure.BELL, bell },
            { Figure.WATERMELON, watermelon },
            { Figure.GRAPES, grapes },
            { Figure.PLUM, plum },
            { Figure.ORANGE, orange },
            { Figure.LEMON, lemon },
            { Figure.CHERRY, cherry }
        };

        reelsGameObject = new List<List<GameObject>>();

        // Size of the figure in the reel for reference.
        var spriteRenderer = reel1Slot1Reference.GetComponent<SpriteRenderer>();
        slotRectSize = spriteRenderer.sprite.rect.size;
        pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        maxMoveSpeed = -(slotRectSize.y / (reelSpeedDivisor * pixelsPerUnit));

        // Assign the figure order for each reel as configured.
        reelsList = new List<List<Figure>>() {
            new List<Figure>() {
                Figure.ORANGE,
                Figure.BELL,
                Figure.WATERMELON,
                Figure.CHERRY,
                Figure.PLUM,
                Figure.LEMON,
                Figure.GRAPES,
                Figure.PLUM,
                Figure.BELL,
                Figure.BELL,
                Figure.ORANGE,
                Figure.GRAPES
            },
            new List<Figure>() {
                Figure.WATERMELON,
                Figure.CHERRY,
                Figure.BELL,
                Figure.PLUM,
                Figure.CHERRY,
                Figure.GRAPES,
                Figure.ORANGE,
                Figure.LEMON,
                Figure.LEMON,
                Figure.LEMON,
                Figure.CHERRY,
                Figure.LEMON,
                Figure.PLUM,
                Figure.LEMON,
                Figure.CHERRY
            },
            new List<Figure>() {
                Figure.GRAPES,
                Figure.WATERMELON,
                Figure.PLUM,
                Figure.GRAPES,
                Figure.BELL,
                Figure.LEMON,
                Figure.CHERRY,
                Figure.BELL,
                Figure.BELL,
                Figure.BELL,
                Figure.ORANGE,
                Figure.ORANGE,
                Figure.GRAPES
            },
            new List<Figure>() {
                Figure.LEMON,
                Figure.PLUM,
                Figure.PLUM,
                Figure.LEMON,
                Figure.GRAPES,
                Figure.ORANGE,
                Figure.WATERMELON,
                Figure.WATERMELON,
                Figure.BELL,
                Figure.CHERRY,
                Figure.CHERRY,
                Figure.LEMON,
                Figure.ORANGE,
                Figure.PLUM,
                Figure.LEMON
            },
            new List<Figure>() {
                Figure.GRAPES,
                Figure.CHERRY,
                Figure.BELL,
                Figure.WATERMELON,
                Figure.ORANGE,
                Figure.ORANGE,
                Figure.PLUM,
                Figure.PLUM,
                Figure.ORANGE,
                Figure.ORANGE,
                Figure.GRAPES,
                Figure.BELL,
                Figure.WATERMELON,
                Figure.CHERRY
            }
        };
    }

    void Start() {
        // The first is prepared for reference in the canvas.
        var slotIndex = 0;
        var reelIndex = 0;
        //reel1Slot.GetComponent<SpriteRenderer>().sprite = asdf[reel1[0]];
        var previousPos = reel1Slot1Reference.transform.position.y;

        var xIncrement = slotRectSize.x / pixelsPerUnit;
        var yIcnrement = slotRectSize.y / pixelsPerUnit;

        GameObject go;
        SpriteRenderer figureSpriteRenderer;
        float xPosition;
        float yPosition;

        foreach (var reel in reelsList) {
            reelsGameObject.Add(new List<GameObject>());
            foreach (var figure in reel) {
                go = new GameObject("reel" + (reelIndex + 1) + "slot" + (slotIndex + 1));
                figureSpriteRenderer = go.AddComponent<SpriteRenderer>();
                go.transform.parent = reel1Slot1Reference.transform.parent;
                go.GetComponent<SpriteRenderer>().sprite = figureToSprite[figure];
                xPosition = reel1Slot1Reference.transform.position.x + xIncrement * reelIndex + (xIncrement / 14f) * reelIndex;
                yPosition = reel1Slot1Reference.transform.position.y - yIcnrement * slotIndex;
                go.transform.position = new Vector2(xPosition, yPosition);
                figureSpriteRenderer.sortingOrder = figureSortingOrder;
                reelsGameObject[reelIndex].Add(go.gameObject);
                slotIndex++;
            }
            MoveLastFigureToFirst(reelsGameObject[reelIndex]);
            slotIndex = 0;
            reelIndex++;
        }
    }

    public void Spin() {
        if (spinning) {
            return;
        }

        var rand = new System.Random();
        spinDuration = rand.Next(minSpinTime, maxSpinTime) / 1000f;
        nextSpinFinish = Time.time + spinDuration;


        // asdf
        var spinStartTieme = nextSpinFinish;
        spinning = true;

        /*foreach (Reel reel in reels) {
            spinStartTieme += delayBetweenReels;
        }*/
    }

    void FixedUpdate() {
        if (spinning) {
            if (Time.time >= nextSpinFinish) {
                // Calcular el offset que llevas de movimiento del último y seguir moviendo hasta que el offset sea igual a rectsize.y
                spinning = false;
            } else {
                foreach (var reel in reelsGameObject) {
                    foreach (var figure in reel) {
                        figure.transform.Translate(new Vector2(0f, maxMoveSpeed));
                    }
                }
                reelUnitsMoved += Math.Abs(maxMoveSpeed);
                if (reelUnitsMoved > slotRectSize.y / pixelsPerUnit) {
                    reelUnitsMoved -= slotRectSize.y / pixelsPerUnit;
                    foreach (var reel in reelsGameObject) {
                        MoveLastFigureToFirst(reel);
                    }
                }
            }
        }
    }

    private void MoveLastFigureToFirst(List<GameObject> reel) {
        var first = reel.First();
        var last = reel.Last();
        last.transform.position = new Vector2(first.transform.position.x, first.transform.position.y + slotRectSize.y / pixelsPerUnit);
        reel.Remove(last);
        reel.Insert(0, last);
    }
}
