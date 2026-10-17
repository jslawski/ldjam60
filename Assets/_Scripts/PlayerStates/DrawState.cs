using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawState : PlayerState
{
    public override void EnterState(PlayerCharacterController characterController)
    {
        base.EnterState(characterController);

        this.characterController.InstantiateStickyNote();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
