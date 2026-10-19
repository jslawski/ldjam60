using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEnd : InteractObject
{
    public override void Interact()
    {
        base.Interact();

        Application.Quit();
    }
}
