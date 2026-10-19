using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFigure : InteractObject
{

    public override void Interact()
    {
        base.Interact();

        AnomalyManager.instance.TriggerScriptedSequence();
    }
}
