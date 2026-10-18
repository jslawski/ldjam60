using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawState : PlayerState
{
    public override void EnterState(PlayerCharacterController characterController)
    {
        base.EnterState(characterController);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        this.characterController.currentStickyNote.GetComponent<StickyNote>().PrepareToDraw();

        this.characterController.DisableHUD();
    }

    public override void UpdateState()
    {
        if (PlayerControlsManager.instance.rightClickInitiated == true)
        {
            PlayerControlsManager.instance.rightClickInitiated = false;
            this.characterController.DiscardStickyNote();
        }

        if (PlayerControlsManager.instance.spacePressInitiated == true)
        {
            PlayerControlsManager.instance.spacePressInitiated = false;

            if (this.characterController.lookObjectStack.Count > 0)
            {
                this.characterController.ChangeState(new LookPlaceState());                
            }
            else
            {
                this.characterController.ChangeState(new PlaceState());
            }
        }

        if (this.characterController.currentStickyNote == null)
        {
            this.characterController.ChangeState(new MoveState());
        }
    }
}
