using UnityEngine;

public class SpinButton : MonoBehaviour {

    // Assign the Slot Machine component from the editor.
    public SlotMachine slotMachine;

    private void OnMouseDown() {
        if (slotMachine != null) {
            slotMachine.Spin();
        }
    }
}
