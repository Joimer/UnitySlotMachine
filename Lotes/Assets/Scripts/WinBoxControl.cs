using UnityEngine;

public class WinBoxControl : MonoBehaviour {

    public GameObject verticalLeft;
    public GameObject horizontalMiddle;
    public GameObject verticalRight;
    public GameObject horizontalContinue;

    private void Start() {
        Hide();
    }

    public void Hide() {
        DrawLines(false, false, false, false);
    }

    public void DrawBox() {
        DrawLines(true, true, true, true);
    }

    public void DrawStartContinuingBox() {
        DrawLines(true, true, false, true);
    }

    public void DrawContinuation() {
        DrawLines(false, true, false, true);
    }

    public void DrawEnd() {
        DrawLines(false, true, true, false);
    }

    private void DrawLines(bool vLeft, bool hMid, bool vRight, bool hContinue) {
        if (verticalLeft != null) {
            verticalLeft.GetComponent<SpriteRenderer>().enabled = vLeft;
        }
        if (horizontalMiddle != null) {
            horizontalMiddle.GetComponent<SpriteRenderer>().enabled = hMid;
        }
        if (verticalRight != null) {
            verticalRight.GetComponent<SpriteRenderer>().enabled = vRight;
        }
        if (horizontalContinue != null) {
            horizontalContinue.GetComponent<SpriteRenderer>().enabled = hContinue;
        }
    }
}
