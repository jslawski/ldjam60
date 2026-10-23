using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlaceState : PlayerState
{
    private Vector2 minCameraPosition = new Vector2(-0.15f, -0.05f);
    private Vector2 maxCameraPosition = new Vector2(0.15f, 0.05f);

    public override void EnterState(PlayerCharacterController characterController)
    {
        base.EnterState(characterController);

        Cursor.visible = false;

        this.characterController.currentStickyNote.GetComponent<StickyNote>().SetToPlacing();      
    }

    public override void UpdateState()
    {
        this.UpdateCameraPosition();

        if (PlayerControlsManager.instance.spacePressInitiated == true)
        {
            PlayerControlsManager.instance.spacePressInitiated = false;
            this.characterController.HoldObject(this.characterController.currentStickyNote);
            this.characterController.ChangeState(new DrawState());
        }

        if (PlayerControlsManager.instance.leftClickInitiated == true)
        {
            PlayerControlsManager.instance.leftClickInitiated = false;
            this.characterController.AttemptPostStickyNote();
        }

        if (PlayerControlsManager.instance.rightClickInitiated == true)
        {
            PlayerControlsManager.instance.rightClickInitiated = false;
            this.characterController.DiscardStickyNote();
        }

        if (this.characterController.currentStickyNote == null)
        {
            this.characterController.ChangeState(new LookState());
        }
    }

    public override void FixedUpdateState()
    {
        this.characterController.UpdateStickyNoteCurrentPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector2 mouseViewportPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float xPosition = Mathf.Lerp(this.minCameraPosition.x, this.maxCameraPosition.x, mouseViewportPosition.x);
        float yPosition = Mathf.Lerp(this.minCameraPosition.y, this.maxCameraPosition.y, mouseViewportPosition.y);

        Camera.main.transform.localPosition = new Vector3(xPosition, yPosition + 0.465f, Camera.main.transform.localPosition.z);
    }
}
