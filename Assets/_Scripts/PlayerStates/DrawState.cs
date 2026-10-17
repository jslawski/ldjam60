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
            this.characterController.ChangeState(new PlaceState());
        }

        if (this.characterController.currentStickyNote == null)
        {
            this.characterController.ChangeState(new MoveState());
        }
    }
}
