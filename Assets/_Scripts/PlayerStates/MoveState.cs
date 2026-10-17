using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    public override void EnterState(PlayerCharacterController characterController)
    {
        base.EnterState(characterController);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void ExitState()
    {
        this.characterController.StopMovement();
    }

    public override void UpdateState()
    {
        this.characterController.UpdateLatestMoveDirection();
        
        if (PlayerControlsManager.instance.leftClickInitiated == true)
        {
            PlayerControlsManager.instance.leftClickInitiated = false;
            //this.characterController.ChangeState(new LookState());

            //this.characterController.DiscardStickyNote();
        }
        
        if (PlayerControlsManager.instance.spacePressInitiated == true)
        { 
            PlayerControlsManager.instance.spacePressInitiated = false;
            this.characterController.InstantiateStickyNote();
        }

        if (this.characterController.currentStickyNote != null)
        {
            this.characterController.ChangeState(new DrawState());
        }
    }

    public override void FixedUpdateState()
    {
        this.characterController.UpdateCameraRotation();
        this.characterController.UpdateLateralVelocity();
    }
}
