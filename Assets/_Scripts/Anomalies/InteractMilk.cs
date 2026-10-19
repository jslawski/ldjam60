using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMilk : InteractObject
{
    public GameObject cookies;
    

    public override void Interact()
    {
        base.Interact();

        this.cookies.SetActive(true);
        this.gameObject.SetActive(false);

        AnomalyManager.instance.ForceNextScriptedAnomaly();

        
    }
}
