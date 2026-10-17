using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerCharacterController characterController; 

    public virtual void EnterState(PlayerCharacterController characterController)
    { 
        this.characterController = characterController;

        PlayerControlsManager.instance.ResetAllInitiatedInputs();
    }
    
    public virtual void ExitState()
    { 
    
    }

    public virtual void UpdateState()
    { 
    
    }

    public virtual void FixedUpdateState()
    { 
    
    }
}
