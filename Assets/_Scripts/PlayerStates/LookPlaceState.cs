using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlaceState : PlayerState
{
    public override void EnterState(PlayerCharacterController characterController)
    {
        base.EnterState(characterController);

        Cursor.visible = false;

        this.characterController.currentStickyNote.GetComponent<StickyNote>().SetToPlacing();
    }

    public override void UpdateState()
    {
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
}
