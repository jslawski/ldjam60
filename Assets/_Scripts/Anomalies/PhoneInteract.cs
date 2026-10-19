using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneInteract : InteractObject
{
    public override void Interact()
    {
        base.Interact();

        AnomalyManager.instance.TriggerScriptedSequence();
    }
}
