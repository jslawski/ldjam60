using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadState : PlayerState
{
    public override void EnterState(PlayerCharacterController characterController)
    {
        base.EnterState(characterController);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        this.characterController.DisableHUD();

        TextSequenceManager.instance.StartTextSequence(this.characterController.currentTextSequence);

        this.characterController.StopMovement();
    }

    public override void UpdateState()
    {
        if (PlayerControlsManager.instance.leftClickInitiated == true)
        { 
            PlayerControlsManager.instance.leftClickInitiated = false;
            TextSequenceManager.instance.RegisterClick();
        }

        if (TextSequenceManager.instance.sequenceFinished == true)
        {
            if (this.characterController.lookObjectStack.Count > 0)
            {
                this.characterController.ChangeState(new LookState());
            }
            else
            {
                this.characterController.ChangeState(new MoveState());
            }
        }
    }
}
