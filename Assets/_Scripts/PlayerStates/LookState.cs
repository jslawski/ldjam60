using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookState : PlayerState
{
    private Vector2 minCameraPosition = new Vector2(-0.1f, -0.1f);
    private Vector2 maxCameraPosition = new Vector2(0.1f, 0.1f);

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

        this.UpdateCameraPosition();

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

        if (PlayerControlsManager.instance.backPressInitiated == true || PlayerControlsManager.instance.rightClickInitiated == true)
        {
            PlayerControlsManager.instance.backPressInitiated = false;
            PlayerControlsManager.instance.rightClickInitiated = false;
            this.characterController.ExitLookTransform();
        }
    }

    private void UpdateCameraPosition()
    {    
        Vector2 mouseViewportPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float xPosition = Mathf.Lerp(this.minCameraPosition.x, this.maxCameraPosition.x, mouseViewportPosition.x);
        float yPosition = Mathf.Lerp(this.minCameraPosition.y, this.maxCameraPosition.y, mouseViewportPosition.y);

        Camera.main.transform.localPosition = new Vector3(xPosition, yPosition + 0.465f, Camera.main.transform.localPosition.z);        
    }
}
