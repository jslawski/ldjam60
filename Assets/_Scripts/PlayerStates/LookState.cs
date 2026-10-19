using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookState : PlayerState
{
    public override void EnterState(PlayerCharacterController characterController)
    {
        base.EnterState(characterController);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        this.characterController.DisableHUD();        
    }

    public override void UpdateState()
    {
        /*
        Vector3 newCameraPosition = Input.mousePosition;
        newCameraPosition.z = 1.0f;

        newCameraPosition = Camera.main.ScreenToWorldPoint(newCameraPosition);
        newCameraPosition.z = Camera.main.transform.position.z;

        Camera.main.transform.position = newCameraPosition;
        */

        if (PlayerControlsManager.instance.leftClickInitiated == true)
        {
            PlayerControlsManager.instance.leftClickInitiated = false;
            this.characterController.TryPickupStickyNote();

            if (this.characterController.currentStickyNote == null)
            {
                this.characterController.ChangeState(new FocusState());
            }
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

        if (PlayerControlsManager.instance.backPressInitiated == true)
        {
            PlayerControlsManager.instance.backPressInitiated = false;
            this.characterController.ExitLookTransform();
        }
    }
}
