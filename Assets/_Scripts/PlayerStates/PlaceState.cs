using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceState : PlayerState
{
    private float _timeToSetup = 0.0f;

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
            this.characterController.ChangeState(new MoveState());
        }
    }

    public override void FixedUpdateState()
    {
        this.characterController.UpdateCameraRotation();
        this.characterController.UpdateLateralVelocity();

        if (this._timeToSetup > 0.0f)
        {
            this._timeToSetup -= Time.fixedDeltaTime;
        }
        else
        {
            this.characterController.UpdateStickyNoteCurrentPosition();
        }
    }
}
